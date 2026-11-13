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
    /// S1 (variant sweep): the <c>ValueTask</c>-predicate overloads of Count, LongCount, Any and All, plus
    /// <c>LongCount(predicate)</c> and <c>Contains</c> with a comparer. Every row scans the whole source.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class CountQuantifierAsyncBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Count_Predicate_Async() =>
            BenchmarkSources.Range(N).Count(static v => new ValueTask<bool>((v & 1) == 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask LongCount_Predicate() =>
            BenchmarkSources.Range(N).LongCount(static v => (v & 1) == 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask LongCount_Predicate_Async() =>
            BenchmarkSources.Range(N).LongCount(static v => new ValueTask<bool>((v & 1) == 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Any_Predicate_Async() =>
            BenchmarkSources.Range(N).Any(static v => new ValueTask<bool>(v < 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask All_Async() =>
            BenchmarkSources.Range(N).All(static v => new ValueTask<bool>(v > 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Contains_Comparer() =>
            BenchmarkSources.Range(N).Contains(-1, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
