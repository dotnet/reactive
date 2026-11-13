// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: the predicate overloads of <c>FirstAsync</c>/<c>FirstOrDefaultAsync</c>/<c>LastAsync</c>/<c>LastOrDefaultAsync</c>.
    /// Each runs its own nested <c>Predicate</c> sink, distinct from the predicate-less <c>Sequence</c> sinks in
    /// <c>FirstLastSingleAsyncBenchmarks</c>. The predicates are chosen so every case scans all N elements:
    /// <c>FirstAsync</c> matches only the last element, the <c>OrDefault</c> forms never match (so they emit the
    /// default at completion), and <c>LastAsync</c> matches every odd element.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class FirstLastPredicateAsyncBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public void FirstAsync_Predicate()
        {
            var n = N;
            Observable.Range(1, n).FirstAsync(v => v == n).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void FirstOrDefaultAsync_Predicate() =>
            Observable.Range(1, N).FirstOrDefaultAsync(static v => v < 0).SubscribeConsume(Consumer);

        [Benchmark]
        public void LastAsync_Predicate() =>
            Observable.Range(1, N).LastAsync(static v => (v & 1) != 0).SubscribeConsume(Consumer);

        [Benchmark]
        public void LastOrDefaultAsync_Predicate() =>
            Observable.Range(1, N).LastOrDefaultAsync(static v => v < 0).SubscribeConsume(Consumer);
    }
}
