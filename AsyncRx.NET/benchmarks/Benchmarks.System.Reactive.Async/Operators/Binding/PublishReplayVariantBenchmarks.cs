// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (variant sweep): the <c>Publish</c>, <c>PublishLast</c> and <c>Replay</c> overloads that
    /// <see cref="PublishBenchmarks"/>, <see cref="PublishLastBenchmarks"/> and <see cref="ReplayBenchmarks"/> do not
    /// reach, over a cold source of N with one subscriber: the selector with an initial value, the
    /// <c>ValueTask</c> selector of <c>PublishLast</c>, <c>Replay(1)</c> (the single-element replay path), and the
    /// buffer-size forms with an explicit immediate scheduler. Connect rows subscribe, connect and drain; selector
    /// rows multicast inside the selector.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class PublishReplayVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_Selector_InitialValue() =>
            BenchmarkSources.Range(N).Publish(static s => s.Select(static v => v + 1), -1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask PublishLast_Selector_Async() =>
            BenchmarkSources.Range(N).PublishLast(static s => new ValueTask<IAsyncObservable<int>>(s)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_BufferSize1_Connect() =>
            BenchmarkSources.Range(N).Replay(1).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_BufferSize_Scheduler_Connect() =>
            BenchmarkSources.Range(N).Replay(16, ImmediateAsyncScheduler.Instance).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_Selector_BufferSize_Scheduler() =>
            BenchmarkSources.Range(N).Replay(static s => s, 16, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);
    }
}
