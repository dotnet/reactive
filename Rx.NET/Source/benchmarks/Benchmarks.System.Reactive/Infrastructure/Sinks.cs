// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// An <see cref="IObserver{T}"/> that funnels every <c>OnNext</c> into a BenchmarkDotNet <see cref="Consumer"/>
    /// so results can't be eliminated as dead code. <c>OnError</c> is <b>consumed</b> (not swallowed), so error-path
    /// benchmarks measure real work and never rethrow into the benchmark.
    /// </summary>
    public sealed class ConsumingObserver<T> : IObserver<T>
    {
        private readonly Consumer _consumer;

        public ConsumingObserver(Consumer consumer) => _consumer = consumer;

        public void OnNext(T value) => _consumer.Consume(value);

        public void OnError(Exception error) => _consumer.Consume(error);

        public void OnCompleted() { }

        /// <summary>True when this observer feeds <paramref name="consumer"/> (reference identity).</summary>
        internal bool Wraps(Consumer consumer) => ReferenceEquals(_consumer, consumer);
    }

    /// <summary>
    /// A <see cref="ConsumingObserver{T}"/> that also blocks until the sequence terminates — for asynchronous
    /// pipelines (e.g. <c>ObserveOn</c>/<c>SubscribeOn</c>) whose completion happens on another thread.
    /// </summary>
    public sealed class BlockingObserver<T> : IObserver<T>, IDisposable
    {
        private readonly Consumer _consumer;
        private readonly ManualResetEventSlim _done = new(false);

        public BlockingObserver(Consumer consumer) => _consumer = consumer;

        public void OnNext(T value) => _consumer.Consume(value);

        public void OnError(Exception error)
        {
            _consumer.Consume(error);
            _done.Set();
        }

        public void OnCompleted() => _done.Set();

        public void Wait() => _done.Wait();

        public void Dispose() => _done.Dispose();
    }

    public static class SinkExtensions
    {
        /// <summary>Subscribes a <see cref="ConsumingObserver{T}"/> that feeds every value (and any error) to <paramref name="consumer"/>.</summary>
        /// <remarks>
        /// The observer is reused rather than allocated per call (see <see cref="ObserverCache{T}"/>), so the
        /// benchmark's Allocated column reflects only the pipeline under test — otherwise a constant observer
        /// allocation would pollute every N=1 row and the scalar-creation benchmarks.
        /// </remarks>
        public static IDisposable SubscribeConsume<T>(this IObservable<T> source, Consumer consumer) =>
            source.Subscribe(ObserverCache<T>.For(consumer));

        /// <summary>
        /// Remembers the most recently created <see cref="ConsumingObserver{T}"/> per element type. Sharing one
        /// instance across subscriptions (even concurrent ones) is safe because the observer has no state beyond its
        /// immutable <see cref="Consumer"/> reference. A benchmark class uses a single <see cref="Consumer"/>, so after
        /// the first call the fast path is one static read plus a reference comparison, with no allocation.
        /// </summary>
        /// <remarks>
        /// The unsynchronised read/write is a benign race: reference reads and writes are atomic, the field is read
        /// once per call, and the returned observer is always checked to wrap the caller's consumer. If two consumers
        /// alternate (e.g. concurrent benchmarks in one process) each call simply falls back to allocating a new
        /// observer — still correct, just not allocation-free. <see cref="BlockingObserver{T}"/> is stateful (it owns
        /// a wait handle) and is deliberately not cached.
        /// </remarks>
        private static class ObserverCache<T>
        {
            private static ConsumingObserver<T> _last;

            public static ConsumingObserver<T> For(Consumer consumer)
            {
                var last = _last;
                if (last != null && last.Wraps(consumer))
                {
                    return last;
                }

                var created = new ConsumingObserver<T>(consumer);
                _last = created;
                return created;
            }
        }

        /// <summary>Subscribes, consumes every value, and blocks until the sequence terminates. For async pipelines.</summary>
        public static void SubscribeBlocking<T>(this IObservable<T> source, Consumer consumer)
        {
            using var observer = new BlockingObserver<T>(consumer);
            using (source.Subscribe(observer))
            {
                observer.Wait();
            }
        }
    }
}
