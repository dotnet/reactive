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
    /// S1 (variant sweep): the <c>ValueTask</c> predicate overloads of <c>FirstOrDefault</c>,
    /// <c>LastOrDefault</c> and <c>SingleOrDefault</c>. As in <see cref="FirstLastPredicateBenchmarks"/>, the
    /// predicate matches only the last element (<c>v == N</c>), so every row scans the whole cold source. The
    /// predicate captures <c>N</c>, so it is built once in <c>[GlobalSetup]</c>.
    /// </summary>
    /// <remarks>
    /// <c>FirstOrDefault</c> terminates on the last element, just before its cold source completes. The source's
    /// completion then reaches an observer that has already terminated and is dropped.
    /// </remarks>
    [BenchmarkCategory("Aggregates")]
    public class OrDefaultPredicateAsyncBenchmarks : OperatorVariantBenchmarkBase
    {
        private Func<int, ValueTask<bool>> _isLast = default!;

        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _isLast = v => new ValueTask<bool>(v == n);
        }

        [Benchmark]
        public ValueTask FirstOrDefault_Predicate_Async() =>
            BenchmarkSources.Range(N).FirstOrDefault(_isLast).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask LastOrDefault_Predicate_Async() =>
            BenchmarkSources.Range(N).LastOrDefault(_isLast).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SingleOrDefault_Predicate_Async() =>
            BenchmarkSources.Range(N).SingleOrDefault(_isLast).SubscribeConsumeAsync(Consumer);
    }
}
