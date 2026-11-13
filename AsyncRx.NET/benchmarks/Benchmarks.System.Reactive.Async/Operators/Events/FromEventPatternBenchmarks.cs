// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Events
{
    /// <summary>
    /// Hot: <c>FromEventPattern</c> over (sender, args) events, on the immediate scheduler: subscribe, raise N
    /// events, dispose. Covers <see cref="EventHandler"/>, <see cref="EventHandler{TEventArgs}"/> and the
    /// conversion form. Each event allocates an <c>EventPattern</c>.
    /// </summary>
    /// <remarks>
    /// Each invocation uses a fresh event host, so handlers cannot accumulate across invocations even if a
    /// removal were missed. The dispose (which removes the handler) is part of the measurement.
    /// </remarks>
    [BenchmarkCategory("Events")]
    public class FromEventPatternBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask FromEventPattern_EventHandler()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEventPattern(h => host.Plain += h, h => host.Plain -= h, ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEventPattern_Generic()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEventPattern<EventArgs>(h => host.Generic += h, h => host.Generic -= h, ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEventPattern_Conversion()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEventPattern<EventHandler, EventArgs>(
                static h => (s, e) => h(s, e),
                h => host.Plain += h,
                h => host.Plain -= h,
                ImmediateAsyncScheduler.Instance));
        }

        private async ValueTask RaiseAsync<T>(EventHost host, IAsyncObservable<T> events)
        {
            var subscription = await events.SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            for (var i = 0; i < N; i++)
            {
                host.Raise();
            }

            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        private sealed class EventHost
        {
            public event EventHandler? Plain;

            public event EventHandler<EventArgs>? Generic;

            public void Raise()
            {
                Plain?.Invoke(this, EventArgs.Empty);
                Generic?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
