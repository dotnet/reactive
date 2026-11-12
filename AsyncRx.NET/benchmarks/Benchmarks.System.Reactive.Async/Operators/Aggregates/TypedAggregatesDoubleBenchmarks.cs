// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>S1 (variant sweep): the <c>double</c> Sum, Min, Max and Average overloads over a pre-typed source.</summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesDoubleBenchmarks : OperatorVariantBenchmarkBase
    {
        private ArrayAsyncSource<double> _source = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new double[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i * 0.5;
            }

            _source = new ArrayAsyncSource<double>(items);
        }

        [Benchmark]
        public ValueTask Sum() => _source.Sum().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Min() => _source.Min().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max() => _source.Max().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Average() => _source.Average().SubscribeConsumeAsync(Consumer);
    }
}
