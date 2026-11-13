// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>S1: seeded <c>Scan</c> (running sum), with the synchronous and the <c>ValueTask</c> accumulator.</summary>
    [BenchmarkCategory("Single")]
    public class ScanBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Scan() =>
            BenchmarkSources.Range(N).Scan(0L, static (acc, v) => acc + v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Scan_Async() =>
            BenchmarkSources.Range(N).Scan(0L, static (acc, v) => new ValueTask<long>(acc + v)).SubscribeConsumeAsync(Consumer);
    }
}
