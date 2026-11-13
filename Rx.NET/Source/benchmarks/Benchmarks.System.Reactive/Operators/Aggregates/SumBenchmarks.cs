// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// Aggregates-category exemplar (S1): <c>Sum</c> — a scalar streaming accumulate that emits on completion.
    /// Projected to <c>long</c> to avoid <c>int</c> overflow at large N. (A reference point for the SIMD
    /// discussion: streaming accumulators cannot be vectorized.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class SumBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Sum() => Observable.Range(1, N).Sum(x => (long)x).SubscribeConsume(Consumer);
    }
}
