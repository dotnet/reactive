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
    /// S1: <c>Sum</c> over <c>int</c> and through a selector widening to <c>long</c> (synchronous and
    /// <c>ValueTask</c>). <c>Sum()</c> on <c>int</c> is checked, so it runs over a pre-built source of small values
    /// (<c>i &amp; 7</c>) that cannot overflow at N = 1,000,000; the selector rows widen <c>Range</c> to <c>long</c>.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class SumBenchmarks : OperatorBenchmarkBase
    {
        private ArrayAsyncSource<int> _small = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i & 7;
            }

            _small = new ArrayAsyncSource<int>(items);
        }

        [Benchmark]
        public ValueTask Sum_Int32() =>
            _small.Sum().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Sum_Selector() =>
            BenchmarkSources.Range(N).Sum(static v => (long)v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Sum_Selector_Async() =>
            BenchmarkSources.Range(N).Sum(static v => new ValueTask<long>(v)).SubscribeConsumeAsync(Consumer);
    }
}
