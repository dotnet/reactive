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
    /// S1 (gated sweep): <c>WithLatestFrom</c> where the second source holds one value and the first pushes N
    /// (tuple result, synchronous selector and <c>ValueTask</c> selector). It takes an <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class WithLatestFromBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask WithLatestFrom_Tuple() =>
            BenchmarkSources.Range(N).WithLatestFrom(BenchmarkSources.Return(1)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask WithLatestFrom_Selector() =>
            BenchmarkSources.Range(N).WithLatestFrom(BenchmarkSources.Return(1), static (a, b) => a + b).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask WithLatestFrom_Selector_Async() =>
            BenchmarkSources.Range(N).WithLatestFrom(BenchmarkSources.Return(1), static (a, b) => new ValueTask<int>(a + b)).SubscribeConsumeAsync(Consumer);
    }
}
