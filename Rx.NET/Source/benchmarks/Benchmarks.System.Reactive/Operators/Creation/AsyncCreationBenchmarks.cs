// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>
    /// S1: the <c>Task</c>-returning factory overloads. Each <c>Create</c> variant pushes N elements from its subscribe
    /// callback and returns an already-completed task, so completion is observed synchronously (Rx subscribes to a
    /// completed task inline). <c>Create_Task</c> (<c>Func&lt;IObserver, Task&gt;</c>, forwarded to the
    /// cancellation-token form), <c>Create_Task_Disposable</c> (<c>Task&lt;IDisposable&gt;</c>) and
    /// <c>Create_Task_Action</c> (<c>Task&lt;Action&gt;</c>) each run a separate <c>Create*Observable</c> class, distinct
    /// from the synchronous <c>Create</c> in <see cref="CreateBenchmarks"/>. <c>Using_Async</c> is the async
    /// <c>Using(resourceFactoryAsync, observableFactoryAsync)</c>, composed internally from <c>FromAsync</c>,
    /// <c>SelectMany</c>, <c>Using</c> and <c>Merge</c>; its factories return completed tasks, the inner one wrapping an
    /// N-element <c>Range</c>. All completed tasks are built once in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class AsyncCreationBenchmarks : OperatorVariantBenchmarkBase
    {
        private Task<IDisposable> _disposableTask;
        private Task<Action> _actionTask;
        private Task<IDisposable> _resourceTask;
        private Task<IObservable<int>> _observableTask;

        [GlobalSetup]
        public void Setup()
        {
            _disposableTask = Task.FromResult(Disposable.Empty);
            _actionTask = Task.FromResult<Action>(static () => { });
            _resourceTask = Task.FromResult(Disposable.Empty);
            _observableTask = Task.FromResult(Observable.Range(1, N));
        }

        [Benchmark]
        public void Create_Task()
        {
            var n = N;
            Observable.Create<int>(observer =>
            {
                for (var i = 0; i < n; i++)
                {
                    observer.OnNext(i);
                }

                observer.OnCompleted();
                return Task.CompletedTask;
            }).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void Create_Task_Disposable()
        {
            var n = N;
            var result = _disposableTask;
            Observable.Create<int>(observer =>
            {
                for (var i = 0; i < n; i++)
                {
                    observer.OnNext(i);
                }

                observer.OnCompleted();
                return result;
            }).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void Create_Task_Action()
        {
            var n = N;
            var result = _actionTask;
            Observable.Create<int>(observer =>
            {
                for (var i = 0; i < n; i++)
                {
                    observer.OnNext(i);
                }

                observer.OnCompleted();
                return result;
            }).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void Using_Async()
        {
            var resource = _resourceTask;
            var observable = _observableTask;
            Observable.Using(_ => resource, (_, _) => observable).SubscribeConsume(Consumer);
        }
    }
}
