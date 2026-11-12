// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>S1: collecting operators: <c>ToList</c>, <c>ToArray</c> and <c>ToHashSet</c>.</summary>
    [BenchmarkCategory("Aggregates")]
    public class ToListToArrayBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask ToList() =>
            BenchmarkSources.Range(N).ToList().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToArray() =>
            BenchmarkSources.Range(N).ToArray().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToHashSet() =>
            BenchmarkSources.Range(N).ToHashSet().SubscribeConsumeAsync(Consumer);
    }
}
