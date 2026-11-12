// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Conversions
{
    /// <summary>
    /// S1 (variant sweep): bridging a synchronous Rx <c>IObservable&lt;T&gt;</c> into AsyncRx with
    /// <c>ToAsyncObservable</c>, on the immediate schedulers and on the default task-pool schedulers. The Rx source
    /// comes from the System.Reactive 6.0.0 package that System.Reactive.Async depends on, not from the in-repo Rx
    /// sources.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class ToAsyncObservableBenchmarks : OperatorVariantBenchmarkBase
    {
        private IObservable<int> _rx = default!;

        [GlobalSetup]
        public void Setup() => _rx = Observable.Range(1, N, global::System.Reactive.Concurrency.ImmediateScheduler.Instance);

        [Benchmark]
        public ValueTask ToAsyncObservable_Immediate() =>
            _rx.ToAsyncObservable(ImmediateAsyncScheduler.Instance, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToAsyncObservable_DefaultScheduler() =>
            _rx.ToAsyncObservable().SubscribeConsumeAsync(Consumer);
    }
}
