// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: the numeric aggregates. Streaming <c>OnNext</c> accumulators, so — as noted in the plan — they are
    /// <b>not</b> SIMD candidates (vectorization would need a materialized array); a useful reference point.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinMaxAverageBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Min() => Observable.Range(1, N).Min().SubscribeConsume(Consumer);

        [Benchmark]
        public void Max() => Observable.Range(1, N).Max().SubscribeConsume(Consumer);

        [Benchmark]
        public void Average() => Observable.Range(1, N).Average().SubscribeConsume(Consumer);
    }
}
