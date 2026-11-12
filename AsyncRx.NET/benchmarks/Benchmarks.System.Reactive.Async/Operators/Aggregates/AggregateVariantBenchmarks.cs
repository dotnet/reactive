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
    /// S1 (variant sweep): the unseeded and result-selector <c>Aggregate</c> overloads, synchronous and
    /// <c>ValueTask</c>.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class AggregateVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Aggregate_NoSeed() =>
            BenchmarkSources.Range(N).Aggregate(static (acc, v) => acc + v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Aggregate_NoSeed_Async() =>
            BenchmarkSources.Range(N).Aggregate(static (acc, v) => new ValueTask<int>(acc + v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Aggregate_ResultSelector() =>
            BenchmarkSources.Range(N).Aggregate(0L, static (acc, v) => acc + v, static acc => acc * 2).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Aggregate_ResultSelector_Async() =>
            BenchmarkSources.Range(N).Aggregate(0L, static (acc, v) => new ValueTask<long>(acc + v), static acc => new ValueTask<long>(acc * 2)).SubscribeConsumeAsync(Consumer);
    }
}
