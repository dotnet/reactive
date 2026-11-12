// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Queryable
{
    /// <summary>
    /// The <c>IQbservable</c> (expression-tree) surface with the local provider. <c>AsQbservable_WhereSelect</c>
    /// builds <c>source.AsQbservable().Where(...).Select(...)</c> and subscribes it. Each invocation builds a new query,
    /// and a query's first <c>Subscribe</c> rewrites the expression tree to the matching <c>Observable</c> methods
    /// (reflection) and compiles it with <c>Expression.Compile</c>. That fixed cost dominates and dwarfs the element
    /// work, which is why N is only 1 and 1,000. <c>AsQbservable_AsObservable</c> is the round trip
    /// <c>AsQbservable().AsObservable()</c>: <c>AsObservable</c> returns the query itself, and because that query
    /// wraps a known source its <c>Subscribe</c> skips rewriting and compiling and forwards straight to the source,
    /// so this measures the wrapper's floor cost.
    /// </summary>
    [BenchmarkCategory("Queryable")]
    public class QbservableBenchmarks : BenchmarkBase
    {
        [Params(1, 1_000)]
        public int N;

        [Benchmark]
        public void AsQbservable_WhereSelect() =>
            Observable.Range(1, N)
                .AsQbservable()
                .Where(v => (v & 1) == 0)
                .Select(v => v + 1)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void AsQbservable_AsObservable() =>
            Observable.Range(1, N).AsQbservable().AsObservable().SubscribeConsume(Consumer);
    }
}
