// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Thrown into a benchmark when its pipeline ends with an error that the benchmark does not expect. Without it,
    /// a library change that made a pipeline fail early would show as a large speed-up instead of a failure.
    /// </summary>
    public sealed class UnexpectedBenchmarkErrorException : Exception
    {
        public UnexpectedBenchmarkErrorException(Exception error)
            : base("The pipeline under test ended with an error that the benchmark does not expect: " + error.GetType().Name + ": " + error.Message +
                   " If the row measures an error path, use FaultingSource.BenchmarkError or pass allowErrors: true.", error)
        {
        }

        /// <summary>
        /// True when an error is expected without an opt-in: <see cref="FaultingSource.BenchmarkError"/> is only
        /// ever sent on purpose by the suite's own faulting sources and <c>Throw</c> rows.
        /// </summary>
        public static bool IsExpected(Exception error, bool allowErrors) =>
            allowErrors || ReferenceEquals(error, FaultingSource.BenchmarkError);
    }

    /// <summary>
    /// An <see cref="IObserver{T}"/> that funnels every <c>OnNext</c> into a BenchmarkDotNet <see cref="Consumer"/>
    /// so results can't be eliminated as dead code. An expected error (see
    /// <see cref="UnexpectedBenchmarkErrorException.IsExpected"/>) is <b>consumed</b>, so error-path benchmarks
    /// measure real work. Any other error throws <see cref="UnexpectedBenchmarkErrorException"/> from
    /// <c>OnError</c>, which fails the benchmark.
    /// </summary>
    public sealed class ConsumingObserver<T> : IObserver<T>
    {
        private readonly Consumer _consumer;
        private readonly bool _allowErrors;

        public ConsumingObserver(Consumer consumer, bool allowErrors = false)
        {
            _consumer = consumer;
            _allowErrors = allowErrors;
        }

        public void OnNext(T value) => _consumer.Consume(value);

        public void OnError(Exception error)
        {
            _consumer.Consume(error);
            if (!UnexpectedBenchmarkErrorException.IsExpected(error, _allowErrors))
            {
                throw new UnexpectedBenchmarkErrorException(error);
            }
        }

        public void OnCompleted() { }

        /// <summary>True when this observer feeds <paramref name="consumer"/> (reference identity).</summary>
        internal bool Wraps(Consumer consumer) => ReferenceEquals(_consumer, consumer);
    }

    /// <summary>
    /// A <see cref="ConsumingObserver{T}"/> that also blocks until the sequence terminates — for asynchronous
    /// pipelines (e.g. <c>ObserveOn</c>/<c>SubscribeOn</c>) whose completion happens on another thread. An
    /// unexpected error is thrown from <see cref="Wait"/>, on the benchmark thread.
    /// </summary>
    public sealed class BlockingObserver<T> : IObserver<T>, IDisposable
    {
        private readonly Consumer _consumer;
        private readonly bool _allowErrors;
        private readonly ManualResetEventSlim _done = new(false);
        private Exception _unexpected;

        public BlockingObserver(Consumer consumer, bool allowErrors = false)
        {
            _consumer = consumer;
            _allowErrors = allowErrors;
        }

        public void OnNext(T value) => _consumer.Consume(value);

        public void OnError(Exception error)
        {
            _consumer.Consume(error);
            if (!UnexpectedBenchmarkErrorException.IsExpected(error, _allowErrors))
            {
                _unexpected = error;
            }

            _done.Set();
        }

        public void OnCompleted() => _done.Set();

        public void Wait()
        {
            _done.Wait();
            if (_unexpected != null)
            {
                throw new UnexpectedBenchmarkErrorException(_unexpected);
            }
        }

        public void Dispose() => _done.Dispose();
    }

    public static class SinkExtensions
    {
        /// <summary>Subscribes a <see cref="ConsumingObserver{T}"/> that feeds every value (and any expected error) to <paramref name="consumer"/>.</summary>
        /// <param name="source">The pipeline under test.</param>
        /// <param name="consumer">The consumer that receives every value.</param>
        /// <param name="allowErrors">
        /// True for a row that measures an error the library sends (for example <c>Timeout</c> firing). Otherwise
        /// only <see cref="FaultingSource.BenchmarkError"/> is consumed and any other error fails the benchmark.
        /// </param>
        /// <remarks>
        /// The observer is reused rather than allocated per call (see <see cref="ObserverCache{T}"/>), so the
        /// benchmark's Allocated column reflects only the pipeline under test — otherwise a constant observer
        /// allocation would pollute every N=1 row and the scalar-creation benchmarks.
        /// </remarks>
        public static IDisposable SubscribeConsume<T>(this IObservable<T> source, Consumer consumer, bool allowErrors = false) =>
            source.Subscribe(ObserverCache<T>.For(consumer, allowErrors));

        /// <summary>
        /// The cached <see cref="ConsumingObserver{T}"/> for <paramref name="consumer"/>, for a <c>Subscribe</c>
        /// overload that is not on <see cref="IObservable{T}"/> (for example <c>IEnumerable&lt;T&gt;.Subscribe</c>).
        /// </summary>
        public static IObserver<T> ConsumingObserverFor<T>(Consumer consumer, bool allowErrors = false) =>
            ObserverCache<T>.For(consumer, allowErrors);

        /// <summary>
        /// Remembers the most recently created <see cref="ConsumingObserver{T}"/> per element type, one for strict
        /// and one for error-tolerant use. Sharing one instance across subscriptions (even concurrent ones) is safe
        /// because the observer has no state beyond its immutable fields. A benchmark class uses a single
        /// <see cref="Consumer"/>, so after the first call the fast path is one static read plus a reference
        /// comparison, with no allocation.
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
            private static ConsumingObserver<T> _strict;
            private static ConsumingObserver<T> _tolerant;

            public static ConsumingObserver<T> For(Consumer consumer, bool allowErrors)
            {
                var last = allowErrors ? _tolerant : _strict;
                if (last != null && last.Wraps(consumer))
                {
                    return last;
                }

                var created = new ConsumingObserver<T>(consumer, allowErrors);
                if (allowErrors)
                {
                    _tolerant = created;
                }
                else
                {
                    _strict = created;
                }

                return created;
            }
        }

        /// <summary>Subscribes, consumes every value, and blocks until the sequence terminates. For async pipelines.</summary>
        public static void SubscribeBlocking<T>(this IObservable<T> source, Consumer consumer, bool allowErrors = false)
        {
            using var observer = new BlockingObserver<T>(consumer, allowErrors);
            using (source.Subscribe(observer))
            {
                observer.Wait();
            }
        }
    }
}
