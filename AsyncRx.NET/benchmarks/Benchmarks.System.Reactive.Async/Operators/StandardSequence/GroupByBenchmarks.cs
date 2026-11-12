// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (gated sweep): <c>GroupBy</c> into 8 groups. The <c>_Groups</c> rows consume only the group observables
    /// (elements go to unobserved groups); <c>_Flattened</c> subscribes every group through <c>SelectMany</c>,
    /// which takes an <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask GroupBy_Groups() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Element_Groups() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7, static v => v + 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupBy_Flattened() =>
            BenchmarkSources.Range(N).GroupBy(static v => v & 7).SelectMany(static g => g).SubscribeConsumeAsync(Consumer);
    }
}
