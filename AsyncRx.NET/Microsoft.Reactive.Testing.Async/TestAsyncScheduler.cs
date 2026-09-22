// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Reactive.Concurrency;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Virtual-time scheduler for testing AsyncRx.NET, the async counterpart of
/// <see cref="TestScheduler"/>. One virtual tick equals one <see cref="TimeSpan"/> tick,
/// matching the sync scheduler's conventions.
/// </summary>
/// <remarks>
/// <para>
/// The pump implements a canonical continuation-priority schedule: continuations readied
/// during a dispatch run ahead of any other work due at the same virtual time. It is one
/// logical thread over two structures: a timer queue keyed
/// by absolute virtual tick, and a same-tick ready deque. Determinism does not come from
/// context capture — AsyncRx operator code awaits with <c>ConfigureAwait(false)</c>
/// throughout — but from a structural invariant: all asynchrony in a test originates from
/// harness primitives, and only the pump thread completes them, so continuations resume
/// inline (depth-first) on the pump thread when the pump completes the thing they await.
/// </para>
/// <para>
/// The pump runs with <see cref="SynchronizationContext.Current"/> cleared. This is load
/// bearing, not an omission: the runtime refuses to inline await continuations while a
/// custom SynchronizationContext is current (see AwaitTaskContinuation.IsValidLocationForInlining),
/// so installing a pump context would punt every ConfigureAwait(false) continuation to the
/// thread pool. With no context, every awaiter — configured or not — resumes inline. It also
/// matches production AsyncRx on TaskPoolAsyncScheduler, where operator code runs contextless.
/// (A non-default <see cref="TaskScheduler"/> would block inlining in exactly the same way, so
/// that is not an option either.)
/// </para>
/// <para>
/// Code that escapes anyway (Task.Yield, Task.Run, real timers) is detected and reported as
/// a failure rather than being silently tolerated. Detection cannot rely on inspecting a
/// work item's task after the fact — an escaped continuation can complete it on a pool thread
/// before the pump looks, which would make the escape indistinguishable from synchronous
/// completion. Instead the pump marks every flow it runs with an <see cref="AsyncLocal{T}"/>
/// that has a change-notification handler. The runtime invokes that handler on whichever
/// thread restores a captured <see cref="ExecutionContext"/>, before the continuation body
/// runs; the handler checks it is the pump thread. So the escape is recorded before escaped
/// code can do anything observable, whether it resumes via Task.Yield, runs inside Task.Run,
/// or fires from a real timer or a foreign cancellation. Harness entry points check the
/// current thread as well, as a second line of defence.
/// </para>
/// <para>
/// Continuations that cannot run inline (forced-yield resumptions) are inserted at the front
/// of the ready deque, in causal depth-first order, so they run before any other work queued
/// at the same tick; newly scheduled immediate work joins at the back, FIFO. This makes the
/// inline and queued paths observationally equivalent: a scenario's trace is identical under
/// <see cref="ExecutionShape.SynchronousCompletion"/> and <see cref="ExecutionShape.ForcedYield"/>.
/// </para>
/// <para>
/// Note one deliberate divergence from the sync <see cref="TestScheduler"/>: work scheduled
/// for the current tick (or the past) runs within the current tick's drain phase rather than
/// being bumped to <c>Clock + 1</c>.
/// </para>
/// </remarks>
public sealed partial class TestAsyncScheduler : AsyncSchedulerBase
{
    // Timers ordered by due time, then by insertion sequence. (A SortedDictionary rather than
    // PriorityQueue because the latter is not available on netstandard2.0.)
    private readonly SortedDictionary<(long DueTime, long Sequence), TimerItem> _timers = [];
    private readonly LinkedList<Action> _ready = new();
    private LinkedListNode<Action>? _continuationCursor;

    // Work the pump has started whose ValueTask has not yet completed. Pump-thread only.
    private readonly HashSet<PendingWork> _outstanding = [];

