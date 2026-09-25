// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1 (throughput) exemplar: pushes <c>N</c> elements synchronously through <c>Select</c> and consumes
    /// them. Sweeps <c>N</c> from 1 to 1,000,000 so subscription cost (small N) and per-element cost
    /// (large N) are both visible, with allocations reported by the shared MemoryDiagnoser.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Select() =>
            Observable.Range(1, N).Select(v => v + 1).SubscribeConsume(Consumer);
    }
}
