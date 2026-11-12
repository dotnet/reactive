// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Overhead
{
    /// <summary>
    /// Anchors AsyncRx cost against plain loops at the same N. <c>ForLoop</c> (baseline) consumes N values
    /// directly; <c>AwaitedValueTaskLoop</c> awaits a synchronously completed <see cref="ValueTask"/> per value,
    /// the floor for any awaited-per-element design; the AsyncRx rows push the same values through
    /// <c>Range</c> + <c>Select</c>.
    /// </summary>
    [BenchmarkCategory("Overhead")]
    public class OverheadBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public void ForLoop()
        {
            var n = N;
            for (var i = 1; i <= n; i++)
            {
                Consumer.Consume(i + 1);
            }
        }

        [Benchmark]
        public async ValueTask AwaitedValueTaskLoop()
        {
            var n = N;
            for (var i = 1; i <= n; i++)
            {
                Consumer.Consume(await NextAsync(i).ConfigureAwait(false));
            }
        }

        [Benchmark]
        public ValueTask AsyncRx_Select() =>
            BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask AsyncRx_Select_Async() =>
            BenchmarkSources.Range(N).Select(static v => new ValueTask<int>(v + 1)).SubscribeConsumeAsync(Consumer);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ValueTask<int> NextAsync(int value) => new(value + 1);
    }
}
