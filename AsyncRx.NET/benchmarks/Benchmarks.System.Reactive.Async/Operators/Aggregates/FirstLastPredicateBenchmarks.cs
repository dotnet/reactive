// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
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
        private Func<int, bool> _isLast = default!;
        private Func<int, ValueTask<bool>> _isLastAsync = default!;

        // The predicates depend on N, so they are built once here: the measured region then allocates no delegate for
        // them, as in OrDefaultPredicateAsyncBenchmarks.
        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _isLast = v => v == n;
            _isLastAsync = v => new ValueTask<bool>(v == n);
        }

        [Benchmark]
        public ValueTask First_Predicate()
        {
            return BenchmarkSources.Range(N).First(_isLast).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask First_Predicate_Async()
        {
            return BenchmarkSources.Range(N).First(_isLastAsync).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask FirstOrDefault_Predicate()
        {
            return BenchmarkSources.Range(N).FirstOrDefault(_isLast).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Last_Predicate()
        {
            return BenchmarkSources.Range(N).Last(_isLast).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Last_Predicate_Async()
        {
            return BenchmarkSources.Range(N).Last(_isLastAsync).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask LastOrDefault_Predicate()
        {
            return BenchmarkSources.Range(N).LastOrDefault(_isLast).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Single_Predicate()
        {
            return BenchmarkSources.Range(N).Single(_isLast).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Single_Predicate_Async()
        {
            return BenchmarkSources.Range(N).Single(_isLastAsync).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask SingleOrDefault_Predicate()
        {
            return BenchmarkSources.Range(N).SingleOrDefault(_isLast).SubscribeConsumeAsync(Consumer);
        }
    }
}
