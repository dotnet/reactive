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
    /// S1: <c>Min</c>/<c>Max</c>/<c>Sum</c>/<c>Average</c> over a pre-typed <c>IObservable&lt;decimal&gt;</c>, so the
    /// <c>decimal</c> sinks (<c>MinDecimal</c>/<c>MaxDecimal</c>/<c>SumDecimal</c>/<c>AverageDecimal</c>) are hit
    /// rather than the <c>int</c> sinks in <c>MinMaxAverageBenchmarks</c>/<c>SumBenchmarks</c>. Decimal arithmetic is
    /// implemented in software, so expect a markedly higher per-element cost than the primitive types. The source is
    /// an <see cref="ArraySource{T}"/> over an array built in <c>[GlobalSetup]</c>, so no per-element conversion runs
    /// in the measured region. (The selector overloads are not benchmarked: each is implemented as <c>Select</c>
    /// followed by the same typed sink.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesDecimalBenchmarks : OperatorVariantBenchmarkBase
    {
        private IObservable<decimal> _source;

        [GlobalSetup]
        public void Setup()
        {
            var items = new decimal[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i + 0.25m;
            }

            _source = new ArraySource<decimal>(items);
        }

        [Benchmark]
        public void Min_Decimal() => _source.Min().SubscribeConsume(Consumer);

        [Benchmark]
        public void Max_Decimal() => _source.Max().SubscribeConsume(Consumer);

        [Benchmark]
        public void Sum_Decimal() => _source.Sum().SubscribeConsume(Consumer);

        [Benchmark]
        public void Average_Decimal() => _source.Average().SubscribeConsume(Consumer);
    }
}
