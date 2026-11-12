// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (variant sweep): the <c>Merge</c>, <c>Concat</c> and <c>Switch</c> overloads that
    /// <see cref="MergeBenchmarks"/>, <see cref="ConcatBenchmarks"/> and <see cref="SwitchBenchmarks"/> do not reach.
    /// <c>Merge_Params</c> merges four cold sources of N (it stands for the binary, <c>IEnumerable</c> and scheduler
    /// forms, which all forward to <c>ToAsyncObservable(scheduler).Merge()</c>). <c>Merge_MaxConcurrent</c> merges
    /// eight cold sources of N, two at a time. <c>Concat_Observable</c> concatenates four cold sources of N. The
    /// <c>_Tasks</c> rows flatten an outer of N already-completed tasks (one shared <c>Task.FromResult(42)</c>), so
    /// they measure the per-task bookkeeping, not task allocation.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class MergeConcatSwitchVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<IAsyncObservable<int>> _eight = default!;
        private IAsyncObservable<IAsyncObservable<int>> _four = default!;
        private IAsyncObservable<Task<int>> _tasks = default!;

        [GlobalSetup]
        public void Setup()
        {
            var s = BenchmarkSources.Range(N);

            var eight = new IAsyncObservable<int>[8];
            for (var i = 0; i < eight.Length; i++)
            {
                eight[i] = s;
            }

            _eight = new ArrayAsyncSource<IAsyncObservable<int>>(eight);
            _four = new ArrayAsyncSource<IAsyncObservable<int>>(new[] { s, s, s, s });

            var task = Task.FromResult(42);
            var tasks = new Task<int>[N];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = task;
            }

            _tasks = new ArrayAsyncSource<Task<int>>(tasks);
        }

        [Benchmark]
        public ValueTask Merge_Params()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.Merge(s, s, s, s).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Merge_MaxConcurrent() => _eight.Merge(2).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Merge_Tasks() => _tasks.Merge().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Concat_Observable() => _four.Concat().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Concat_Tasks() => _tasks.Concat().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Switch_Tasks() => _tasks.Switch().SubscribeConsumeAsync(Consumer);
    }
}
