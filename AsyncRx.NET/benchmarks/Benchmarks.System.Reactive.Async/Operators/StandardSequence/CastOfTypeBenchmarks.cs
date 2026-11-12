// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Cast</c> and <c>OfType</c> over a pre-boxed <c>object</c> source built in <c>[GlobalSetup]</c>, so
    /// the boxing is outside the measured region and only the type test and unbox are measured. AsyncRx takes
    /// both type arguments explicitly (<c>Cast&lt;object, int&gt;()</c>).
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class CastOfTypeBenchmarks : OperatorBenchmarkBase
    {
        private ArrayAsyncSource<object> _boxed = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new object[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i;
            }

            _boxed = new ArrayAsyncSource<object>(items);
        }

        [Benchmark]
        public ValueTask Cast() => _boxed.Cast<object, int>().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask OfType() => _boxed.OfType<object, int>().SubscribeConsumeAsync(Consumer);
    }
}
