// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Conversions
{
    /// <summary>
    /// S1 (variant sweep): <c>IEnumerable&lt;T&gt;.ToAsyncObservable(scheduler)</c> on the immediate scheduler,
    /// over an <c>int[N]</c> built once and held in a field typed <see cref="IEnumerable{T}"/>. The enumeration
    /// goes through the interface, as it would for any enumerable source.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class EnumerableToAsyncObservableBenchmarks : OperatorVariantBenchmarkBase
    {
        private IEnumerable<int> _items = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i;
            }

            _items = items;
        }

        [Benchmark]
        public ValueTask ToAsyncObservable_Enumerable() =>
            _items.ToAsyncObservable(ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);
    }
}
