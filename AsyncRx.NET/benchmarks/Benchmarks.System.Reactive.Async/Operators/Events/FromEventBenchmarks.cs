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
    /// Hot: <c>FromEvent</c> over a plain .NET event, on the immediate scheduler: subscribe (adds the handler),
    /// raise N events, dispose (removes it). Covers the <c>Action&lt;T&gt;</c> form, the delegate-type form, the
    /// conversion form and the parameterless <c>Action</c> form. AsyncRx shares the event through
    /// <c>Publish().RefCount()</c>, and raising an event starts each <c>OnNextAsync</c> without awaiting it.
    /// </summary>
    /// <remarks>
    /// Each invocation uses a fresh event host, so handlers cannot accumulate across invocations even if a
    /// removal were missed. The dispose (which removes the handler) is part of the measurement.
    /// </remarks>
    [BenchmarkCategory("Events")]
    public class FromEventBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask FromEvent_Action()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<int>(h => host.Value += h, h => host.Value -= h, ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEvent_Delegate()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<Action<int>, int>(h => host.Value += h, h => host.Value -= h, ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEvent_Conversion()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<EventHandler<int>, int>(
                static h => (_, e) => h(e),
                h => host.Handler += h,
                h => host.Handler -= h,
                ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public async ValueTask FromEvent_Unit()
        {
            var host = new EventHost();
            var subscription = await AsyncObservable.FromEvent(h => host.Signal += h, h => host.Signal -= h, ImmediateAsyncScheduler.Instance)
                .SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            for (var i = 0; i < N; i++)
            {
                host.RaiseSignal();
            }

            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        private async ValueTask RaiseAsync(EventHost host, IAsyncObservable<int> events)
        {
            var subscription = await events.SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            for (var i = 0; i < N; i++)
            {
                host.Raise(i);
            }

            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        private sealed class EventHost
        {
            public event Action<int>? Value;

            public event EventHandler<int>? Handler;

            public event Action? Signal;

            public void Raise(int value)
            {
                Value?.Invoke(value);
                Handler?.Invoke(this, value);
            }

            public void RaiseSignal() => Signal?.Invoke();
        }
    }
}
