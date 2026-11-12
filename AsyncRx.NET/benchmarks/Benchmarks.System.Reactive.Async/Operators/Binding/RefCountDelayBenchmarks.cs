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
    /// scheduler. The first subscriber connects; the delayed disconnect is armed on the virtual clock. Compare with
    /// the inherited <c>Source_Baseline</c>.
    /// </summary>
    /// <remarks>
    /// The driver awaits termination and does not dispose the subscription, so the disconnect timer is not reached
    /// within the timeline; the row measures the delayed-disconnect bookkeeping on connect and per element.
    /// </remarks>
    [BenchmarkCategory("Binding")]
    public class RefCountDelayBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_RefCount_DisconnectDelay() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Publish().RefCount(TimeSpan.FromTicks(PeriodTicks), scheduler));
    }
}
