// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Distinct</c> over an all-unique stream, so its internal <c>HashSet</c> grows to N — a bounded
    /// allocation candidate for the modernization spike.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class DistinctBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Distinct() => Observable.Range(1, N).Distinct().SubscribeConsume(Consumer);

        [Benchmark]
        public void Distinct_KeySelector() => Observable.Range(1, N).Distinct(static v => v % 1000).SubscribeConsume(Consumer);
    }
}
