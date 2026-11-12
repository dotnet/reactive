// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// S1: count-based <c>Buffer</c> — a <c>List</c> per batch (an allocation candidate). The skip variant keeps
    /// multiple in-flight buffers. The size-1/size-2 variants are the worst cases for per-batch overhead: one
    /// buffer per element (<c>Buffer(1)</c>), a skip larger than the count so half the elements are dropped
    /// (<c>Buffer(1, 2)</c>), and overlapping buffers with every element in two of them (<c>Buffer(2, 1)</c>).
    /// </summary>
    [BenchmarkCategory("Single")]
    public class BufferCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Count() => Observable.Range(1, N).Buffer(16).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Count_Skip() => Observable.Range(1, N).Buffer(16, 8).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Count1() => Observable.Range(1, N).Buffer(1).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Count1_Skip2() => Observable.Range(1, N).Buffer(1, 2).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Count2_Skip1() => Observable.Range(1, N).Buffer(2, 1).SubscribeConsume(Consumer);
    }
}
