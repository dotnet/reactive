// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

using Microsoft.Reactive.Testing.Async;

namespace Benchmarks.System.Reactive.Async.Operators.Lifecycle
{
    /// <summary>
    /// Lifecycle: tears down a <b>still-live</b> subscription mid-stream. The hot <see cref="PushSource{T}"/>
    /// sources never complete, so <c>DisposeAsync</c> lands on an active subscription after <c>N</c> pushes. This
    /// exercises the unsubscribe and cleanup path of operators with non-trivial disposal, which cold-source
    /// benchmarks miss.
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class DisposeBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public async ValueTask Merge_Dispose()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            var merged = new ArrayAsyncSource<IAsyncObservable<int>>(new IAsyncObservable<int>[] { a, b }).Merge();
            var subscription = await merged.SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            await PushPairsAsync(a, b).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask CombineLatest_Dispose()
        {
            var a = new PushSource<int>();
            var b = new PushSource<int>();
            var subscription = await a.CombineLatest(b, static (x, y) => x + y).SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            await PushPairsAsync(a, b).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask Switch_Dispose()
        {
            var outer = new PushSource<IAsyncObservable<int>>();
            var inner = new PushSource<int>();
            var subscription = await outer.Switch().SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            await outer.OnNextAsync(inner).ConfigureAwait(false);
            await inner.PushAsync(N / 2).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public ValueTask GroupBy_Dispose()
        {
            var source = new PushSource<int>();
            return source.GroupBy(static v => v & 7).SelectMany(static g => g).RunAndDisposeAsync(source, N / 2, _consumer);
        }

        [Benchmark]
        public ValueTask Window_Dispose()
        {
            var source = new PushSource<int>();
            return source.Window(16, 8).SelectMany(static w => w).RunAndDisposeAsync(source, N / 2, _consumer);
        }

        [Benchmark]
        public async ValueTask Publish_Dispose()
        {
            var source = new PushSource<int>();
            var published = source.Publish();
            var subscription = await published.SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            var connection = await published.ConnectAsync().ConfigureAwait(false);
            await source.PushAsync(N / 2).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
            await connection.DisposeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Throttle on the virtual-time scheduler. The pushes run inside the pump, <see cref="ThrottlePeriodTicks"/>
        /// apart, and the throttle window is three periods, so each push cancels the timer that the previous push
        /// armed, and no timer ever fires. One period after the last push the subscription is disposed, which
        /// cancels the last timer. The row includes the scheduler's cost per element; SelfCheck asserts that
        /// nothing is emitted and no timer runs after disposal.
        /// </summary>
        [Benchmark]
        public ValueTask Throttle_Dispose()
        {
            VirtualTimeDriver.RunTimedThenDispose(N / 2, ThrottlePeriodTicks, Sinks.ConsumeOnly<int>(_consumer), ThrottleWindow, ThrottlePipeline);
            return default;
        }

        /// <summary>The push period of <see cref="Throttle_Dispose"/>, in virtual ticks.</summary>
        internal const long ThrottlePeriodTicks = 10;

        /// <summary>The throttle window of <see cref="Throttle_Dispose"/>: three push periods.</summary>
        internal static readonly TimeSpan ThrottleWindow = TimeSpan.FromTicks(3 * ThrottlePeriodTicks);

        /// <summary>The pipeline of <see cref="Throttle_Dispose"/>, shared with SelfCheck.</summary>
        internal static readonly Func<IAsyncObservable<int>, TestAsyncScheduler, TimeSpan, IAsyncObservable<int>> ThrottlePipeline =
            static (source, scheduler, window) => source.Throttle(window, scheduler);

        private ValueTask PushPairsAsync(PushSource<int> a, PushSource<int> b) => Pump.PushPairsAsync(a, b, N / 2);
    }
}
