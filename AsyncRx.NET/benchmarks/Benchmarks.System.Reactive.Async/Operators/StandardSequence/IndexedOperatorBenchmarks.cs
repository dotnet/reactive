// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (variant sweep): the index-taking overloads of Select, Where, SkipWhile and TakeWhile, synchronous and
    /// <c>ValueTask</c>-returning. The indexed SelectMany overloads are in <see cref="SelectManyOverloadBenchmarks"/>.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class IndexedOperatorBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Select_Indexed() =>
            BenchmarkSources.Range(N).Select(static (v, i) => v + i).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Select_Indexed_Async() =>
            BenchmarkSources.Range(N).Select(static (v, i) => new ValueTask<int>(v + i)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Where_Indexed() =>
            BenchmarkSources.Range(N).Where(static (v, i) => (i & 1) == 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Where_Indexed_Async() =>
            BenchmarkSources.Range(N).Where(static (v, i) => new ValueTask<bool>((i & 1) == 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SkipWhile_Indexed() =>
            BenchmarkSources.Range(N).SkipWhile(static (v, i) => i < 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SkipWhile_Indexed_Async() =>
            BenchmarkSources.Range(N).SkipWhile(static (v, i) => new ValueTask<bool>(i < 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeWhile_Indexed() =>
            BenchmarkSources.Range(N).TakeWhile(static (v, i) => i >= 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeWhile_Indexed_Async() =>
            BenchmarkSources.Range(N).TakeWhile(static (v, i) => new ValueTask<bool>(i >= 0)).SubscribeConsumeAsync(Consumer);
    }
}
