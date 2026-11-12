// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1 (gated sweep): <c>SequenceEqual</c> over two equal sources of N, with the default and an explicit
    /// comparer. It takes an <c>AsyncGate</c> per element from either side.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class SequenceEqualBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask SequenceEqual() =>
            BenchmarkSources.Range(N).SequenceEqual(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SequenceEqual_Comparer() =>
            BenchmarkSources.Range(N).SequenceEqual(BenchmarkSources.Range(N), EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
