// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>S1: seeded <c>Aggregate</c> (sum into a long), with the synchronous and the <c>ValueTask</c> accumulator.</summary>
    [BenchmarkCategory("Aggregates")]
    public class AggregateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Aggregate() =>
            BenchmarkSources.Range(N).Aggregate(0L, static (acc, v) => acc + v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Aggregate_Async() =>
            BenchmarkSources.Range(N).Aggregate(0L, static (acc, v) => new ValueTask<long>(acc + v)).SubscribeConsumeAsync(Consumer);
    }
}
