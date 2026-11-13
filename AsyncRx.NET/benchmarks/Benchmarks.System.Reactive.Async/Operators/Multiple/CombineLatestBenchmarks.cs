// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (gated sweep): binary <c>CombineLatest</c> over two cold sources of N (tuple result, synchronous
    /// selector and <c>ValueTask</c> selector). It takes an <c>AsyncGate</c> per element from either side.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask CombineLatest_Tuple() =>
            BenchmarkSources.Range(N).CombineLatest(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask CombineLatest_Selector() =>
            BenchmarkSources.Range(N).CombineLatest(BenchmarkSources.Range(N), static (a, b) => a + b).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask CombineLatest_Selector_Async() =>
            BenchmarkSources.Range(N).CombineLatest(BenchmarkSources.Range(N), static (a, b) => new ValueTask<int>(a + b)).SubscribeConsumeAsync(Consumer);
    }
}
