// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (variant sweep): the <c>GroupBy</c> overloads that combine an element selector, a capacity hint and a
    /// key comparer, which <see cref="GroupByBenchmarks"/> and <see cref="GroupByVariantBenchmarks"/> do not
    /// cover. As in <see cref="GroupByVariantBenchmarks"/>, the source is partitioned into 8 groups
    /// (<c>v &amp; 7</c>, capacity 8, <see cref="EqualityComparer{T}.Default"/>) and only the group observables
    /// are consumed, so the rows measure the routing cost.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByOverloadBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask GroupBy_Element_Comparer() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, static v => v + 1, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Element_Capacity() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, static v => v + 1, 8).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Capacity_Comparer() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, 8, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Element_Capacity_Comparer() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, static v => v + 1, 8, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Capacity_Comparer_Async() =>
            BenchmarkSources.Range(N).GroupBy(static v => new ValueTask<int>(v & 7), 8, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Element_Capacity_Comparer_Async() =>
            BenchmarkSources.Range(N).GroupBy(static v => new ValueTask<int>(v & 7), static v => new ValueTask<int>(v + 1), 8, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
