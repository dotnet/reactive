// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class RepeatBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;

        public int _store;

        [Benchmark]
        public void Repeat_Infinite()
        {
            Observable.Repeat(1).Take(N).Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Repeat_Finite()
        {
            Observable.Repeat(1, N).Subscribe(v => Volatile.Write(ref _store, v));
        }
    }
}
