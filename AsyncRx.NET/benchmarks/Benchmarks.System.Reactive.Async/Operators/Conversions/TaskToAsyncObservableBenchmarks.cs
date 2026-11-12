// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Conversions
{
    /// <summary>
    /// Per-subscription cost of turning a <see cref="Task"/> into an async observable: already-completed
    /// <c>Task</c> and <c>Task&lt;T&gt;</c>, a pending <c>Task&lt;T&gt;</c> completed after subscription, and the
    /// lower-level <c>AcceptAsync</c> that pushes a completed task's result straight into an observer.
    /// </summary>
    [BenchmarkCategory("Conversions")]
    public class TaskToAsyncObservableBenchmarks
    {
        private static readonly Task<int> CompletedTaskOfInt = Task.FromResult(42);

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask TaskOfT_Completed() =>
            CompletedTaskOfInt.ToAsyncObservable().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask TaskOfT_Completed_Scheduler() =>
            CompletedTaskOfInt.ToAsyncObservable(ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask Task_Completed() =>
            Task.CompletedTask.ToAsyncObservable().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public async ValueTask TaskOfT_Pending()
        {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            var sink = Sinks.For<int>(_consumer);
            await tcs.Task.ToAsyncObservable().SubscribeAsync(sink).ConfigureAwait(false);
            tcs.SetResult(42);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask TaskOfT_AcceptAsync()
        {
            var sink = Sinks.For<int>(_consumer);
            await CompletedTaskOfInt.AcceptAsync(sink).ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask Task_AcceptAsync()
        {
            var sink = Sinks.For<Unit>(_consumer);
            await Task.CompletedTask.AcceptAsync(sink).ConfigureAwait(false);
            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}
