// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1 (variant sweep): the selector forms of <c>Min</c>, <c>Max</c> and <c>Average</c> that
    /// <see cref="MinMaxAverageBenchmarks"/> does not cover. The typed rows project to <c>int</c>, which binds the
    /// generated <c>int</c> overloads. The <c>_Generic_</c> rows bind <c>Min&lt;TSource, TResult&gt;</c> /
    /// <c>Max&lt;TSource, TResult&gt;</c>: they project to <see cref="TimeSpan"/>
    /// (<c>TimeSpan.FromTicks(v)</c>), which has no typed overload and does not allocate. An <c>int</c>-returning
    /// lambda would bind the more specific typed overload instead.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class SelectorAggregateBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Min_Selector() =>
            BenchmarkSources.Range(N).Min(static v => -v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Min_Selector_Async() =>
            BenchmarkSources.Range(N).Min(static v => new ValueTask<int>(-v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max_Selector_Async() =>
            BenchmarkSources.Range(N).Max(static v => new ValueTask<int>(-v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Average_Selector() =>
            BenchmarkSources.Range(N).Average(static v => v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Min_Generic_Selector() =>
            BenchmarkSources.Range(N).Min(static v => TimeSpan.FromTicks(v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max_Generic_Selector_Async() =>
            BenchmarkSources.Range(N).Max(static v => new ValueTask<TimeSpan>(TimeSpan.FromTicks(v))).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Min_Generic_Selector_Comparer() =>
            BenchmarkSources.Range(N).Min(static v => TimeSpan.FromTicks(v), Comparer<TimeSpan>.Default).SubscribeConsumeAsync(Consumer);
    }
}
