// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>Min</c>/<c>Max</c> with an explicit <see cref="IComparer{T}"/>. These run the generic comparer-based
    /// <c>Min&lt;TSource&gt;</c>/<c>Max&lt;TSource&gt;</c> sinks (value-type branch, one interface call per element)
    /// instead of the specialised <c>MinInt32</c>/<c>MaxInt32</c> sinks that the parameterless overloads in
    /// <c>MinMaxAverageBenchmarks</c> hit for an <c>int</c> source, so the pair isolates the cost of the generic path.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinMaxComparerBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly IComparer<int> _comparer = Comparer<int>.Default;

        [Benchmark]
        public void Min_Comparer() => Observable.Range(1, N).Min(_comparer).SubscribeConsume(Consumer);

        [Benchmark]
        public void Max_Comparer() => Observable.Range(1, N).Max(_comparer).SubscribeConsume(Consumer);
    }
}
