// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1: count-based <c>Buffer</c>: non-overlapping (<c>count</c>), overlapping (<c>skip &lt; count</c>) and
    /// gapped (<c>skip &gt; count</c>). Each emitted buffer is a list allocation, which the Allocated column shows.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class BufferCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Buffer() =>
            BenchmarkSources.Range(N).Buffer(16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Buffer_Overlapping() =>
            BenchmarkSources.Range(N).Buffer(16, 8).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Buffer_Gapped() =>
            BenchmarkSources.Range(N).Buffer(8, 16).SubscribeConsumeAsync(Consumer);
    }
}
