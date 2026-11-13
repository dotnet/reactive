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
    /// S1: <c>Min</c>/<c>Max</c>/<c>Sum</c>/<c>Average</c> over a pre-typed <c>IObservable&lt;long&gt;</c>, so the
    /// <c>long</c> sinks (<c>MinInt64</c>/<c>MaxInt64</c>/<c>SumInt64</c>/<c>AverageInt64</c>) are hit rather than the
    /// <c>int</c> sinks in <c>MinMaxAverageBenchmarks</c>/<c>SumBenchmarks</c>. The source is an
    /// <see cref="ArraySource{T}"/> over an array built in <c>[GlobalSetup]</c>, so no per-element conversion runs in
    /// the measured region. The values are 1..N, so <c>Sum</c>'s checked accumulation never overflows. (The selector
    /// overloads are not benchmarked: each is implemented as <c>Select</c> followed by the same typed sink.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesInt64Benchmarks : OperatorVariantBenchmarkBase
    {
        private IObservable<long> _source;

        [GlobalSetup]
        public void Setup()
        {
            var items = new long[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i + 1;
            }

            _source = new ArraySource<long>(items);
        }

        [Benchmark]
        public void Min_Int64() => _source.Min().SubscribeConsume(Consumer);

        [Benchmark]
        public void Max_Int64() => _source.Max().SubscribeConsume(Consumer);

        [Benchmark]
        public void Sum_Int64() => _source.Sum().SubscribeConsume(Consumer);

        [Benchmark]
        public void Average_Int64() => _source.Average().SubscribeConsume(Consumer);
    }
}
