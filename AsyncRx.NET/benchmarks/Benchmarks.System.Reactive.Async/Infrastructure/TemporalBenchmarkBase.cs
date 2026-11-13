// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Microsoft.Reactive.Testing.Async;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Base class for temporal (S3) operator benchmarks driven by <see cref="TestAsyncScheduler"/>, the virtual-time
    /// scheduler from <c>Microsoft.Reactive.Testing.Async</c>. <see cref="N"/> is capped below the throughput sweep
    /// because every element costs at least one timed work item on the scheduler. This type is <c>abstract</c> so
    /// auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every derived class inherits <see cref="Source_Baseline"/>: the benchmark-driven source alone (N pushes
    /// <see cref="PeriodTicks"/> apart, then completion) into the consumer. It is the class baseline, so the Ratio
    /// column reads as "operator pipeline cost relative to driving the timeline", and the baseline includes the
    /// scheduler's own cost. Derived classes must not declare their own <c>Baseline = true</c> method.
    /// </para>
    /// <para>
    /// The source is pushed by the benchmark (<see cref="VirtualTimeDriver.RunTimedAsync"/>) rather than built
    /// from a library timed <c>Generate</c>, because <c>Generate</c> pays a <c>RendezVous</c>/<c>ExecuteAsync</c>
    /// round per element, which would swamp the baseline. Pass <c>static</c> lambdas with explicit state so no
    /// closure is allocated per invocation.
    /// </para>
    /// </remarks>
    public abstract class TemporalBenchmarkBase : BenchmarkBase
    {
        /// <summary>Fixed source inter-arrival period, in virtual ticks.</summary>
        protected const long PeriodTicks = 10;

        /// <summary>The stream midpoint for <paramref name="n"/> elements; see <see cref="VirtualTimeDriver.MidpointTicks"/>.</summary>
        protected static long MidpointTicks(int n) => VirtualTimeDriver.MidpointTicks(n, PeriodTicks);

        [Params(100, 1_000, 10_000)]
        public int N;

        /// <summary>The source alone: N pushes <see cref="PeriodTicks"/> apart on a fresh scheduler, no operator.</summary>
        [Benchmark(Baseline = true)]
        public ValueTask Source_Baseline() =>
            RunTimedAsync(0, static (source, _, _) => source);

        /// <summary>
        /// Builds a pipeline over a pushed source, subscribes the consumer, pushes N elements
        /// <see cref="PeriodTicks"/> apart in virtual time, completes the source, and drains until no work remains.
        /// </summary>
        protected ValueTask RunTimedAsync<TState, TResult>(
            TState state,
            Func<IAsyncObservable<int>, TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, sink, sink.Completion, state, build);
        }

        /// <summary>As <see cref="RunTimedAsync"/> with two pushed sources that receive each value in turn (left, then right).</summary>
        protected ValueTask RunTimedPairAsync<TState, TResult>(
            TState state,
            Func<IAsyncObservable<int>, IAsyncObservable<int>, TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunTimedPairAsync(N, PeriodTicks, sink, sink.Completion, state, build);
        }

        /// <summary>For time <i>sources</i> (Timer, Interval, timed Generate): subscribe, then drain until no work remains.</summary>
        protected ValueTask RunSourceAsync<TState, TResult>(
            TState state,
            Func<TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunSourceAsync(sink, sink.Completion, state, build);
        }
    }

    /// <summary>
    /// The virtual-time driving loops shared by <see cref="TemporalBenchmarkBase"/> and <see cref="SelfCheck"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="TestAsyncScheduler"/> is a synchronous, single-threaded pump: <c>Start</c> runs every due item,
    /// and every continuation those items ready, inline on the calling thread, advancing the clock between them.
    /// So these loops complete synchronously; they return <see cref="ValueTask"/> only to keep the benchmark
    /// method shape.
    /// </para>
    /// <para>
    /// The pipeline is subscribed from the benchmark body, before any work has run. Everything after that runs
    /// inside the pump: the pushes are a chain of timed work items (<see cref="PushChain"/>), each pushing one
    /// value and scheduling the next <c>periodTicks</c> later, and the last one completes the sources. Pushing
    /// from the body between <c>AdvanceBy</c> calls does not work: an operator that cancels or completes an
    /// in-flight timer in response to a push would then do so off the pump, which the scheduler reports as an
    /// escape.
    /// </para>
    /// <para>
    /// <c>Start</c> throws if scheduled work fails, escapes to a real thread, or is left outstanding when the
    /// timeline runs out, and <see cref="Drain"/> throws if the sink never terminated. A stuck pipeline therefore
    /// fails the benchmark instead of hanging it.
    /// </para>
    /// </remarks>
    public static class VirtualTimeDriver
    {
        /// <summary>
        /// The midpoint of an <paramref name="n"/>-element timed source, in virtual ticks: half a period after element
        /// <c>n/2 - 1</c>, which is due at <c>periodTicks * (n/2)</c>. The half-period offset keeps the boundary off an
        /// element's tick, so the result does not depend on how the scheduler orders work due at the same time: a
        /// time-based Skip drops exactly <c>n/2</c> elements and a Take keeps exactly <c>n/2</c>, for odd <c>n</c> too.
        /// The Rx suite's <c>TemporalBenchmarkBase.MidpointTicks</c> uses the same formula.
        /// </summary>
        public static long MidpointTicks(int n, long periodTicks) => periodTicks * (n / 2) + periodTicks / 2;

        public static ValueTask RunTimedAsync<TState, TResult>(
            int n,
            long periodTicks,
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<IAsyncObservable<int>, TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new TestAsyncScheduler();
            var source = new PushSource<int>();

            Complete(scheduler, build(source, scheduler, state).SubscribeAsync(sink));
            PushChain.Start(scheduler, source, null, n, periodTicks);
            Drain(scheduler, done);
            return default;
        }

        /// <summary>
        /// As <see cref="RunTimedAsync"/> with two pushed sources: at each step the same value goes into the left
        /// source and then the right source.
        /// </summary>
        public static ValueTask RunTimedPairAsync<TState, TResult>(
            int n,
            long periodTicks,
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<IAsyncObservable<int>, IAsyncObservable<int>, TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new TestAsyncScheduler();
            var left = new PushSource<int>();
            var right = new PushSource<int>();

            Complete(scheduler, build(left, right, scheduler, state).SubscribeAsync(sink));
            PushChain.Start(scheduler, left, right, n, periodTicks);
            Drain(scheduler, done);
            return default;
        }

        public static ValueTask RunSourceAsync<TState, TResult>(
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<TestAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new TestAsyncScheduler();
            Complete(scheduler, build(scheduler, state).SubscribeAsync(sink));
            Drain(scheduler, done);
            return default;
        }

        // Runs the timeline until no work remains, then requires the sink to have terminated: a pipeline that
        // ends with nothing scheduled but never completed would otherwise pass silently.
        private static void Drain(TestAsyncScheduler scheduler, CompletionSignal done)
        {
            scheduler.Start();
            if (!done.IsSet)
            {
                throw new InvalidOperationException(
                    $"The pipeline did not terminate: the virtual-time pump ran out of work at tick {scheduler.Clock} before the sink received OnCompleted or OnError.");
            }
        }

        // Body-level operations run before the pump has run anything, so they normally complete synchronously;
        // if one does not, RunToCompletion pumps the current tick until it does.
        private static void Complete<T>(TestAsyncScheduler scheduler, ValueTask<T> operation)
        {
            if (!operation.IsCompletedSuccessfully)
            {
                scheduler.RunToCompletion(operation).AsTask().GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Pushes 0..n-1 into one or two sources, one value every <c>period</c> ticks starting at tick
        /// <c>period</c>, then completes them, all as scheduled work inside the pump. One instance per run; the
        /// state-argument scheduling overloads mean no closure per step.
        /// </summary>
        private sealed class PushChain
        {
            private readonly TestAsyncScheduler _scheduler;
            private readonly PushSource<int> _first;
            private readonly PushSource<int>? _second;
            private readonly int _n;
            private readonly TimeSpan _period;
            private int _next;

            private PushChain(TestAsyncScheduler scheduler, PushSource<int> first, PushSource<int>? second, int n, long periodTicks)
            {
                _scheduler = scheduler;
                _first = first;
                _second = second;
                _n = n;
                _period = TimeSpan.FromTicks(periodTicks);
            }

            public static void Start(TestAsyncScheduler scheduler, PushSource<int> first, PushSource<int>? second, int n, long periodTicks)
            {
                var chain = new PushChain(scheduler, first, second, n, periodTicks);
                Complete(scheduler, chain.ScheduleNext());
            }

            // A zero-length run completes at tick 0; otherwise the first value arrives at tick `period`.
            private ValueTask<IAsyncDisposable> ScheduleNext() =>
                _next < _n
                    ? _scheduler.ScheduleAsync(this, _period, static (chain, _) => chain.StepAsync())
                    : _scheduler.ScheduleAsync(this, static (chain, _) => chain.CompleteAsync());

            private async ValueTask StepAsync()
            {
                var value = _next++;
                await _first.OnNextAsync(value).ConfigureAwait(false);
                if (_second != null)
                {
                    await _second.OnNextAsync(value).ConfigureAwait(false);
                }

                if (_next < _n)
                {
                    await ScheduleNext().ConfigureAwait(false);
                }
                else
                {
                    await CompleteAsync().ConfigureAwait(false);
                }
            }

            private async ValueTask CompleteAsync()
            {
                await _first.OnCompletedAsync().ConfigureAwait(false);
                if (_second != null)
                {
                    await _second.OnCompletedAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
