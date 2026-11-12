// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (gated sweep): <c>Publish</c> over a cold source of N with one subscriber. <c>Publish</c> and
    /// <c>Publish(initialValue)</c> subscribe, connect and drain; the selector forms multicast inside the
    /// selector with no explicit connect. The connectable observable takes an <c>AsyncGate</c>.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class PublishBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_Connect() =>
            BenchmarkSources.Range(N).Publish().SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Publish_InitialValue_Connect() =>
            BenchmarkSources.Range(N).Publish(-1).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Publish_Selector() =>
            BenchmarkSources.Range(N).Publish(static s => s.Select(static v => v + 1)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Publish_Selector_Async() =>
            BenchmarkSources.Range(N).Publish(static s => new ValueTask<IAsyncObservable<int>>(s.Select(static v => v + 1))).SubscribeConsumeAsync(Consumer);
    }
}
