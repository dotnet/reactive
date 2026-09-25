// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Base class for temporal (S3) operator benchmarks driven by virtual time. <see cref="N"/> is capped
    /// well below the throughput sweep because every element becomes a scheduler-queue entry (cost ~N·logN).
    /// This type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// Operators whose hot path depends on the window-vs-arrival relationship (Throttle, Sample, Buffer(time),
    /// Window(time)) derive from <see cref="RateWindowBenchmarkBase"/> instead, which adds the density knob.
    /// </remarks>
    public abstract class TemporalBenchmarkBase : BenchmarkBase
    {
        /// <summary>Fixed source inter-arrival period, in virtual ticks.</summary>
        protected const long PeriodTicks = 10;

        [Params(100, 1_000, 10_000, 100_000)]
        public int N;
    }
}
