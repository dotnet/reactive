// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Concurrency
{
    /// <summary>Concurrency (S1): <c>Synchronize</c> serializes notifications behind a gate — a per-notification lock cost.</summary>
    [BenchmarkCategory("Concurrency")]
    public class SynchronizeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Synchronize() => Observable.Range(1, N).Synchronize().SubscribeConsume(Consumer);
    }
}
