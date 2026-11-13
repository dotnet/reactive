// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>Min</c>/<c>Max</c>/<c>Sum</c>/<c>Average</c> over a pre-typed <c>IObservable&lt;int?&gt;</c>, so the
    /// nullable sinks (<c>MinInt32Nullable</c>/<c>MaxInt32Nullable</c>/<c>SumInt32Nullable</c>/<c>AverageInt32Nullable</c>)
    /// are hit rather than the <c>int</c> sinks in <c>MinMaxAverageBenchmarks</c>/<c>SumBenchmarks</c>. Every eighth
    /// value is null, exercising the skip-null branch; the non-null values stay below 256 so <c>Sum</c>'s checked
    /// <c>int</c> accumulation cannot overflow at N=1,000,000. The source is an <see cref="ArraySource{T}"/> over an
    /// array built in <c>[GlobalSetup]</c>, so no per-element conversion runs in the measured region. (The selector
    /// overloads are not benchmarked: each is implemented as <c>Select</c> followed by the same typed sink.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesNullableInt32Benchmarks : OperatorVariantBenchmarkBase
    {
        private IObservable<int?> _source;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int?[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = (i & 7) == 7 ? null : i & 0xFF;
            }

            _source = new ArraySource<int?>(items);
        }

        [Benchmark]
        public void Min_NullableInt32() => _source.Min().SubscribeConsume(Consumer);

        [Benchmark]
        public void Max_NullableInt32() => _source.Max().SubscribeConsume(Consumer);

        [Benchmark]
        public void Sum_NullableInt32() => _source.Sum().SubscribeConsume(Consumer);

        [Benchmark]
        public void Average_NullableInt32() => _source.Average().SubscribeConsume(Consumer);
    }
}
