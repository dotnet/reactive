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
    /// S3: the <c>Window</c> counterparts of <see cref="BufferTimeVariantBenchmarks"/>, beyond the hopping
    /// <c>Window(timeSpan)</c> (<see cref="WindowTimeBenchmarks"/>) and <c>Window(boundaries)</c>
    /// (<see cref="BufferWindowBoundaryBenchmarks"/>): <c>Window(timeSpan, count)</c> (<c>Ferry</c> sink; count 8 is
    /// reached before the 16-period span), <c>Window(timeSpan, timeShift)</c> (overlapping <c>TimeSliding</c> sink,
    /// span 8 / shift 4 periods), <c>Window(windowClosingSelector)</c> (<c>Selector</c> sink, 8-period one-shot timer)
    /// and <c>Window(openings, closingSelector)</c> (a <c>GroupJoin</c>; openings every 4 periods, each window 8 long,
    /// openings running slightly past the source so every element is windowed; as in the Buffer case, the windows
    /// still open when the openings complete are torn down unclosed, so the flattened output never signals
    /// completion, but every subscription is disposed and <c>Start()</c> drains). Inner windows are flattened with
    /// <c>SelectMany</c>, as in <see cref="WindowTimeBenchmarks"/>, so element delivery is measured and not just
    /// window creation. All run on a plain <see cref="HistoricalScheduler"/>: none of these sinks uses
    /// <c>SchedulePeriodic</c>.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class WindowTimeVariantBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Window_Time_Count()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 16);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(span, 8, scheduler)
                .SelectMany(static w => w)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Window_Time_Shift()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);
            var shift = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(span, shift, scheduler)
                .SelectMany(static w => w)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Window_ClosingSelector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(() => Observable.Timer(span, scheduler))
                .SelectMany(static w => w)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Window_Openings_ClosingSelector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var span = TimeSpan.FromTicks(PeriodTicks * 8);
            var openings = VirtualTimeSource.Timed(scheduler, n / 4 + 2, TimeSpan.FromTicks(PeriodTicks * 4));

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(openings, _ => Observable.Timer(span, scheduler))
                .SelectMany(static w => w)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
