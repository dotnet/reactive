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
    /// S3: the <c>Buffer</c> overloads beyond the hopping <c>Buffer(timeSpan)</c> (<see cref="BufferTimeBenchmarks"/>)
    /// and <c>Buffer(boundaries)</c> (<see cref="BufferWindowBoundaryBenchmarks"/>), each a distinct sink:
    /// <list type="bullet">
    /// <item><c>Buffer_Time_Count</c> — <c>Buffer(timeSpan, count)</c>, the <c>Ferry</c> sink. The count (8) is reached
    /// before the span (16 periods), so every buffer is closed by count and the timer is re-armed each time.</item>
    /// <item><c>Buffer_Time_Shift</c> — <c>Buffer(timeSpan, timeShift)</c> with span 8 and shift 4 periods, the
    /// overlapping <c>TimeSliding</c> sink (each element lands in two buffers).</item>
    /// <item><c>Buffer_ClosingSelector</c> — <c>Buffer(bufferClosingSelector)</c>, the <c>Selector</c> sink; the
    /// selector returns an 8-period one-shot timer, re-invoked after each close.</item>
    /// <item><c>Buffer_Openings_ClosingSelector</c> — <c>Buffer(openings, closingSelector)</c>, implemented as
    /// <c>Window(openings, ...)</c> (a <c>GroupJoin</c>) plus <c>SelectMany(ToList)</c>. Openings arrive every 4
    /// periods and each buffer lasts 8, so buffers overlap two-deep. The openings run slightly past the source so
    /// every source element is buffered. When the openings complete, <c>GroupJoin</c> completes and tears down the
    /// windows still open at that moment without closing them, so the last buffer or two is never emitted and the
    /// output never signals completion. Every subscription is still disposed, so <c>Start()</c> drains.</item>
    /// </list>
    /// All run on a plain <see cref="HistoricalScheduler"/>: none of these sinks uses <c>SchedulePeriodic</c>.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class BufferTimeVariantBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Time_Count()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 16);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(span, 8, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Buffer_Time_Shift()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);
            var shift = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(span, shift, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Buffer_ClosingSelector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(() => Observable.Timer(span, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Buffer_Openings_ClosingSelector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);
            var openings = VirtualTimeSource.Timed(scheduler, n / 4 + 2, TimeSpan.FromTicks(PeriodTicks * 4));

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(openings, _ => Observable.Timer(span, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
