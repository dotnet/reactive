// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// Single-sequence-category exemplar (S1): <c>Scan</c> — an O(1)-state running accumulate that emits per
    /// element. A <c>long</c> accumulator avoids overflow at large N.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class ScanBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Scan() => Observable.Range(1, N).Scan(0L, static (acc, v) => acc + v).SubscribeConsume(Consumer);
    }
}
