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
    /// S1 (throughput) exemplar: pushes <c>N</c> elements through <c>Select</c> on the immediate scheduler and
    /// consumes them. Sweeps <c>N</c> from 1 to 1,000,000, so subscription cost (small N) and per-element cost
    /// (large N) are both visible. <c>_Async</c> uses the <c>ValueTask</c>-selector overload with a
    /// synchronously completed result.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Select() =>
            BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Select_Async() =>
            BenchmarkSources.Range(N).Select(static v => new ValueTask<int>(v + 1)).SubscribeConsumeAsync(Consumer);
    }
}
