// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: the index-taking overloads of <c>Select</c>, <c>Where</c>, <c>TakeWhile</c> and <c>SkipWhile</c>. Each runs
    /// its own <c>*Indexed</c> sink (it keeps a running index and checks it for overflow on every element), distinct
    /// from the plain-predicate sinks in <c>SelectBenchmarks</c>/<c>WhereBenchmarks</c>/<c>TakeSkipBenchmarks</c>.
    /// The <c>TakeWhile</c>/<c>SkipWhile</c> shapes mirror <c>TakeSkipBenchmarks</c>: a 2N source whose predicate
    /// flips at index N, so both emit N elements.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class IndexedOperatorBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public void Select_Indexed() =>
            Observable.Range(1, N).Select(static (v, i) => v + i).SubscribeConsume(Consumer);

        [Benchmark]
        public void Where_Indexed() =>
            Observable.Range(1, N).Where(static (v, i) => (i & 1) == 0).SubscribeConsume(Consumer);

        [Benchmark]
        public void TakeWhile_Indexed()
        {
            var n = N;
            Observable.Range(1, 2 * n).TakeWhile((v, i) => i < n).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void SkipWhile_Indexed()
        {
            var n = N;
            Observable.Range(1, 2 * n).SkipWhile((v, i) => i < n).SubscribeConsume(Consumer);
        }
    }
}
