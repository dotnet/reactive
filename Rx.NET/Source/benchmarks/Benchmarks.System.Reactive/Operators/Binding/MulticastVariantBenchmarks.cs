// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>
    /// S2: multicast overloads with a distinct sink or subject from those in <see cref="PublishBenchmarks"/>,
    /// <see cref="ReplayBenchmarks"/> and <see cref="RefCountBenchmarks"/>, each with one subscriber:
    /// <list type="bullet">
    /// <item><c>Publish_Selector</c> — <c>Publish(selector)</c>, the self-connecting <c>Multicast&lt;TSource,
    /// TIntermediate, TResult&gt;</c> sink over a fresh <c>Subject</c> (identity selector). <c>Multicast(subjectSelector,
    /// selector)</c> with a <c>Subject</c> is the same code, so it is not measured separately.</item>
    /// <item><c>Publish_InitialValue</c> — <c>Publish(initialValue)</c>, a connectable over a <c>BehaviorSubject</c>.</item>
    /// <item><c>Replay_Selector</c> — <c>Replay(selector)</c>, the <c>Multicast</c> sink over an unbounded
    /// <c>ReplaySubject</c>, which buffers all N elements.</item>
    /// <item><c>Replay_BufferSize_Window</c> — <c>Replay(bufferSize, window, scheduler)</c>, the time-and-count bounded
    /// <c>ReplayByTime</c> subject. It delivers through a scheduled observer on the virtual scheduler, so the N
    /// elements queue during <c>Connect()</c> and are drained by <c>Start()</c>; the virtual clock does not move during
    /// <c>Connect()</c>, so trimming is by the 64-element bound only.</item>
    /// <item><c>RefCount_DisconnectDelay</c> — <c>RefCount(disconnectDelay, scheduler)</c>, the <c>RefCount.Lazy</c>
    /// sink: after the subscriber completes, the disconnect is deferred by a virtual-time timer that <c>Start()</c>
    /// fires.</item>
    /// </list>
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class MulticastVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly TimeSpan ReplayWindow = TimeSpan.FromTicks(1_000);
        private static readonly TimeSpan DisconnectDelay = TimeSpan.FromTicks(100);

        [Benchmark]
        public void Publish_Selector() =>
            Observable.Range(1, N).Publish(static xs => xs).SubscribeConsume(Consumer);

        [Benchmark]
        public void Publish_InitialValue()
        {
            var connectable = Observable.Range(1, N).Publish(0);
            var subscription = connectable.SubscribeConsume(Consumer);

            using (connectable.Connect())
            {
                subscription.Dispose();
            }
        }

        [Benchmark]
        public void Replay_Selector() =>
            Observable.Range(1, N).Replay(static xs => xs).SubscribeConsume(Consumer);

        [Benchmark]
        public void Replay_BufferSize_Window()
        {
            var scheduler = new HistoricalScheduler();
            var connectable = Observable.Range(1, N).Replay(64, ReplayWindow, scheduler);
            var subscription = connectable.SubscribeConsume(Consumer);

            using (connectable.Connect())
            {
                scheduler.Start();
                subscription.Dispose();
            }
        }

        [Benchmark]
        public void RefCount_DisconnectDelay()
        {
            var scheduler = new HistoricalScheduler();

            Observable.Range(1, N).Publish().RefCount(DisconnectDelay, scheduler).SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
