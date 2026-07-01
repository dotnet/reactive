// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// S1: the count-based last-N family. Each keeps a fixed-capacity sliding <c>Queue</c> — a clean zero-allocation
    /// candidate (the capacity is known up front, so a ring buffer / pooled array would eliminate the churn).
    /// </summary>
    [BenchmarkCategory("Single")]
    public class TakeLastSkipLastBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void TakeLast_Count() => Observable.Range(1, N).TakeLast(16).SubscribeConsume(Consumer);

        [Benchmark]
        public void SkipLast_Count() => Observable.Range(1, N).SkipLast(16).SubscribeConsume(Consumer);

        [Benchmark]
        public void TakeLastBuffer_Count() => Observable.Range(1, N).TakeLastBuffer(16).SubscribeConsume(Consumer);
    }
}
