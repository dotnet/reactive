// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class BufferCountBenchmark
    {
        private IList<int> _store;

        [Benchmark]
        public void Exact()
        {
            Observable.Range(1, 1000)
                .Buffer(1)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Skip()
        {
            Observable.Range(1, 1000)
                .Buffer(1, 2)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Overlap()
        {
            Observable.Range(1, 1000)
                .Buffer(2, 1)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }
    }
}
