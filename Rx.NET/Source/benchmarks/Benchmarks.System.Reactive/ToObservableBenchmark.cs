// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class ToObservableBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;

        private int _store;

        [Benchmark]
        public void Exact()
        {
            Enumerable.Range(1, N)
                .ToObservable()
                .Subscribe(v => Volatile.Write(ref _store, v));
        }
    }
}
