// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// S1: seedless <c>Scan(accumulator)</c>, which runs the <c>Scan&lt;TSource&gt;</c> sink (the first element seeds
    /// the accumulator via a has-value flag), distinct from the seeded <c>Scan&lt;TSource, TAccumulate&gt;</c> in
    /// <c>ScanBenchmarks</c>. Emits one running total per element.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class ScanVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public void Scan_Seedless() =>
            Observable.Range(1, N).Scan(static (acc, v) => acc + v).SubscribeConsume(Consumer);
    }
}
