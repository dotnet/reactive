// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>Count</c> and the quantifiers. Predicates are chosen so each must scan the whole stream
    /// (worst case): <c>Any</c>/<c>Contains</c> never match, <c>All</c> always holds.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class CountQuantifierBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Count() => Observable.Range(1, N).Count().SubscribeConsume(Consumer);

        [Benchmark]
        public void Any() => Observable.Range(1, N).Any(static v => v == int.MaxValue).SubscribeConsume(Consumer);

        [Benchmark]
        public void All() => Observable.Range(1, N).All(static v => v > 0).SubscribeConsume(Consumer);

        [Benchmark]
        public void Contains() => Observable.Range(1, N).Contains(int.MaxValue).SubscribeConsume(Consumer);
    }
}
