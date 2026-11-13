// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: the two <c>Aggregate</c> overloads not covered by the seeded form in <c>AggregateBenchmarks</c>. The
    /// seedless overload runs the <c>Aggregate&lt;TSource&gt;</c> sink (the first element becomes the accumulator,
    /// tracked with a has-value flag); the result-selector overload runs <c>Aggregate&lt;TSource, TAccumulate,
    /// TResult&gt;</c>, which applies the selector once at completion.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class AggregateVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public void Aggregate_Seedless() =>
            Observable.Range(1, N).Aggregate(static (acc, v) => acc + v).SubscribeConsume(Consumer);

        [Benchmark]
        public void Aggregate_ResultSelector() =>
            Observable.Range(1, N).Aggregate(0L, static (acc, v) => acc + v, static acc => acc * 2).SubscribeConsume(Consumer);
    }
}
