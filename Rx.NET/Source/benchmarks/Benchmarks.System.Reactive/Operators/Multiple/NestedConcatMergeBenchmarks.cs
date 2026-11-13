// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1 fan-out: <c>Concat</c> and <c>Merge</c> over a stream of inner sequences
    /// (<c>IObservable&lt;IObservable&lt;T&gt;&gt;</c>) — distinct sinks from the two-source <c>Concat</c> in
    /// <see cref="ConcatBenchmarks"/> and the array <c>Merge</c> in <see cref="MergeBenchmarks"/>. Cross-mapped so
    /// total emitted work stays ~constant across N (inner size <c>1,000,000 / N</c>, precomputed in
    /// <c>[GlobalSetup]</c> to avoid a per-invocation closure), so N reads as the inner-subscription count.
    /// (Moved here from the legacy <c>ComparisonBenchmarks</c> grid, where they were <c>ConcatCrossMap</c> and
    /// <c>MergeCrossMap</c>.)
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class NestedConcatMergeBenchmarks : OperatorBenchmarkBase
    {
        private int _innerSize;

        [GlobalSetup]
        public void Setup() => _innerSize = 1_000_000 / N;

        [Benchmark]
        public void Concat_Nested() =>
            Observable.Concat(Observable.Range(1, N).Select(v => Observable.Range(v, _innerSize))).SubscribeConsume(Consumer);

        [Benchmark]
        public void Merge_Nested() =>
            Observable.Merge(Observable.Range(1, N).Select(v => Observable.Range(v, _innerSize))).SubscribeConsume(Consumer);
    }
}
