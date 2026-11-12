// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// A reusable, allocation-free completion signal with a single waiter. <see cref="Reset"/> re-arms it for the
    /// next invocation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IsSet"/> becomes true only after the core has completed, and <see cref="Reset"/> waits out a
    /// <see cref="Set"/> that is still in progress. Without both, a waiter that saw the signal set could reset
    /// it while a thread-pool thread was still completing the core, corrupting the next invocation. That is only
    /// possible for pipelines that complete on another thread, such as <c>ObserveOn(TaskPool)</c>.
    /// </para>
    /// <para>
    /// Continuations of a pending <see cref="WaitAsync"/> run asynchronously, so a <see cref="Reset"/> issued
    /// from a resumed waiter can never run inside <see cref="Set"/> and spin on it. Synchronous pipelines are
    /// unaffected: they set the signal before anyone waits, and <see cref="WaitAsync"/> returns a completed
    /// <see cref="ValueTask"/>.
    /// </para>
    /// </remarks>
    public sealed class CompletionSignal : IValueTaskSource
    {
        private const int Armed = 0;
        private const int Setting = 1;
        private const int Signalled = 2;

        private ManualResetValueTaskSourceCore<bool> _core = new() { RunContinuationsAsynchronously = true };
        private int _state;

        public bool IsSet => Volatile.Read(ref _state) == Signalled;

        public void Reset()
        {
            var spinner = new SpinWait();
            while (Volatile.Read(ref _state) == Setting)
            {
                spinner.SpinOnce();
            }

            _core.Reset();
            Volatile.Write(ref _state, Armed);
        }

        public void Set()
        {
            if (Interlocked.CompareExchange(ref _state, Setting, Armed) == Armed)
            {
                _core.SetResult(true);
                Volatile.Write(ref _state, Signalled);
            }
        }

        /// <summary>
        /// Completes when the signal is set. Returns an already-completed <see cref="ValueTask"/> when it is set
        /// already, which is the common case for pipelines that run to completion inside <c>SubscribeAsync</c>.
        /// </summary>
        public ValueTask WaitAsync() => IsSet ? default : new ValueTask(this, _core.Version);

        void IValueTaskSource.GetResult(short token) => _core.GetResult(token);

        ValueTaskSourceStatus IValueTaskSource.GetStatus(short token) => _core.GetStatus(token);

        void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) =>
            _core.OnCompleted(continuation, state, token, flags);
    }

    /// <summary>
    /// An <see cref="IAsyncObserver{T}"/> that funnels every value into a BenchmarkDotNet <see cref="Consumer"/>, so
    /// results cannot be removed as dead code, and sets <see cref="Completion"/> when the sequence terminates.
    /// <c>OnErrorAsync</c> consumes the error (it is not rethrown), so error-path benchmarks measure real work.
    /// </summary>
    /// <remarks>
    /// It implements the interface directly rather than deriving from <c>AsyncObserverBase&lt;T&gt;</c>, so the sink
    /// adds no interlocked state checks of its own.
    /// </remarks>
    public sealed class ConsumingAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly Consumer _consumer;

        public ConsumingAsyncObserver(Consumer consumer) => _consumer = consumer;

        public CompletionSignal Completion { get; } = new();

        public ValueTask OnNextAsync(T value)
        {
            _consumer.Consume(value);
            return default;
        }

        public ValueTask OnErrorAsync(Exception error)
        {
            _consumer.Consume(error);
            Completion.Set();
            return default;
        }

        public ValueTask OnCompletedAsync()
        {
            Completion.Set();
            return default;
        }

        internal bool Wraps(Consumer consumer) => ReferenceEquals(_consumer, consumer);
    }

    /// <summary>
    /// A stateless consuming observer with no completion tracking, for subscriptions that are not awaited to
    /// termination (never-completing sources, lifecycle benchmarks that dispose mid-stream). Safe to share.
    /// </summary>
    public sealed class ConsumeOnlyAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly Consumer _consumer;

        public ConsumeOnlyAsyncObserver(Consumer consumer) => _consumer = consumer;

        public ValueTask OnNextAsync(T value)
        {
            _consumer.Consume(value);
            return default;
        }

        public ValueTask OnErrorAsync(Exception error)
        {
            _consumer.Consume(error);
            return default;
        }

        public ValueTask OnCompletedAsync() => default;

        internal bool Wraps(Consumer consumer) => ReferenceEquals(_consumer, consumer);
    }

    /// <summary>
    /// A consuming observer whose <c>OnNextAsync</c> yields (<c>await Task.Yield()</c>) before consuming: a truly
    /// asynchronous consumer, used to measure how operators behave when downstream calls do not complete
    /// synchronously.
    /// </summary>
    public sealed class YieldingAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly Consumer _consumer;

        public YieldingAsyncObserver(Consumer consumer) => _consumer = consumer;

        public CompletionSignal Completion { get; } = new();

        public async ValueTask OnNextAsync(T value)
        {
            await Task.Yield();
            _consumer.Consume(value);
        }

        public ValueTask OnErrorAsync(Exception error)
        {
            _consumer.Consume(error);
            Completion.Set();
            return default;
        }

        public ValueTask OnCompletedAsync()
        {
            Completion.Set();
            return default;
        }
    }

    /// <summary>
    /// Counts what an observer receives. Used by <see cref="SelfCheck"/> to check that pipelines deliver what
    /// the benchmarks assume.
    /// </summary>
    public sealed class CountingAsyncObserver<T> : IAsyncObserver<T>
    {
        private long _count;

        public CompletionSignal Completion { get; } = new();

        public Action<T>? OnValue { get; set; }

        public long Count => Interlocked.Read(ref _count);

        public Exception? Error { get; private set; }

        public bool Completed { get; private set; }

        public ValueTask OnNextAsync(T value)
        {
            Interlocked.Increment(ref _count);
            OnValue?.Invoke(value);
            return default;
        }

        public ValueTask OnErrorAsync(Exception error)
        {
            Error = error;
            Completion.Set();
            return default;
        }

        public ValueTask OnCompletedAsync()
        {
            Completed = true;
            Completion.Set();
            return default;
        }
    }

    public static class Sinks
    {
        /// <summary>
        /// The cached <see cref="ConsumingAsyncObserver{T}"/> for <paramref name="consumer"/>, with its completion
        /// signal re-armed. One per element type: the observer is stateful, so it supports one awaited
        /// subscription at a time. Benchmarks that need several live subscribers allocate their own observers.
        /// </summary>
        /// <remarks>
        /// Only use this for subscriptions that are awaited to termination. A pipeline abandoned before it
        /// terminates could complete the shared observer during a later invocation; use
        /// <see cref="SinkExtensions.SubscribeNoWaitAsync{T}"/> (which uses the stateless observer) for those.
        /// The unsynchronised cache read/write is a benign race, as in the Rx suite's <c>ObserverCache</c>.
        /// </remarks>
        public static ConsumingAsyncObserver<T> For<T>(Consumer consumer)
        {
            var observer = Cache<T>.Last;
            if (observer == null || !observer.Wraps(consumer))
            {
                observer = new ConsumingAsyncObserver<T>(consumer);
                Cache<T>.Last = observer;
            }

            observer.Completion.Reset();
            return observer;
        }

        /// <summary>The cached stateless <see cref="ConsumeOnlyAsyncObserver{T}"/> for <paramref name="consumer"/>.</summary>
        public static ConsumeOnlyAsyncObserver<T> ConsumeOnly<T>(Consumer consumer)
        {
            var observer = Cache<T>.LastConsumeOnly;
            if (observer == null || !observer.Wraps(consumer))
            {
                observer = new ConsumeOnlyAsyncObserver<T>(consumer);
                Cache<T>.LastConsumeOnly = observer;
            }

            return observer;
        }

        private static class Cache<T>
        {
            public static ConsumingAsyncObserver<T>? Last;
            public static ConsumeOnlyAsyncObserver<T>? LastConsumeOnly;
        }
    }

    public static class SinkExtensions
    {
        /// <summary>
        /// Subscribes the cached consuming observer, then completes when the sequence terminates (completion or
        /// error). Does not dispose the subscription. Allocation-free when the pipeline runs to completion inside
        /// <c>SubscribeAsync</c>.
        /// </summary>
        public static ValueTask SubscribeConsumeAsync<T>(this IAsyncObservable<T> source, Consumer consumer)
        {
            var observer = Sinks.For<T>(consumer);
            var subscribe = source.SubscribeAsync(observer);
            if (subscribe.IsCompletedSuccessfully)
            {
                _ = subscribe.Result;
                return observer.Completion.WaitAsync();
            }

            return SlowAsync(subscribe, observer);

            static async ValueTask SlowAsync(ValueTask<IAsyncDisposable> subscribe, ConsumingAsyncObserver<T> observer)
            {
                await subscribe.ConfigureAwait(false);
                await observer.Completion.WaitAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Subscribes the cached consuming observer, awaits termination, then disposes the subscription. For
        /// operators whose disposal path is part of what is being measured.
        /// </summary>
        public static async ValueTask SubscribeConsumeDisposeAsync<T>(this IAsyncObservable<T> source, Consumer consumer)
        {
            var observer = Sinks.For<T>(consumer);
            var subscription = await source.SubscribeAsync(observer).ConfigureAwait(false);
            await observer.Completion.WaitAsync().ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        /// <summary>Subscribes the stateless consuming observer and returns the subscription without waiting for termination.</summary>
        public static ValueTask<IAsyncDisposable> SubscribeNoWaitAsync<T>(this IAsyncObservable<T> source, Consumer consumer) =>
            source.SubscribeAsync(Sinks.ConsumeOnly<T>(consumer));

        /// <summary>Subscribes <paramref name="observer"/>, then completes when its <paramref name="completion"/> is set.</summary>
        public static async ValueTask SubscribeAwaitAsync<T>(this IAsyncObservable<T> source, IAsyncObserver<T> observer, CompletionSignal completion)
        {
            await source.SubscribeAsync(observer).ConfigureAwait(false);
            await completion.WaitAsync().ConfigureAwait(false);
        }
    }
}
