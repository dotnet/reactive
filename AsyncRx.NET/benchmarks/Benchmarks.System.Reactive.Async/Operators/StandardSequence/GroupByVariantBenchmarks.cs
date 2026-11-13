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
    /// S1 (gated sweep): the other <c>GroupBy</c> overloads, consuming only the group observables: the
    /// <c>ValueTask</c> key selector, the capacity hint and an explicit comparer.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByVariantBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask GroupBy_Async() =>
            BenchmarkSources.Range(N).GroupBy(static v => new ValueTask<int>(v & 7)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Element_Async() =>
            BenchmarkSources.Range(N).GroupBy(static v => new ValueTask<int>(v & 7), static v => new ValueTask<int>(v + 1)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Capacity() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, 8).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Comparer() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
