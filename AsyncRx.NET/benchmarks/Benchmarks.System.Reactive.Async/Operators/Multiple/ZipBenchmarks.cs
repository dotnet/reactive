// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (gated sweep): binary <c>Zip</c> (tuple result, synchronous selector and <c>ValueTask</c> selector) over
    /// two cold sources of N. The first source pushes all N before the second is subscribed, so the pairing queue
    /// grows to N; the second source then drains it and the sequence completes. Zip takes an <c>AsyncGate</c> per
    /// element.
    /// </summary>
    /// <remarks>
    /// Zip completes when the first source has completed and its queue is empty, which happens on the second
    /// source's last element. The second source's own completion then reaches a terminated observer and is
    /// dropped.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class ZipBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Zip_Tuple() =>
            BenchmarkSources.Range(N).Zip(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Zip_Selector() =>
            BenchmarkSources.Range(N).Zip(BenchmarkSources.Range(N), static (a, b) => a + b).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Zip_Selector_Async() =>
            BenchmarkSources.Range(N).Zip(BenchmarkSources.Range(N), static (a, b) => new ValueTask<int>(a + b)).SubscribeConsumeAsync(Consumer);
    }
}
