// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Async
{
    /// <summary>
    /// Per-subscription cost of the factories that turn a function or an asynchronous operation into a one-value
    /// observable: <c>Start</c> (function and action, immediate scheduler and the default task-pool scheduler),
    /// <c>StartAsync</c>, <c>FromAsync</c> (<c>ValueTask&lt;T&gt;</c>, <c>Task</c> and cancellation-token forms) and
    /// <c>ToAsync</c> at arity 0 and 2. The operations complete synchronously. The <c>ToAsync</c> delegates are
    /// built once.
    /// </summary>
    [BenchmarkCategory("Async")]
    public class AsyncFactoryBenchmarks
    {
        private static readonly Func<IAsyncObservable<int>> ToAsync0 = AsyncObservable.ToAsync(static () => 42, ImmediateAsyncScheduler.Instance);
        private static readonly Func<int, int, IAsyncObservable<int>> ToAsync2 = AsyncObservable.ToAsync<int, int, int>(static (a, b) => a + b, ImmediateAsyncScheduler.Instance);

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask Start_Func() =>
            AsyncObservable.Start(static () => 42, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Start_Func_DefaultScheduler() =>
            AsyncObservable.Start(static () => 42).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Start_Action() =>
            AsyncObservable.Start(static () => { }, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask StartAsync() =>
            AsyncObservable.StartAsync(static () => new ValueTask<int>(42)).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsync_ValueTask() =>
            AsyncObservable.FromAsync(static () => new ValueTask<int>(42), ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsync_CancellationToken() =>
            AsyncObservable.FromAsync(static (CancellationToken _) => new ValueTask<int>(42), ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsync_Task() =>
            AsyncObservable.FromAsync(static () => Task.CompletedTask, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask ToAsync_Arity0() => ToAsync0().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask ToAsync_Arity2() => ToAsync2(20, 22).SubscribeConsumeAsync(_consumer);
    }
}
