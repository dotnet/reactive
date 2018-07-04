// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class ScalarScheduleBenchmark
    {
        private int _store;
        private Exception _exceptionStore;

        private IScheduler _eventLoop;

        private Exception _exception;

        [GlobalSetup]
        public void Setup()
        {
            _eventLoop = new EventLoopScheduler();
            _exception = new Exception();
        }

        private void BlockingConsume(IObservable<int> source)
        {
            var cde = new CountdownEvent(1);

            source.Subscribe(v => Volatile.Write(ref _store, v),
                e => 
                {
                    Volatile.Write(ref _exceptionStore, e);
                    cde.Signal();
                }, 
                () => cde.Signal()
            );

            // spin-wait will result in faster completion detection
            // because it takes 5 microseconds to resume a blocked thread
            // for me on Windows
            while (cde.CurrentCount != 0) ;
        }

        private void ConsumeSync(IObservable<int> source)
        {
            source.Subscribe(v => Volatile.Write(ref _store, v), e => Volatile.Write(ref _exceptionStore, e));
        }

        [Benchmark]
        public void Return_Immediate()
        {
            ConsumeSync(Observable.Return(1, ImmediateScheduler.Instance));
        }

        [Benchmark]
        public void Return_CurrentThread()
        {
            ConsumeSync(Observable.Return(1, CurrentThreadScheduler.Instance));
        }

        [Benchmark]
        public void Return_EventLoop()
        {
            BlockingConsume(Observable.Return(1, _eventLoop));
        }

        [Benchmark]
        public void Return_TaskPool()
        {
            BlockingConsume(Observable.Return(1, TaskPoolScheduler.Default));
        }

        [Benchmark]
        public void Return_ThreadPool()
        {
            BlockingConsume(Observable.Return(1, ThreadPoolScheduler.Instance));
        }

        [Benchmark]
        public void Throw_Immediate()
        {
            ConsumeSync(Observable.Throw<int>(_exception, ImmediateScheduler.Instance));
        }

        [Benchmark]
        public void Throw_CurrentThread()
        {
            ConsumeSync(Observable.Throw<int>(_exception, CurrentThreadScheduler.Instance));
        }

        [Benchmark]
        public void Throw_EventLoop()
        {
            BlockingConsume(Observable.Throw<int>(_exception, _eventLoop));
        }

        [Benchmark]
        public void Throw_TaskPool()
        {
            BlockingConsume(Observable.Throw<int>(_exception, TaskPoolScheduler.Default));
        }

        [Benchmark]
        public void Throw_ThreadPool()
        {
            BlockingConsume(Observable.Throw<int>(_exception, ThreadPoolScheduler.Instance));
        }

#if CURRENT

        [Benchmark]
        public void Prepend_Immediate()
        {
            ConsumeSync(Observable.Return(1, ImmediateScheduler.Instance).Prepend(0, ImmediateScheduler.Instance));
        }


        [Benchmark]
        public void Prepend_CurrentThread()
        {
            ConsumeSync(Observable.Return(1, CurrentThreadScheduler.Instance).Prepend(0, CurrentThreadScheduler.Instance));
        }

        [Benchmark]
        public void Prepend_EventLoop()
        {
            BlockingConsume(Observable.Return(1, _eventLoop).Prepend(0, _eventLoop));
        }

        [Benchmark]
        public void Prepend_TaskPool()
        {
            BlockingConsume(Observable.Return(1, TaskPoolScheduler.Default).Prepend(0, TaskPoolScheduler.Default));
        }

        [Benchmark]
        public void Prepend_ThreadPool()
        {
            BlockingConsume(Observable.Return(1, ThreadPoolScheduler.Instance).Prepend(0, ThreadPoolScheduler.Instance));
        }

        [Benchmark]
        public void Append_Immediate()
        {
            ConsumeSync(Observable.Return(1, ImmediateScheduler.Instance).Append(0, ImmediateScheduler.Instance));
        }


        [Benchmark]
        public void Append_CurrentThread()
        {
            ConsumeSync(Observable.Return(1, CurrentThreadScheduler.Instance).Append(0, CurrentThreadScheduler.Instance));
        }

        [Benchmark]
        public void Append_EventLoop()
        {
            BlockingConsume(Observable.Return(1, _eventLoop).Append(0, _eventLoop));
        }

        [Benchmark]
        public void Append_TaskPool()
        {
            BlockingConsume(Observable.Return(1, TaskPoolScheduler.Default).Append(0, TaskPoolScheduler.Default));
        }

        [Benchmark]
        public void Append_ThreadPool()
        {
            BlockingConsume(Observable.Return(1, ThreadPoolScheduler.Instance).Append(0, ThreadPoolScheduler.Instance));
        }
#endif
    }
}
