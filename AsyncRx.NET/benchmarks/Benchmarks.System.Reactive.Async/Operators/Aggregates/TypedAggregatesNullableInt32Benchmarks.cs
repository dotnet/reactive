// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1 (variant sweep): the <c>int?</c> Sum, Min, Max and Average overloads over a pre-typed source in which
    /// every eighth element is null and the values are small (<c>i &amp; 63</c>), so the checked <c>int?</c> sum
    /// cannot overflow.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class TypedAggregatesNullableInt32Benchmarks : OperatorVariantBenchmarkBase
    {
        private ArrayAsyncSource<int?> _source = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int?[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = (i & 7) == 0 ? null : i & 63;
            }

            _source = new ArrayAsyncSource<int?>(items);
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
