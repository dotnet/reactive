// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>S1: <c>Min</c>, <c>Max</c> and <c>Average</c> over <c>int</c>, plus the selector forms.</summary>
    [BenchmarkCategory("Aggregates")]
    public class MinMaxAverageBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Min() =>
            BenchmarkSources.Range(N).Min().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max() =>
            BenchmarkSources.Range(N).Max().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Average() =>
            BenchmarkSources.Range(N).Average().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Max_Selector() =>
            BenchmarkSources.Range(N).Max(static v => -v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Average_Selector_Async() =>
            BenchmarkSources.Range(N).Average(static v => new ValueTask<int>(v)).SubscribeConsumeAsync(Consumer);
    }
}
