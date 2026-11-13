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
    /// S1: <c>Min</c>/<c>Max</c>/<c>Sum</c>/<c>Average</c> over a pre-typed <c>IObservable&lt;double&gt;</c>, so the
    /// <c>double</c> sinks (<c>MinDouble</c>/<c>MaxDouble</c>/<c>SumDouble</c>/<c>AverageDouble</c>) are hit rather
    /// than the <c>int</c> sinks in <c>MinMaxAverageBenchmarks</c>/<c>SumBenchmarks</c>; <c>MinDouble</c> also
    /// carries a NaN check the integer sinks lack. The source is an <see cref="ArraySource{T}"/> over
    /// an array built in <c>[GlobalSetup]</c>, so no per-element conversion runs in the measured region. (The selector
    /// overloads are not benchmarked: each is implemented as <c>Select</c> followed by the same typed sink.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesDoubleBenchmarks : OperatorVariantBenchmarkBase
    {
        private IObservable<double> _source;

        [GlobalSetup]
        public void Setup()
        {
            var items = new double[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i + 0.5;
            }

            _source = new ArraySource<double>(items);
        }

        [Benchmark]
        public void Min_Double() => _source.Min().SubscribeConsume(Consumer);

        [Benchmark]
        public void Max_Double() => _source.Max().SubscribeConsume(Consumer);

        [Benchmark]
        public void Sum_Double() => _source.Sum().SubscribeConsume(Consumer);

        [Benchmark]
        public void Average_Double() => _source.Average().SubscribeConsume(Consumer);
    }
}
