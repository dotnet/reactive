// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>
    /// Multicast scenarios the "all-subscribers-before-Connect" benchmarks miss: a <c>Replay</c> subscriber that
    /// attaches <b>after</b> the buffer has filled (so it exercises catch-up replay — Replay's whole point), and a
    /// <c>RefCount</c> subscribe/dispose/re-subscribe churn (0→1→0→1 connect/disconnect).
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class LateSubscriberBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Replay_LateSubscriber()
        {
            var source = new Subject<int>();
            var replayed = source.Replay();
            using var connection = replayed.Connect();

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                source.OnNext(i);   // fills the replay buffer while there are no subscribers
            }

            using (replayed.SubscribeConsume(_consumer))   // late subscriber → replays the buffered half, then goes live
            {
                for (var i = half; i < N; i++)
                {
                    source.OnNext(i);
                }

                source.OnCompleted();
            }
        }

        [Benchmark]
        public void RefCount_Churn()
        {
            var source = new Subject<int>();
            var shared = source.Publish().RefCount();

            for (var round = 0; round < N; round++)
            {
                using (shared.SubscribeConsume(_consumer))   // first subscriber connects; dispose disconnects
                {
                    source.OnNext(round);
                }
            }
        }
    }
}
