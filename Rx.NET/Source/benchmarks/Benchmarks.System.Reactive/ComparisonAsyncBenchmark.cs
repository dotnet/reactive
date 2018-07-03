// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class ComparisonAsyncBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;
        private int _store;

        IScheduler _scheduler1;
        IScheduler _scheduler2;

        [GlobalSetup]
        public void Setup()
        {
            _scheduler1 = new EventLoopScheduler();
            _scheduler2 = new EventLoopScheduler();
        }

        [Benchmark]
        public void ObserveOn()
        {
            var cde = new CountdownEvent(1);

            Observable.Range(1, N).ObserveOn(_scheduler1)
                .Subscribe(v => Volatile.Write(ref _store, v), () => cde.Signal());

            if (N <= 1000)
            {
                while (cde.CurrentCount != 0) ;
            }
            else
            {
                cde.Wait();
            }
        }

        [Benchmark]
        public void SubscribeOn()
        {
            var cde = new CountdownEvent(1);

            Observable.Range(1, N).SubscribeOn(_scheduler1)
                .Subscribe(v => Volatile.Write(ref _store, v), () => cde.Signal());

            if (N <= 1000)
            {
                while (cde.CurrentCount != 0) ;
            }
            else
            {
                cde.Wait();
            }
        }

        [Benchmark]
        public void SubscribeOnObserveOn()
        {
            var cde = new CountdownEvent(1);

            Observable.Range(1, N)
                .SubscribeOn(_scheduler1)
                .ObserveOn(_scheduler2)
                .Subscribe(v => Volatile.Write(ref _store, v), () => cde.Signal());

            if (N <= 1000)
            {
                while (cde.CurrentCount != 0) ;
            }
            else
            {
                cde.Wait();
            }
        }
    }
}
