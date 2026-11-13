// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Relationship between an operator's time-window and the source's inter-arrival period. One knob
    /// captures the hot-path variation for rate/window operators without exploding the benchmark matrix.
    /// </summary>
    public enum Regime
    {
        /// <summary>Window &lt; period — elements are spaced further apart than the window (e.g. Throttle: all pass).</summary>
        Sparse,

        /// <summary>Window ≈ period — the boundary case.</summary>
        Boundary,

        /// <summary>Window ≫ period — elements arrive far faster than the window (e.g. Throttle: constant cancel+reschedule).</summary>
        Dense,
    }

    /// <summary>
    /// Base class for rate/window temporal operators (Throttle, Sample, Buffer(time), Window(time)) whose hot
    /// path depends on how the operator's <see cref="Window"/> relates to the source's <see cref="Period"/>.
    /// Adds the <see cref="Density"/> knob and computes the window once per parameter combination.
    /// </summary>
    public abstract class RateWindowBenchmarkBase : TemporalBenchmarkBase
    {
        [Params(Regime.Sparse, Regime.Boundary, Regime.Dense)]
        public Regime Density;

        /// <summary>The source inter-arrival period.</summary>
        protected TimeSpan Period { get; private set; }

        /// <summary>The operator's time window, derived from <see cref="Density"/> relative to <see cref="Period"/>.</summary>
        protected TimeSpan Window { get; private set; }

        [GlobalSetup]
        public void RateWindowSetup()
        {
            Period = TimeSpan.FromTicks(PeriodTicks);
            Window = Density switch
            {
                Regime.Sparse => TimeSpan.FromTicks(PeriodTicks / 2),   // window < period → every element passes
                Regime.Boundary => TimeSpan.FromTicks(PeriodTicks),     // window ≈ period
                Regime.Dense => TimeSpan.FromTicks(PeriodTicks * N),    // window ≫ period → arrives far faster than the window
                _ => TimeSpan.FromTicks(PeriodTicks),
            };
        }
    }
}
