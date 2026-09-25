// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// Isolates the cost of boxing: the same pipeline over value-type <c>int</c> (<c>Unboxed</c>, baseline) vs one
    /// routed through <c>IObservable&lt;object&gt;</c> (<c>Boxed</c> — a box on the way in and an unbox on the way out
    /// per element). The Ratio + Allocated columns quantify the per-element box/unbox tax directly.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class BoxingBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public void Unboxed() => Observable.Range(1, N).Select(static v => v + 1).SubscribeConsume(Consumer);

        [Benchmark]
        public void Boxed() =>
            Observable.Range(1, N).Select(static v => (object)v).Select(static o => (int)o + 1).SubscribeConsume(Consumer);
    }
}
