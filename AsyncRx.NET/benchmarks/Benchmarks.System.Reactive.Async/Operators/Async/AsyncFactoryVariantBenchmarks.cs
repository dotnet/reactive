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
    /// Per-subscription cost of the factory overloads that <c>AsyncFactoryBenchmarks</c> does not cover:
    /// <c>StartAsync</c> with a cancellation token and with <see cref="Task"/>-returning actions, <c>FromAsync</c>
    /// with a token-taking <see cref="Task"/> action, and <c>ToAsync</c> over <see cref="Action"/> delegates at
    /// arity 0 and 2. Every row passes the immediate scheduler, and every operation completes synchronously. The
    /// <c>ToAsync</c> delegates are built once.
    /// </summary>
    [BenchmarkCategory("Async")]
    public class AsyncFactoryVariantBenchmarks
    {
        private static readonly Func<IAsyncObservable<Unit>> ToAsyncAction0 = AsyncObservable.ToAsync(static () => { }, ImmediateAsyncScheduler.Instance);
        private static readonly Func<int, int, IAsyncObservable<Unit>> ToAsyncAction2 = AsyncObservable.ToAsync<int, int>(static (a, b) => { }, ImmediateAsyncScheduler.Instance);

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask StartAsync_CancellationToken() =>
            AsyncObservable.StartAsync(static (CancellationToken _) => new ValueTask<int>(42), ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask StartAsync_Task() =>
            AsyncObservable.StartAsync(static () => Task.CompletedTask, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask StartAsync_Task_CancellationToken() =>
            AsyncObservable.StartAsync(static (CancellationToken _) => Task.CompletedTask, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsync_Task_CancellationToken() =>
            AsyncObservable.FromAsync(static (CancellationToken _) => Task.CompletedTask, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask ToAsync_Action_Arity0() => ToAsyncAction0().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask ToAsync_Action_Arity2() => ToAsyncAction2(20, 22).SubscribeConsumeAsync(_consumer);
    }
}
