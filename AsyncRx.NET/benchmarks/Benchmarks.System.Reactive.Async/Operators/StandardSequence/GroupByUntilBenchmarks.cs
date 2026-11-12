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
    /// S1 (gated sweep): <c>GroupByUntil</c> partitions into 8 keyed groups whose duration never fires, so it
    /// measures the per-element routing and duration bookkeeping. <c>_Flattened</c> subscribes every group.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByUntilBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask GroupByUntil_Groups() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static _ => AsyncObservable.Never<int>())
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Flattened() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static _ => AsyncObservable.Never<int>())
                .SelectMany(static g => g)
                .SubscribeConsumeAsync(Consumer);
    }
}
