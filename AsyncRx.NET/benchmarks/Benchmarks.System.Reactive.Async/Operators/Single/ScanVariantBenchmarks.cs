// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>S1 (variant sweep): unseeded <c>Scan</c>, synchronous and <c>ValueTask</c>.</summary>
    [BenchmarkCategory("Single")]
    public class ScanVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Scan_NoSeed() =>
            BenchmarkSources.Range(N).Scan(static (acc, v) => acc + v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Scan_NoSeed_Async() =>
            BenchmarkSources.Range(N).Scan(static (acc, v) => new ValueTask<int>(acc + v)).SubscribeConsumeAsync(Consumer);
    }
}
