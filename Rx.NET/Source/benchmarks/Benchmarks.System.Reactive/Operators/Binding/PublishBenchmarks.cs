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
    /// Multicast-category exemplar (S2): <c>Publish</c> fans a single cold source out to M subscribers via
    /// <c>Connect()</c>. Sweeps subscriber count to expose the multicast dispatch cost.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class PublishBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        [Params(1, 2, 5)]
        public int Subscribers;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Publish()
        {
            var connectable = Observable.Range(1, N).Publish();

            var subscriptions = new IDisposable[Subscribers];
            for (var i = 0; i < Subscribers; i++)
            {
                subscriptions[i] = connectable.SubscribeConsume(_consumer);
            }

            using (connectable.Connect())   // drives the source synchronously to all subscribers
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Dispose();
                }
            }
        }
    }
}