    // Failures may be recorded from foreign threads (that is itself one of the failures we
    // detect), so this list has its own lock; everything else is pump-thread affine.
    private readonly object _failureGate = new();
    private readonly List<Exception> _failures = [];
    private readonly List<string> _diagnostics = [];
    private bool _escapeRecorded;

    // The pump running the current flow. Set inside every dispatched item, so every
    // continuation captured under the pump carries it in its ExecutionContext, and the runtime
    // invokes OnAmbientPumpChanged on whichever thread restores that context — before the
    // continuation body runs. That is what makes escape detection race-free: see the class
    // remarks and OnAmbientPumpChanged.
    private static readonly AsyncLocal<TestAsyncScheduler?> AmbientPump = new(OnAmbientPumpChanged);

    private long _nextSequence;
    private Thread? _pumpThread;
    private volatile bool _pumping;

    /// <summary>
    /// Creates a scheduler that runs all await points synchronously.
    /// </summary>
    public TestAsyncScheduler()
        : this(ExecutionShape.SynchronousCompletion)
    {
    }

    /// <summary>
    /// Creates a scheduler that runs await points according to the specified <paramref name="executionShape"/>.
    /// </summary>
    /// <param name="executionShape">
    /// Determines whether await points complete synchronously or genuinely suspend, resuming via
    /// the front of the ready deque.
    /// </param>
    public TestAsyncScheduler(ExecutionShape executionShape)
    {
        ExecutionShape = executionShape;
    }

    /// <summary>
    /// Gets the policy determining how await points behave.
    /// </summary>
    public ExecutionShape ExecutionShape { get; }

    /// <summary>
    /// Gets the current virtual time, in ticks.
    /// </summary>
    public long Clock { get; private set; }

    /// <inheritdoc/>
    public override DateTimeOffset Now => new(Clock, TimeSpan.Zero);

    /// <summary>
    /// Schedules work at an absolute virtual time. Used by the testable observables and the
    /// <c>Start</c> harness; also available to tests directly.
    /// </summary>
    /// <returns>A disposable that (synchronously) cancels the scheduled work, best effort.</returns>
    public IDisposable ScheduleAbsolute(long dueTime, Func<CancellationToken, ValueTask> action, string? description = null)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var work = new PendingWork<Func<CancellationToken, ValueTask>>(action, static (action, ct) => action(ct), description ?? "work", Math.Max(dueTime, Clock), CancellationToken.None);
        var item = new TimerItem(Isolate(() => RunUserWork(work)));

        EnqueueTimer(item, work.DueTime);

