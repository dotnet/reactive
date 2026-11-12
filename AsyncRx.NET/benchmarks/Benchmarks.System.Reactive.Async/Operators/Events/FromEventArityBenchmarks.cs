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
    /// Hot: <c>FromEvent</c> over multi-argument events, on the immediate scheduler: subscribe (adds the handler),
    /// raise N events, dispose (removes it). Covers the <c>Action&lt;T1, T2&gt;</c> and
    /// <c>Action&lt;T1, T2, T3&gt;</c> forms and the arity-3 conversion form over a custom delegate type. Each
    /// event becomes a value tuple. As in <c>FromEventBenchmarks</c>, AsyncRx shares the event through
    /// <c>Publish().RefCount()</c>, raising an event starts each <c>OnNextAsync</c> without awaiting it, and
    /// each invocation uses a fresh event host.
    /// </summary>
    [BenchmarkCategory("Events")]
    public class FromEventArityBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        /// <summary>A custom three-argument event delegate, for the conversion form.</summary>
        public delegate void Int3Handler(int a, int b, int c);

        [Benchmark]
        public ValueTask FromEvent_Action_Arity2()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<int, int>(
                (Action<int, int> h) => host.Value2 += h,
                (Action<int, int> h) => host.Value2 -= h,
                ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEvent_Action_Arity3()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<int, int, int>(
                (Action<int, int, int> h) => host.Value3 += h,
                (Action<int, int, int> h) => host.Value3 -= h,
                ImmediateAsyncScheduler.Instance));
        }

        [Benchmark]
        public ValueTask FromEvent_Conversion_Arity3()
        {
            var host = new EventHost();
            return RaiseAsync(host, AsyncObservable.FromEvent<Int3Handler, int, int, int>(
                static h => (a, b, c) => h(a, b, c),
                h => host.Handler3 += h,
                h => host.Handler3 -= h,
                ImmediateAsyncScheduler.Instance));
        }

        private async ValueTask RaiseAsync<T>(EventHost host, IAsyncObservable<T> events)
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
            public event Action<int, int>? Value2;

            public event Action<int, int, int>? Value3;

            public event Int3Handler? Handler3;

            public void Raise(int value)
            {
                Value2?.Invoke(value, value);
                Value3?.Invoke(value, value, value);
                Handler3?.Invoke(value, value, value);
            }
        }
    }
}
