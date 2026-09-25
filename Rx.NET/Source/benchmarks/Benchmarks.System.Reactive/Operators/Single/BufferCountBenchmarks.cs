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
    /// multiple in-flight buffers.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class BufferCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Count() => Observable.Range(1, N).Buffer(16).SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Count_Skip() => Observable.Range(1, N).Buffer(16, 8).SubscribeConsume(Consumer);
    }
}
