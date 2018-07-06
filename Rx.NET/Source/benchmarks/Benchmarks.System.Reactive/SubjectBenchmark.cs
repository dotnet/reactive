// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class SubjectBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;

        [Params(0, 1, 2, 3, 4, 5)]
        public int M;

        private int _store;

        [Benchmark]
        public object SubjectPush()
        {
            var subj = new Subject<int>();
            var consumers = new IDisposable[M];
            var m = M;
            for (var i = 0; i < m; i++)
            {
                consumers[i] = subj.Subscribe(v => Volatile.Write(ref _store, v));
            }

            var n = N;
            for (var i = 0; i < n; i++)
            {
                subj.OnNext(i);
            }

            return consumers;
        }
    }
}