        return new AnonymousDisposable(() => item.Canceled = true);
    }

    /// <summary>
    /// Runs the pump until no work remains: drain the ready deque to quiescence, advance the
    /// clock to the next due timer, repeat. The calling thread is the pump thread for the
    /// duration, with <see cref="SynchronizationContext.Current"/> cleared so that every
    /// await continuation resumes inline (see the class remarks). Throws if any scheduled
    /// work failed, escaped to a real thread, or never completed.
    /// </summary>
    public void Start()
    {
        if (_pumping)
        {
            throw new InvalidOperationException("The virtual-time pump is already running; Start cannot be called reentrantly.");
        }

        _pumping = true;
        _pumpThread = Thread.CurrentThread;
        var previousContext = SynchronizationContext.Current;
        SynchronizationContext.SetSynchronizationContext(null);

        try
        {
            var dispatchesThisTick = 0;

            while (!HasFailures)
            {
                if (_ready.First is { } node)
                {
                    _ready.RemoveFirst();
                    _continuationCursor = null;
                    node.Value();
                    CountDispatch(ref dispatchesThisTick);
                }
                else if (DequeueTimer() is { } timer)
                {
                    var (item, key) = timer;

                    if (item.Canceled)
                    {
                        continue;
                    }

                    if (key.DueTime > Clock)
                    {
                        Clock = key.DueTime;
                        dispatchesThisTick = 0;
                    }

                    _continuationCursor = null;
                    item.Run();
                    CountDispatch(ref dispatchesThisTick);
                }
                else
                {
                    break;
                }
            }

            ThrowIfFailed();
            ThrowIfWorkOutstanding();
        }
        finally
        {
            _pumping = false;
            _pumpThread = null;
            SynchronizationContext.SetSynchronizationContext(previousContext);
        }
    }

    /// <summary>
    /// The number of work items the pump will run at one virtual tick before concluding that
    /// the scenario is looping: "fail informatively, never hang" for the one failure mode a
    /// wall-clock watchdog cannot interrupt, a pump thread that is busy forever at a single
    /// tick (e.g. an operator that re-creates a window whose closing completes instantly).
    /// Far above anything a legitimate scenario dispatches within one tick.
    /// </summary>
    public const int MaxDispatchesPerTick = 100_000;

    private void CountDispatch(ref int dispatchesThisTick)
    {
        if (++dispatchesThisTick >= MaxDispatchesPerTick)
        {
            RecordFailure(new TestAsyncSchedulerException(
                $"The virtual-time pump ran {MaxDispatchesPerTick:N0} work items at tick {Clock} without virtual time advancing; " +
                "the scenario is almost certainly looping (e.g. work that keeps rescheduling itself for the current tick). " +
                "Stopped to fail informatively rather than hang."));
        }
    }

    /// <summary>
    /// A harness await point: completes synchronously under
    /// <see cref="ExecutionShape.SynchronousCompletion"/>, genuinely suspends (resuming via
    /// the front of the ready deque, preserving canonical order) under
    /// <see cref="ExecutionShape.ForcedYield"/>. The testable observables and observers await
    /// this at each step so both async code paths get exercised.
    /// </summary>
    public YieldPointAwaitable YieldPoint() => new(this);

    /// <inheritdoc/>
    protected override ValueTask ScheduleAsyncCore<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, CancellationToken token)
    {
        var work = new PendingWork<TState>(state, action, "immediately scheduled work", Clock, token);

        if (VerifyPumpThread("IAsyncScheduler.ScheduleAsync"))
        {
            _ready.AddLast(Isolate(() => RunUserWork(work)));
        }

        return default;
    }

    /// <inheritdoc/>
    protected override ValueTask Delay(TimeSpan dueTime, CancellationToken token)
    {
        if (!VerifyPumpThread("Delay") || dueTime <= TimeSpan.Zero)
        {
            return default;
        }

        // Completed by the pump when virtual time reaches the due tick; the awaiting state
        // machine resumes inline on the pump thread (no SynchronizationContext is current,
        // so nothing blocks continuation inlining).
        var tcs = new TaskCompletionSource<bool>();
        var item = new TimerItem(Isolate(() => tcs.TrySetResult(true)));

        EnqueueTimer(item, checked(Clock + dueTime.Ticks));

        if (token.CanBeCanceled)
        {
            token.Register(() =>
            {
                item.Canceled = true;
                tcs.TrySetCanceled(token);
            });
        }

        return new ValueTask(tcs.Task);
    }

    private void EnqueueTimer(TimerItem item, long dueTime)
    {
        if (VerifyPumpThread("scheduling timed work"))
        {
            _timers.Add((dueTime, _nextSequence++), item);
        }
    }

    private (TimerItem Item, (long DueTime, long Sequence) Key)? DequeueTimer()
    {
        if (_timers.Count == 0)
        {
            return null;
        }

        // SortedDictionary enumerates in key order, so the first entry is the next timer due.
        var first = _timers.First();
        _timers.Remove(first.Key);
        return (first.Value, first.Key);
    }

    private void RunUserWork(PendingWork work)
    {
        if (work.Token.IsCancellationRequested)
        {
            return;
        }

        work.StartedAt = Clock;

        try
        {
            var task = work.Invoke();

            if (task.IsCompleted)
            {
                ObserveCompletion(task, work);
            }
            else
            {
                _outstanding.Add(work);
                task.AsTask().ContinueWith(
                    (t, state) => ((TestAsyncScheduler)state!).OnWorkCompleted(work, t),
                    this,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
            }
        }
        catch (OperationCanceledException) when (work.Token.IsCancellationRequested)
        {
            // Cancellation through the work's own token (e.g. a disposed timer) is normal, not
            // a failure. Any other OperationCanceledException is one nothing asked for, so it
            // is a failure like any other exception (handled below).
        }
        catch (Exception ex)
        {
            RecordFailure(WorkFailure(work, ex));
        }
    }

    /// <summary>
    /// Observes a completed work item's outcome: nothing for success, nothing for cancellation
    /// through the work's own token, a recorded failure for anything else — including a
    /// cancellation that the work's token did not ask for, which would otherwise let
    /// <see cref="Start"/> return normally from work that never did its job.
    /// </summary>
    private void ObserveCompletion(ValueTask task, PendingWork work)
    {
        if (!task.IsCompletedSuccessfully)
        {
            try
            {
                task.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException) when (work.Token.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                RecordFailure(WorkFailure(work, ex));
            }
        }
    }

    /// <summary>
    /// The key under which an exception thrown by scheduled work carries, in its
    /// <see cref="Exception.Data"/>, a description of that work and when it was scheduled and
    /// started. The same text is written to the test's output when <c>Start</c> fails, so a
    /// failure names the culprit (e.g. a timer that fired after the sequence terminated) rather
    /// than just the symptom — while <c>Start</c> still rethrows the original exception, as the
    /// sync <see cref="TestScheduler"/> does.
    /// </summary>
    public const string WorkDataKey = "Microsoft.Reactive.Testing.Async.Work";

    private Exception WorkFailure(PendingWork work, Exception exception)
    {
        var description = $"Scheduled work failed at tick {Clock}: {work.Describe()} threw {exception.GetType().Name}: {exception.Message}";

        try
        {
            exception.Data[WorkDataKey] = description;
        }
        catch (ArgumentException)
        {
            // Some exception types expose read-only Data; the output line below still carries it.
        }

        lock (_failureGate)
        {
            _diagnostics.Add(description);
        }

        return exception;
    }

    private void OnWorkCompleted(PendingWork work, Task task)
    {
        if (Thread.CurrentThread != _pumpThread)
        {
            // The work item's continuation chain ran (at least partly) off the pump; the
            // pump's data structures must not be touched from here. (OnAmbientPumpChanged
            // will normally have recorded the escape already, as this continuation's context
            // was restored onto the foreign thread; this is the backstop.)
            RecordEscape(work.Describe(), "completed on");
            return;
        }

        _outstanding.Remove(work);

        if (task.IsFaulted)
        {
            RecordFailure(WorkFailure(work, task.Exception!.InnerExceptions.Count == 1 ? task.Exception.InnerException! : task.Exception));
        }
        else if (task.IsCanceled)
        {
            // GetResult rethrows the original OperationCanceledException (with its token), so
            // the same rule applies as for synchronous completion: only cancellation through
            // the work's own token is benign.
            ObserveCompletion(new ValueTask(task), work);
        }
    }

    /// <summary>
    /// Inserts a continuation into the ready deque, front-first in causal depth-first order:
    /// the first continuation readied during the current dispatch goes to the very front,
    /// each subsequent one directly after the previous, all ahead of pre-existing queue
    /// content. This mirrors the nesting order that inline (ConfigureAwait(false)) completion
    /// produces, which is what makes the two paths trace-equivalent.
    /// </summary>
    internal void PostContinuation(Action continuation)
    {
        if (!VerifyPumpThread("posting a continuation"))
        {
            return;
        }

        var isolated = Isolate(continuation);

        _continuationCursor = _continuationCursor is null
            ? _ready.AddFirst(isolated)
            : _ready.AddAfter(_continuationCursor, isolated);
    }

    /// <summary>
    /// Records an escape failure if called off the pump thread while the pump is running.
    /// Observers that receive notifications under the pump (the harness's own, and any a test
    /// adapter builds) call this on every delivery, so a notification produced by work that
    /// escaped to a real thread is reported rather than silently recorded or lost.
    /// </summary>
    /// <returns>True when on the pump thread (or the pump is not running).</returns>
    public bool EnsurePumpThread(string operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        return VerifyPumpThread(operation);
    }

    private bool VerifyPumpThread(string operation)
    {
        if (_pumping && Thread.CurrentThread != _pumpThread)
        {
            RecordEscape(operation, "ran on");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Invoked by the runtime whenever <see cref="AmbientPump"/>'s value changes on a
    /// thread — including when a captured <see cref="ExecutionContext"/> is restored onto a
    /// thread, which happens before the continuation (or Task.Run body, timer callback,
    /// cancellation callback...) that captured it runs. A restore onto anything but the pump
    /// thread while the pump is running is an escape, and is recorded here before the escaped
    /// code can execute. Explicit sets (the pump marking a flow it is about to run) are not
    /// context changes and are ignored.
    /// </summary>
    private static void OnAmbientPumpChanged(AsyncLocalValueChangedArgs<TestAsyncScheduler?> args)
    {
        if (args.ThreadContextChanged && args.CurrentValue is { } scheduler)
        {
            scheduler.VerifyPumpThread("execution");
        }
    }

    /// <summary>
    /// Records an escape from the pump. Only the first escape in a run is a failure: once a
    /// flow has left the pump thread, everything it does from there (touching the harness,
    /// completing its work item) is a consequence of that same escape, so subsequent reports
    /// go to the diagnostic output rather than turning the failure into an aggregate.
    /// </summary>
    private void RecordEscape(string what, string verb)
    {
        var message = $"Work escaped the virtual-time pump: {what} {verb} thread " +
            $"'{Thread.CurrentThread.Name ?? $"#{Environment.CurrentManagedThreadId}"}' instead of the pump thread. " +
            "All asynchrony in a canonical-schedule test must originate from harness primitives " +
            "(no Task.Run, real timers, or thread-pool continuations).";

        lock (_failureGate)
        {
            if (_escapeRecorded)
            {
                _diagnostics.Add(message);
            }
            else
            {
                _escapeRecorded = true;
                _failures.Add(new TestAsyncSchedulerException(message));
            }
        }
    }

    internal void RecordFailure(Exception exception)
    {
        lock (_failureGate)
        {
            _failures.Add(exception);
        }
    }

    private bool HasFailures
    {
        get
        {
            lock (_failureGate)
            {
                return _failures.Count > 0;
            }
        }
    }

    private void ThrowIfFailed()
    {
        lock (_failureGate)
        {
            foreach (var diagnostic in _diagnostics)
            {
                Console.Out.WriteLine($"[TestAsyncScheduler] {diagnostic}");
            }

            if (_failures.Count == 1)
            {
                ExceptionDispatchInfo.Capture(_failures[0]).Throw();
            }

            if (_failures.Count > 1)
            {
                throw new AggregateException("Multiple failures occurred while running the virtual-time pump.", _failures);
            }
        }
    }

    private void ThrowIfWorkOutstanding()
    {
        if (_outstanding.Count > 0)
        {
            var detail = string.Join(Environment.NewLine, _outstanding.Select(w => "  - " + w.Describe()));

            throw new TestAsyncSchedulerException(
                $"The virtual-time pump ran out of work at tick {Clock}, but {_outstanding.Count} asynchronous operation(s) never completed:" +
                $"{Environment.NewLine}{detail}{Environment.NewLine}" +
                "The code under test is awaiting something that no virtual-time event will ever complete.");
        }
    }

    /// <summary>
    /// Runs <paramref name="action"/> under the <see cref="ExecutionContext"/> captured now, when
    /// it is queued. The thread pool does this for every work item, so an <c>AsyncLocal</c>
    /// written inside one item never leaks into the next; a single pump thread running items
    /// synchronously would otherwise let such writes accumulate across items (which broke
    /// <c>AsyncGate</c>'s AsyncLocal-based re-entrancy detection — a later work item wrongly
    /// saw the lock as held by its own flow). This keeps "what operator code sees" faithful
    /// to production for ambient state as well as for continuations.
    /// </summary>
    /// <remarks>
    /// Inside the restored context the item is marked as running under this pump (see
    /// <see cref="AmbientPump"/>). Doing this here rather than once in <see cref="Start"/>
    /// matters: an item queued before <c>Start</c> captured a context without the mark, and
    /// continuations captured inside it would otherwise be unmarked, and their escapes invisible.
    /// </remarks>
    private Action Isolate(Action action)
    {
        var context = ExecutionContext.Capture();

        return context is null
            ? action
            : () => ExecutionContext.Run(
                context,
                static state =>
                {
                    var (scheduler, action) = ((TestAsyncScheduler, Action))state!;
                    AmbientPump.Value = scheduler;
                    action();
                },
                (this, action));
    }

    private sealed class TimerItem(Action run)
    {
        public bool Canceled;
        public void Run() => run();
    }

    private abstract class PendingWork(string description, long dueTime, CancellationToken token)
    {
        public CancellationToken Token { get; } = token;
        public long DueTime { get; } = dueTime;
        public long StartedAt { get; set; } = -1;

        public abstract ValueTask Invoke();

        public string Describe() => $"{description} (scheduled for tick {DueTime}, started at tick {StartedAt})";
    }

    private sealed class PendingWork<TState>(TState state, Func<TState, CancellationToken, ValueTask> action, string description, long dueTime, CancellationToken token)
        : PendingWork(description, dueTime, token)
    {
        public override ValueTask Invoke() => action(state, Token);
    }

    private sealed class AnonymousDisposable(Action dispose) : IDisposable
    {
        public void Dispose() => dispose();
    }

    /// <summary>Awaitable returned by <see cref="YieldPoint"/>.</summary>
    public readonly struct YieldPointAwaitable(TestAsyncScheduler scheduler)
    {
        /// <summary>
        /// Gets an awaiter. Typically called by code generated for an <c>await</c> expression.
        /// </summary>
        /// <returns>An <see cref="Awaiter"/> for this yield point.</returns>
        public Awaiter GetAwaiter() => new(scheduler);

        /// <summary>
        /// The awaiter for <see cref="YieldPointAwaitable"/>. Typically used by code generated for
        /// an <c>await</c> expression.
        /// </summary>
        /// <param name="scheduler">The scheduler whose <see cref="ExecutionShape"/> determines how this awaiter behaves.</param>
        public readonly struct Awaiter(TestAsyncScheduler scheduler) : ICriticalNotifyCompletion
        {
            /// <summary>
            /// Gets a value indicating whether the awaiter has completed. Typically called by code
            /// generated for an <c>await</c> expression.
            /// </summary>
            public bool IsCompleted => scheduler.ExecutionShape == ExecutionShape.SynchronousCompletion;

            /// <summary>
            /// Called by code generated for an <c>await</c> expression when the operation completes.
            /// </summary>
            /// <remarks>
            /// For <c>void</c>-typed awaiters, there is no value to return. This exists only to
            /// enable exceptions to be thrown, but this awaiter never does that.
            /// </remarks>
            public void GetResult()
            {
            }

            /// <summary>
            /// Schedules the continuation action to be invoked when the operation completes.
            /// Typically called by code generated for an <c>await</c> expression.
            /// </summary>
            /// <param name="continuation">The action to invoke when the operation completes.</param>
            public void OnCompleted(Action continuation) => scheduler.PostContinuation(continuation);

            /// <summary>
            /// Schedules the continuation action to be invoked when the operation completes.
            /// Typically called by code generated for an <c>await</c> expression.
            /// </summary>
            /// <param name="continuation">The action to invoke when the operation completes.</param>
            public void UnsafeOnCompleted(Action continuation) => scheduler.PostContinuation(continuation);
        }
    }
}
