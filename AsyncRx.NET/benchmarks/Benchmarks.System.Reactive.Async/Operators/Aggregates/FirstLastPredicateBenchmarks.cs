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
    /// S1 (variant sweep): the predicate overloads of the element operators, synchronous and <c>ValueTask</c>.
    /// The predicates match only the last element (<c>v == N</c>), so every row scans the whole source.
    /// </summary>
    /// <remarks>
    /// The First rows terminate on the last element, just before the cold source completes. The source's
    /// completion then reaches an observer that has already terminated and is dropped, so every row uses the cold
    /// <c>Range</c> shape.
    /// </remarks>
    [BenchmarkCategory("Aggregates")]
    public class FirstLastPredicateBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask First_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).First(v => v == n).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask First_Predicate_Async()
        {
            var n = N;
            return BenchmarkSources.Range(n).First(v => new ValueTask<bool>(v == n)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask FirstOrDefault_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).FirstOrDefault(v => v == n).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Last_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).Last(v => v == n).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Last_Predicate_Async()
        {
            var n = N;
            return BenchmarkSources.Range(n).Last(v => new ValueTask<bool>(v == n)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask LastOrDefault_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).LastOrDefault(v => v == n).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Single_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).Single(v => v == n).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Single_Predicate_Async()
        {
            var n = N;
            return BenchmarkSources.Range(n).Single(v => new ValueTask<bool>(v == n)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask SingleOrDefault_Predicate()
        {
            var n = N;
            return BenchmarkSources.Range(n).SingleOrDefault(v => v == n).SubscribeConsumeAsync(Consumer);
        }
    }
}
