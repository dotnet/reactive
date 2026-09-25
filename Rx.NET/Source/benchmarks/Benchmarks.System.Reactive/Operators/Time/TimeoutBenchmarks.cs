// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: <c>Timeout</c> in three modes. <c>NeverTrips</c> (timeout &gt; period) measures per-element timer-reset
    /// bookkeeping. <c>Trips</c> fires and switches to a fallback (completes). <c>Throws</c> fires with no fallback,
    /// so a real <c>TimeoutException</c> is propagated as <c>OnError</c> — the error path.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeoutBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Timeout_NeverTrips()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var timeout = TimeSpan.FromTicks(PeriodTicks * 4);   // always longer than the gap → never fires

            VirtualTimeSource.Timed(scheduler, n, period)
                .Timeout(timeout, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timeout_Trips()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var timeout = TimeSpan.FromTicks(PeriodTicks * 2);   // longer than the gap → all N elements reset the timer...

            // ...then the stream goes silent (Never), so the timer finally elapses after the last element and
            // Timeout trips, switching to the fallback. A timeout shorter than the first element's arrival would
            // instead fire at subscription and measure zero-element, N-independent work.
            VirtualTimeSource.Timed(scheduler, n, period).Concat(Observable.Never<int>())
                .Timeout(timeout, Observable.Empty<int>(), scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timeout_Throws()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var timeout = TimeSpan.FromTicks(PeriodTicks * 2);   // longer than the gap → all N elements reset the timer...

            // ...then silence trips the timer after the last element; with no fallback this surfaces as
            // OnError(TimeoutException) — the error path — instead of firing at subscription on zero elements.
            VirtualTimeSource.Timed(scheduler, n, period).Concat(Observable.Never<int>())
                .Timeout(timeout, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
