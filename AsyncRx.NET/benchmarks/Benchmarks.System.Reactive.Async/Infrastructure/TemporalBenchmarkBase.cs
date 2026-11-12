// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Base class for temporal (S3) operator benchmarks driven by <see cref="VirtualTimeAsyncScheduler"/>.
    /// <see cref="N"/> is capped below the throughput sweep because every element is at least one scheduler
    /// round and many timed operators take an <c>AsyncGate</c> per element. This type is <c>abstract</c> so
    /// auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every derived class inherits <see cref="Source_Baseline"/>: the benchmark-driven source alone (advance
    /// <see cref="PeriodTicks"/>, push, repeat N times, complete) into the consumer. It is the class baseline, so
    /// the Ratio column reads as "operator pipeline cost relative to driving the timeline". Derived classes must
    /// not declare their own <c>Baseline = true</c> method.
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

        [Params(100, 1_000, 10_000)]
        public int N;

        /// <summary>The source alone: N pushes <see cref="PeriodTicks"/> apart on a fresh scheduler, no operator.</summary>
        [Benchmark(Baseline = true)]
        public ValueTask Source_Baseline() =>
            RunTimedAsync(0, static (source, _, _) => source);

        /// <summary>
        /// Builds a pipeline over a pushed source, subscribes the consumer, pushes N elements
        /// <see cref="PeriodTicks"/> apart in virtual time, completes the source, and drains until the sink
        /// terminates.
        /// </summary>
        protected ValueTask RunTimedAsync<TState, TResult>(
            TState state,
            Func<IAsyncObservable<int>, VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunTimedAsync(N, PeriodTicks, sink, sink.Completion, state, build);
        }

        /// <summary>As <see cref="RunTimedAsync"/> with two pushed sources that receive each value in turn (left, then right).</summary>
        protected ValueTask RunTimedPairAsync<TState, TResult>(
            TState state,
            Func<IAsyncObservable<int>, IAsyncObservable<int>, VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunTimedPairAsync(N, PeriodTicks, sink, sink.Completion, state, build);
        }

        /// <summary>For time <i>sources</i> (Timer, Interval, timed Generate): subscribe, then drain until the sink terminates.</summary>
        protected ValueTask RunSourceAsync<TState, TResult>(
            TState state,
            Func<VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var sink = Sinks.For<TResult>(Consumer);
            return VirtualTimeDriver.RunSourceAsync(sink, sink.Completion, state, build);
        }
    }

    /// <summary>The virtual-time driving loops shared by <see cref="TemporalBenchmarkBase"/> and <see cref="SelfCheck"/>.</summary>
    public static class VirtualTimeDriver
    {
        public static async ValueTask RunTimedAsync<TState, TResult>(
            int n,
            long periodTicks,
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<IAsyncObservable<int>, VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new VirtualTimeAsyncScheduler();
            var source = new PushSource<int>();

            await build(source, scheduler, state).SubscribeAsync(sink).ConfigureAwait(false);
            await scheduler.AdvanceByAsync(TimeSpan.Zero).ConfigureAwait(false); // work scheduled at subscription time

            var period = TimeSpan.FromTicks(periodTicks);
            for (var i = 0; i < n; i++)
            {
                await scheduler.AdvanceByAsync(period).ConfigureAwait(false);
                await source.OnNextAsync(i).ConfigureAwait(false);
            }

            await source.OnCompletedAsync().ConfigureAwait(false);
            await scheduler.RunUntilAsync(done).ConfigureAwait(false);
        }

        /// <summary>
        /// As <see cref="RunTimedAsync"/> with two pushed sources: at each step it advances time, then pushes the
        /// same value into the left source and then the right source.
        /// </summary>
        public static async ValueTask RunTimedPairAsync<TState, TResult>(
            int n,
            long periodTicks,
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<IAsyncObservable<int>, IAsyncObservable<int>, VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new VirtualTimeAsyncScheduler();
            var left = new PushSource<int>();
            var right = new PushSource<int>();

            await build(left, right, scheduler, state).SubscribeAsync(sink).ConfigureAwait(false);
            await scheduler.AdvanceByAsync(TimeSpan.Zero).ConfigureAwait(false);

            var period = TimeSpan.FromTicks(periodTicks);
            for (var i = 0; i < n; i++)
            {
                await scheduler.AdvanceByAsync(period).ConfigureAwait(false);
                await left.OnNextAsync(i).ConfigureAwait(false);
                await right.OnNextAsync(i).ConfigureAwait(false);
            }

            await left.OnCompletedAsync().ConfigureAwait(false);
            await right.OnCompletedAsync().ConfigureAwait(false);
            await scheduler.RunUntilAsync(done).ConfigureAwait(false);
        }

        public static async ValueTask RunSourceAsync<TState, TResult>(
            IAsyncObserver<TResult> sink,
            CompletionSignal done,
            TState state,
            Func<VirtualTimeAsyncScheduler, TState, IAsyncObservable<TResult>> build)
        {
            var scheduler = new VirtualTimeAsyncScheduler();
            await build(scheduler, state).SubscribeAsync(sink).ConfigureAwait(false);
            await scheduler.RunUntilAsync(done).ConfigureAwait(false);
        }
    }
}
