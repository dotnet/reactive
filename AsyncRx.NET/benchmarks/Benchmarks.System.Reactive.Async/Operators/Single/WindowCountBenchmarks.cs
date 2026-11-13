// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1 (gated sweep): count-based <c>Window</c> in its three shapes: tumbling (<c>Window(count)</c>), gapped
    /// (<c>skip &gt; count</c>) and overlapping (<c>skip &lt; count</c>). <c>_Windows</c> consumes only the window
    /// observables; <c>_Flattened</c> subscribes every window through <c>Merge</c>, which takes an
    /// <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class WindowCountBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Window_Windows() =>
            BenchmarkSources.Range(N).Window(16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Window_Gapped_Windows() =>
            BenchmarkSources.Range(N).Window(8, 16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Window_Overlapping_Windows() =>
            BenchmarkSources.Range(N).Window(16, 8).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Window_Overlapping_Flattened() =>
            BenchmarkSources.Range(N).Window(16, 8).Merge().SubscribeConsumeAsync(Consumer);
    }
}
