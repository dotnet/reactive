// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S3 (temporal): <c>Publish().RefCount(disconnectDelay, scheduler)</c> over the pushed source, on the virtual-time
    /// scheduler. The first subscriber connects. Compare with the inherited <c>Source_Baseline</c>.
    /// </summary>
    /// <remarks>
    /// The row covers the whole lifecycle: connect, the per-element forwarding, and the delayed disconnect. When the
    /// sequence completes, AutoDetach disposes the RefCount subscription, which drops the count to zero and schedules
    /// the disconnect one delay later. The driver runs the timeline until no work remains, so that disconnect work item,
    /// its gate and the disposal of the connection are measured too. <c>--verify</c> checks that the timeline ends one
    /// delay after the last push.
    /// </remarks>
    [BenchmarkCategory("Binding")]
    public class RefCountDelayBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_RefCount_DisconnectDelay() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Publish().RefCount(TimeSpan.FromTicks(PeriodTicks), scheduler));
    }
}
