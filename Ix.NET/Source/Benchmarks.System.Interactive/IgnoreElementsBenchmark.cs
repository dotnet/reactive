// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Interactive
{
    [MemoryDiagnoser]
    public class IgnoreElementsBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;

        private int _store;

        private int[]? _array;
        private List<int>? _list;

        [Benchmark]
        public void Ignore()
        {
            Enumerable.Range(1, N)
                .IgnoreElements()
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void IgnoreList()
        {
            _list!
                .IgnoreElements()
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void IgnoreArray()
        {
            _array!
                .IgnoreElements()
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [GlobalSetup]
        public void Setup()
        {
            _array = new int[N];
            _list = new List<int>(N);
            for (var i = 0; i < N; i++)
            {
                _array[i] = i;
                _list.Add(i);
            }
        }
    }
}
