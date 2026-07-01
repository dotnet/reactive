// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>
    /// S2 multicast with replay: <c>Replay</c> buffers past elements for late subscribers. A prime allocation
    /// candidate (the replay buffer). <c>BufferSize == 0</c> replays the entire stream; otherwise a bounded window.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class ReplayBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        [Params(1, 5)]
        public int Subscribers;

        [Params(0, 64)]
        public int BufferSize;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Replay()
        {
            var source = Observable.Range(1, N);
            var connectable = BufferSize == 0 ? source.Replay() : source.Replay(BufferSize);

            var subscriptions = new IDisposable[Subscribers];
            for (var i = 0; i < Subscribers; i++)
            {
                subscriptions[i] = connectable.SubscribeConsume(_consumer);
            }

            using (connectable.Connect())
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Dispose();
                }
            }
        }
    }
}
