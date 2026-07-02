// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// <c>Materialize</c> wraps each notification in a reference-type <c>Notification&lt;T&gt;</c> — a per-element heap
    /// allocation and a prime zero-allocation candidate. <c>Dematerialize</c> unwraps it. <c>Materialize_Error</c>
    /// materializes a faulting source, so the <c>OnError</c>→<c>Notification.OnError</c> path is exercised too.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class MaterializeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Materialize() => Observable.Range(1, N).Materialize().SubscribeConsume(Consumer);

        [Benchmark]
        public void MaterializeDematerialize() =>
            Observable.Range(1, N).Materialize().Dematerialize().SubscribeConsume(Consumer);

        [Benchmark]
        public void Materialize_Error() => FaultingSource.Faulting(N).Materialize().SubscribeConsume(Consumer);
    }
}
