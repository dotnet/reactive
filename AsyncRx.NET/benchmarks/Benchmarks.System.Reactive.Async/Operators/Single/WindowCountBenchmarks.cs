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
    /// S1 (gated sweep): count-based <c>Window</c>. <c>_Windows</c> consumes only the window observables;
    /// <c>_Flattened</c> subscribes every window through <c>Merge</c>, which takes an <c>AsyncGate</c> per element.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: <c>Window(count)</c> and the gapped <c>Window(count, skip)</c> with <c>skip &gt; count</c> throw
    /// <c>InvalidOperationException("Queue empty")</c> once <c>count</c> elements have arrived: the operator opens
    /// its first window only after <c>skip</c> elements but tries to close one after <c>count</c>. Only the
    /// overlapping form (<c>skip &lt; count</c>) runs, so only it is measured. Restore the commented-out rows when
    /// the library is fixed.
    /// </remarks>
    [BenchmarkCategory("Single")]
    public class WindowCountBenchmarks : GatedOperatorBenchmarkBase
    {
        // KNOWN GAP (see remarks):
        // [Benchmark]
        // public ValueTask Window_Windows() => BenchmarkSources.Range(N).Window(16).SubscribeConsumeAsync(Consumer);
        //
        // [Benchmark]
        // public ValueTask Window_Gapped_Windows() => BenchmarkSources.Range(N).Window(8, 16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Window_Overlapping_Windows() =>
            BenchmarkSources.Range(N).Window(16, 8).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Window_Overlapping_Flattened() =>
            BenchmarkSources.Range(N).Window(16, 8).Merge().SubscribeConsumeAsync(Consumer);
    }
}
