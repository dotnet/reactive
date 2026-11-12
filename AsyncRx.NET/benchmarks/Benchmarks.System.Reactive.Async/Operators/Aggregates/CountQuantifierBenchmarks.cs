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
    /// S1: counting and quantifiers. The predicates are chosen so the operator scans the whole source
    /// (<c>Any</c> never matches, <c>All</c> always matches, <c>Contains</c> looks for a missing value).
    /// <c>Any()</c> without a predicate stops at the first element, so it is driven by
    /// <see cref="Pump.RunUntilDoneAsync{TResult}"/>, a producer that stops once the sink has terminated. (A cold
    /// source cannot be cancelled before it returns its subscription, so it would push N - 1 elements that the
    /// terminated observer drops.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class CountQuantifierBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Count() =>
            BenchmarkSources.Range(N).Count().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Count_Predicate() =>
            BenchmarkSources.Range(N).Count(static v => (v & 1) == 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Any() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.Any());

        [Benchmark]
        public ValueTask Any_Predicate() =>
            BenchmarkSources.Range(N).Any(static v => v < 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask All() =>
            BenchmarkSources.Range(N).All(static v => v > 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Contains() =>
            BenchmarkSources.Range(N).Contains(-1).SubscribeConsumeAsync(Consumer);
    }
}
