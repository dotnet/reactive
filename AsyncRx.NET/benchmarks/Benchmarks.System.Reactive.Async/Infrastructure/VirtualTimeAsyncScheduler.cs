// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// A virtual-time <see cref="IAsyncScheduler"/> for temporal (S3) benchmarks. It predates
    /// <c>Microsoft.Reactive.Testing.Async</c>'s <c>TestAsyncScheduler</c> and is tuned for measurement. Time only moves when <see cref="AdvanceByAsync"/> or
    /// <see cref="RunUntilAsync"/> pumps it, so a 100,000-tick window costs no wall-clock waiting.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The pump rule: <b>time never advances while a timed item is still running, unless that item has parked on
    /// a later virtual wake-up.</b>
    /// </para>
    /// <list type="bullet">
    /// <item>Items due now always run, in (due time, enqueue order) order, inline on the pump.</item>
    /// <item>
    /// A <i>timed</i> item (scheduled with a due time) whose action does not complete synchronously is awaited
    /// before time advances. Many AsyncRx timer callbacks take an <c>AsyncGate</c>, which hops to the thread
    /// pool; awaiting them keeps sequencing deterministic.
    /// </item>
    /// <item>
    /// A timed item counts as <i>parked</i>, and stops blocking, once any new future item is enqueued after it
    /// started. That is how periodic loops (<c>Timer(due, period)</c>, <c>Interval</c>, <c>Sample</c>,
    /// <c>Buffer/Window(time)</c>) behave: each tick awaits <c>scheduler.Delay(period)</c>, which enqueues the
    /// next wake-up.
    /// </item>
    /// <item>
    /// <i>Immediate</i> items (no due time, for example the <c>RendezVous</c>/<c>ExecuteAsync</c> continuations)
    /// are started but not awaited. <c>Delay</c>'s drain loop waits on a semaphore that only the source
    /// releases; awaiting it would deadlock.
    /// </item>
    /// </list>
    /// <para>
    /// Work that continues on a thread-pool thread after an immediate item started is not awaited, so for
    /// gated operators the interleaving of a timer tick with the next push can vary between runs; the amount of
    /// work does not. Use one instance per benchmark invocation and one pump at a time.
    /// </para>
    /// <para>
    /// The pump waits at most <see cref="StallTimeout"/> (environment variable
    /// <c>ASYNCRX_BENCH_STALL_TIMEOUT_MS</c>, default 30,000 ms, <c>-1</c> for infinite) for in-flight work to
    /// signal progress, then throws, so a library hang fails the benchmark instead of blocking the run.
    /// </para>
    /// </remarks>
    public sealed class VirtualTimeAsyncScheduler : IAsyncScheduler
    {
        /// <summary>Virtual time zero.</summary>
        public static readonly DateTimeOffset Epoch = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>How long the pump waits for in-flight work before it reports a stall.</summary>
        public static readonly TimeSpan StallTimeout = ReadStallTimeout();

        private readonly object _gate = new();
        private readonly PriorityQueue<Item, (long Due, long Seq)> _queue = new();
        private readonly List<Blocking> _blocking = new(); // pump-only
        private readonly SemaphoreSlim _wake = new(0);
        private readonly Action _signal;
        private long _nowTicks;
        private long _seq;
        private long _futureVersion;
        private bool _pumpWaiting;
        private bool _signalled;
        private int _pumping;
        private Exception? _fault;

        public VirtualTimeAsyncScheduler() => _signal = Signal;

        public DateTimeOffset Now => Epoch.AddTicks(Volatile.Read(ref _nowTicks));

        /// <summary>The current virtual time, in ticks since <see cref="Epoch"/>.</summary>
        public long NowTicks => Volatile.Read(ref _nowTicks);

        public ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, Func<TState, CancellationToken, ValueTask> action) =>
            Enqueue(state, action, 0, timed: false);

        public ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, TimeSpan dueTime, Func<TState, CancellationToken, ValueTask> action) =>
            Enqueue(state, action, Math.Max(0, dueTime.Ticks), timed: true);

        public ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, DateTimeOffset dueTime, Func<TState, CancellationToken, ValueTask> action) =>
            Enqueue(state, action, Math.Max(0, (dueTime - Now).Ticks), timed: true);

        /// <summary>Runs everything due up to and including now + <paramref name="by"/>, then sets the clock to that time.</summary>
        public ValueTask AdvanceByAsync(TimeSpan by) => PumpAsync(Volatile.Read(ref _nowTicks) + by.Ticks, null);

        /// <summary>Runs the timeline (advancing time as needed) until <paramref name="done"/> is set and no timed item is in flight.</summary>
        public ValueTask RunUntilAsync(CompletionSignal done)
        {
            if (done == null)
            {
                throw new ArgumentNullException(nameof(done));
            }

            done.OnSet = _signal;
            return PumpAsync(long.MaxValue, done);
        }

        private ValueTask<IAsyncDisposable> Enqueue<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, long relativeTicks, bool timed)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var item = new Item<TState>(state, action, timed);
            lock (_gate)
            {
                var due = _nowTicks + relativeTicks;
                item.Due = due;
                _queue.Enqueue(item, (due, _seq++));
                if (relativeTicks > 0)
                {
                    _futureVersion++;
                }
            }

            Signal();
            return new ValueTask<IAsyncDisposable>(item);
        }

        private async ValueTask PumpAsync(long target, CompletionSignal? done)
        {
            if (Interlocked.Exchange(ref _pumping, 1) != 0)
            {
                throw new InvalidOperationException("VirtualTimeAsyncScheduler supports one pump at a time.");
            }

            try
            {
                while (true)
                {
                    RunDue();
                    Prune();
                    ThrowIfFaulted();

                    if (_blocking.Count > 0)
                    {
                        await WaitForSignalAsync().ConfigureAwait(false);
                        continue;
                    }

                    if (done is { IsSet: true })
                    {
                        return;
                    }

                    lock (_gate)
                    {
                        if (_queue.TryPeek(out _, out var priority))
                        {
                            if (priority.Due <= _nowTicks)
                            {
                                continue; // new due-now work arrived
                            }

                            if (priority.Due <= target)
                            {
                                _nowTicks = priority.Due;
                                continue;
                            }
                        }

                        if (done == null)
                        {
                            if (target > _nowTicks)
                            {
                                _nowTicks = target;
                            }

                            return;
                        }
                    }

                    // RunUntil with nothing scheduled: wait for async work to enqueue more or for the sink to terminate.
                    await WaitForSignalAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                Volatile.Write(ref _pumping, 0);
            }
        }

        private void RunDue()
        {
            while (true)
            {
                Item item;
                lock (_gate)
                {
                    if (!_queue.TryPeek(out item!, out var priority) || priority.Due > _nowTicks)
                    {
                        return;
                    }

                    _queue.Dequeue();
                }

                if (!item.IsCancelled)
                {
                    Start(item);
                }
            }
        }

        private void Start(Item item)
        {
            var version = Interlocked.Read(ref _futureVersion);

            ValueTask pending;
            try
            {
                pending = item.Invoke(item.CreateToken());
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                RecordFault(ex);
                return;
            }

            if (pending.IsCompleted)
            {
                try
                {
                    pending.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    RecordFault(ex);
                }

                return;
            }

            var task = pending.AsTask();
            if (item.Timed)
            {
                _blocking.Add(new Blocking(task, version, item.Due));
                task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(_signal);
            }
            else
            {
                ObserveFaults(task);
            }
        }

        private void Prune()
        {
            if (_blocking.Count == 0)
            {
                return;
            }

            var futureVersion = Interlocked.Read(ref _futureVersion);
            for (var i = _blocking.Count - 1; i >= 0; i--)
            {
                var blocking = _blocking[i];
                if (blocking.Task.IsCompleted)
                {
                    if (blocking.Task.IsFaulted)
                    {
                        RecordFault(blocking.Task.Exception!.InnerException ?? blocking.Task.Exception);
                    }

                    _blocking.RemoveAt(i);
                }
                else if (blocking.Version != futureVersion)
                {
                    // Parked on a later wake-up: stop blocking, but still surface a later fault.
                    ObserveFaults(blocking.Task);
                    _blocking.RemoveAt(i);
                }
            }
        }

        private void ObserveFaults(Task task) =>
            task.ContinueWith(
                static (t, state) => ((VirtualTimeAsyncScheduler)state!).RecordFault(t.Exception!.InnerException ?? t.Exception),
                this,
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

        private void RecordFault(Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                return;
            }

            Interlocked.CompareExchange(ref _fault, ex, null);
            Signal();
        }

        private void ThrowIfFaulted()
        {
            var fault = Volatile.Read(ref _fault);
            if (fault != null)
            {
                ExceptionDispatchInfo.Capture(fault).Throw();
            }
        }

        private ValueTask WaitForSignalAsync()
        {
            lock (_gate)
            {
                if (_signalled)
                {
                    _signalled = false;
                    return default;
                }

                _pumpWaiting = true;
            }

            if (StallTimeout == Timeout.InfiniteTimeSpan)
            {
                return new ValueTask(_wake.WaitAsync());
            }

            return WaitWithTimeoutAsync();
        }

        private async ValueTask WaitWithTimeoutAsync()
        {
            if (!await _wake.WaitAsync(StallTimeout).ConfigureAwait(false))
            {
                string detail;
                lock (_gate)
                {
                    detail = string.Format(
                        CultureInfo.InvariantCulture,
                        "now={0} ticks, queued={1}, blocking timed items due at [{2}]",
                        _nowTicks,
                        _queue.Count,
                        string.Join(", ", _blocking.ConvertAll(static b => b.Due.ToString(CultureInfo.InvariantCulture))));
                }

                throw new TimeoutException($"VirtualTimeAsyncScheduler stalled for {StallTimeout.TotalMilliseconds} ms ({detail}).");
            }
        }

        private void Signal()
        {
            var release = false;
            lock (_gate)
            {
                if (_pumpWaiting)
                {
                    _pumpWaiting = false;
                    release = true;
                }
                else
                {
                    _signalled = true;
                }
            }

            if (release)
            {
                _wake.Release();
            }
        }

        private static TimeSpan ReadStallTimeout()
        {
            var raw = Environment.GetEnvironmentVariable("ASYNCRX_BENCH_STALL_TIMEOUT_MS");
            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ms))
            {
                return ms < 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromMilliseconds(ms);
            }

            return TimeSpan.FromSeconds(30);
        }

        private readonly record struct Blocking(Task Task, long Version, long Due);

        private abstract class Item : IAsyncDisposable
        {
            public readonly bool Timed;
            public long Due;
            private CancellationTokenSource? _cts;
            private int _cancelled;

            protected Item(bool timed) => Timed = timed;

            public abstract ValueTask Invoke(CancellationToken token);

            public bool IsCancelled => Volatile.Read(ref _cancelled) != 0;

            // The token source is created only when the item starts, so items cancelled while queued (Throttle's
            // superseded timers, for example) never allocate one.
            public CancellationToken CreateToken()
            {
                var cts = new CancellationTokenSource();
                Interlocked.Exchange(ref _cts, cts);
                if (IsCancelled)
                {
                    cts.Cancel();
                }

                return cts.Token;
            }

            public ValueTask DisposeAsync()
            {
                if (Interlocked.Exchange(ref _cancelled, 1) == 0)
                {
                    Volatile.Read(ref _cts)?.Cancel();
                }

                return default;
            }
        }

        // Carries the caller's state, so stateful scheduling (IAsyncScheduler's only shape since the state-argument
        // change) needs no closure. The stateless AsyncScheduler extensions pass the action itself as the state.
        private sealed class Item<TState> : Item
        {
            private readonly TState _state;
            private readonly Func<TState, CancellationToken, ValueTask> _action;

            public Item(TState state, Func<TState, CancellationToken, ValueTask> action, bool timed)
                : base(timed)
            {
                _state = state;
                _action = action;
            }

            public override ValueTask Invoke(CancellationToken token) => _action(_state, token);
        }
    }
}
