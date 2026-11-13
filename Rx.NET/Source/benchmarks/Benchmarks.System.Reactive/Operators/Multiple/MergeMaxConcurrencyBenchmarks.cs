// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: the <c>Merge</c> overload with a concurrency cap subscribes to at most 4 inner sequences at a time,
    /// queueing the rest — a distinct code path from the unbounded array/nested Merge. Inner size precomputed in
    /// <c>[GlobalSetup]</c> to avoid a per-invocation closure.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class MergeMaxConcurrencyBenchmarks : OperatorBenchmarkBase
    {
        private int _innerSize;

        [GlobalSetup]
        public void Setup() => _innerSize = 1_000_000 / N;

        [Benchmark]
        public void Merge_MaxConcurrency() =>
            Observable.Merge(Observable.Range(1, N).Select(v => Observable.Range(v, _innerSize)), 4).SubscribeConsume(Consumer);
    }
}
