// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>S1: count-based <c>Window</c> produces a sub-observable per batch; the inner windows are flattened.</summary>
    [BenchmarkCategory("Single")]
    public class WindowCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Window_Count() =>
            Observable.Range(1, N).Window(16).SelectMany(static w => w).SubscribeConsume(Consumer);
    }
}
