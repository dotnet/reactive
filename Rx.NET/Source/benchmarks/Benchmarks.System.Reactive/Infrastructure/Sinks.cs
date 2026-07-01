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
        public static IDisposable SubscribeConsume<T>(this IObservable<T> source, Consumer consumer) =>
            source.Subscribe(new ConsumingObserver<T>(consumer));

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
