// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

#if (CURRENT)
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class AppendPrependBenchmark
    {
        [Params(1, 10, 100, 1000, 10000)]
        public int N;

        private int _store;

        [Benchmark(Baseline = true)]
        public void StartWithArray()
        {
            var array = new int[2 * N];
            var max = 2 * N - 1;

            for (var i = 0; i < N; i++)
            {
                array[i] = i;
                array[max - i] = i;
            }

            Observable
                .Empty<int>()
                .StartWith(array)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void StartWithList()
        {
            var list = new List<int>();

            for (var i = 0; i < N; i++)
            {
                list.Insert(i, 0);
                list.Add(i);
            }

            Observable
                .Empty<int>()
                .StartWith(list)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void StartWithLinkedList()
        {
            var list = new LinkedList<int>();

            for (var i = 0; i < N; i++)
            {
                list.AddFirst(i);
                list.AddLast(i);
            }

            Observable
                .Empty<int>()
                .StartWith(list)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void AppendPrepend()
        {
            var obs = Observable.Empty<int>();

            for (var i = 0; i < N; i++)
            {
                obs = obs.Prepend(i);
                obs = obs.Append(i);
            }

            obs.Subscribe(v => Volatile.Write(ref _store, v));
        }
    }
}
#endif
