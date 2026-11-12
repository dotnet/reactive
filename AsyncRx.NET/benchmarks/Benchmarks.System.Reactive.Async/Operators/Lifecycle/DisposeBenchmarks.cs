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

        // KNOWN GAP: Window(count) throws "Queue empty" once `count` elements have arrived (it opens its first window
        // only after `skip` elements but tries to close one after `count`). The overlapping Window(count, skip) form
        // with skip < count does not hit that path, so it is used here.
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

        [Benchmark]
        public async ValueTask Throttle_Dispose()
        {
            // The throttle window is far longer than the run, so the timer never fires; disposal cancels it.
            var scheduler = new TestAsyncScheduler();
            var source = new PushSource<int>();
            var subscription = await source.Throttle(TimeSpan.FromTicks(long.MaxValue / 4), scheduler).SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            await source.PushAsync(N / 2).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        private async ValueTask PushPairsAsync(PushSource<int> a, PushSource<int> b)
        {
            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                await a.OnNextAsync(i).ConfigureAwait(false);
                await b.OnNextAsync(i).ConfigureAwait(false);
            }
        }
    }
}
