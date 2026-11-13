// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1 (variant sweep): the generic <c>Min</c>/<c>Max</c> with an explicit comparer (and without, through the
    /// generic overload). AsyncRx declares these without <c>this</c>, so they are called as static methods.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinMaxComparerBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Min_Generic() =>
            AsyncObservable.Min<int>(BenchmarkSources.Range(N), Comparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max_Generic() =>
            AsyncObservable.Max<int>(BenchmarkSources.Range(N), Comparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
