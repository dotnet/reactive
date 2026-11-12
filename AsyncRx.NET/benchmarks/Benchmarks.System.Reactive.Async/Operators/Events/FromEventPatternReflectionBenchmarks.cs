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
    /// Hot: the reflection-based <c>FromEventPattern</c> overloads, which find the event by name on an instance
    /// (target, eventName) or a type (static event). Subscribe, raise N events, dispose; the subscription pays
    /// the reflection lookup.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: disposing a <c>FromEvent</c>/<c>FromEventPattern</c> subscription throws
    /// <c>OperationCanceledException</c> (the <c>SubscribeOn</c> dispose action awaits a <c>RendezVous</c> on the
    /// token it has just cancelled). The benchmarks swallow it with
    /// <see cref="Pump.DisposeIgnoringCancellationAsync"/>, and use a fresh event host per invocation so a missed
    /// handler removal cannot accumulate handlers across invocations.
    /// </remarks>
    [BenchmarkCategory("Events")]
    public class FromEventPatternReflectionBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public async ValueTask FromEventPattern_Instance()
        {
            var host = new EventHost();
            var subscription = await AsyncObservable.FromEventPattern(host, nameof(EventHost.Instance), ImmediateAsyncScheduler.Instance)
                .SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            for (var i = 0; i < N; i++)
            {
                host.RaiseInstance();
            }

            await Pump.DisposeIgnoringCancellationAsync(subscription).ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask FromEventPattern_Static()
        {
            var subscription = await AsyncObservable.FromEventPattern(typeof(EventHost), nameof(EventHost.Static), ImmediateAsyncScheduler.Instance)
                .SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            for (var i = 0; i < N; i++)
            {
                EventHost.RaiseStatic();
            }

            await Pump.DisposeIgnoringCancellationAsync(subscription).ConfigureAwait(false);
        }

        public sealed class EventHost
        {
            public event EventHandler? Instance;

            public static event EventHandler? Static;

            public void RaiseInstance() => Instance?.Invoke(this, EventArgs.Empty);

            public static void RaiseStatic() => Static?.Invoke(null, EventArgs.Empty);
        }
    }
}
