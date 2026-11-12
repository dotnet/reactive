// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Base class for temporal (S3) operator benchmarks driven by virtual time. <see cref="N"/> is capped
    /// well below the throughput sweep because every element becomes a scheduler-queue entry (cost ~N·logN).
    /// This type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every derived class inherits <see cref="Source_Baseline"/>, which runs only the shared virtual-time source
    /// (<see cref="VirtualTimeSource.Timed(IScheduler, int, TimeSpan)"/> on a fresh <see cref="HistoricalScheduler"/>)
    /// into the consumer. It is the class baseline, so the Ratio column reads as "operator pipeline cost relative
    /// to source + scheduler cost" at the same N: a ratio near 1 means the operator adds little over driving the
    /// timeline itself. Derived classes must not declare their own <c>Baseline = true</c> method.
    /// </para>
    /// <para>
    /// Operators whose hot path depends on the window-vs-arrival relationship (Throttle, Sample, Buffer(time),
    /// Window(time)) derive from <see cref="RateWindowBenchmarkBase"/> instead, which adds the density knob. The
    /// baseline ignores density, so it simply repeats (with the same result) for each <c>Density</c> value there.
    /// </para>
    /// </remarks>
    public abstract class TemporalBenchmarkBase : BenchmarkBase
    {
        /// <summary>Fixed source inter-arrival period, in virtual ticks.</summary>
        protected const long PeriodTicks = 10;

        [Params(100, 1_000, 10_000, 100_000)]
        public int N;

        /// <summary>
        /// The source alone: N elements <see cref="PeriodTicks"/> apart on a fresh <see cref="HistoricalScheduler"/>,
        /// drained by <c>Start()</c> with no operator in between.
        /// </summary>
        [Benchmark(Baseline = true)]
        public void Source_Baseline()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            VirtualTimeSource.Timed(scheduler, n, period)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
