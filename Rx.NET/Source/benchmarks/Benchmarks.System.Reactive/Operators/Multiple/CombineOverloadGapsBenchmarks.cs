// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: the remaining combining overload shapes. <c>Zip_Enumerable</c> zips an observable with an
    /// <c>IEnumerable</c> (the <c>Zip.Enumerable</c> sink, which pulls one item per <c>OnNext</c>) over a pre-built
    /// N-element array. <c>Merge_Task</c>, <c>Concat_Task</c> and <c>Switch_Task</c> flatten an
    /// <c>IObservable&lt;Task&lt;int&gt;&gt;</c> of N already-completed tasks: <c>Merge</c> runs its dedicated
    /// <c>Merge.Tasks</c> sink (completed-task fast path), while <c>Concat</c>/<c>Switch</c> convert each task with
    /// <c>ToObservable</c> and run their usual inner-observable sinks. One pre-completed <c>Task&lt;int&gt;</c> and the
    /// task-stream source are built in <c>[GlobalSetup]</c>, so no task is allocated per element.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineOverloadGapsBenchmarks : OperatorVariantBenchmarkBase
    {
        private int[] _second;
        private IObservable<Task<int>> _tasks;

        [GlobalSetup]
        public void Setup()
        {
            _second = new int[N];
            for (var i = 0; i < _second.Length; i++)
            {
                _second[i] = i;
            }

            var completed = Task.FromResult(42);
            _tasks = Observable.Range(1, N).Select(_ => completed);
        }

        [Benchmark]
        public void Zip_Enumerable() =>
            Observable.Range(1, N).Zip(_second, static (a, b) => a + b).SubscribeConsume(Consumer);

        [Benchmark]
        public void Merge_Task() => _tasks.Merge().SubscribeConsume(Consumer);

        [Benchmark]
        public void Concat_Task() => _tasks.Concat().SubscribeConsume(Consumer);

        [Benchmark]
        public void Switch_Task() => _tasks.Switch().SubscribeConsume(Consumer);
    }
}
