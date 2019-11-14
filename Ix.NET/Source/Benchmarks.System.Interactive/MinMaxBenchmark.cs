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
    public class MinMaxBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;
        private int _store;
        private IList<int>? _listStore;

        private readonly IComparer<int> _comparer = Comparer<int>.Default;

        [Benchmark]
        public void Min()
        {
            Volatile.Write(ref _store, Enumerable.Range(1, N).Min(_comparer));
        }

        [Benchmark]
        public void MinBy()
        {
            Volatile.Write(ref _listStore, Enumerable.Range(1, N).MinBy(v => -v, _comparer));
        }

        [Benchmark]
        public void Max()
        {
            Volatile.Write(ref _store, Enumerable.Range(1, N).Max(_comparer));
        }

        [Benchmark]
        public void MaxBy()
        {
            Volatile.Write(ref _listStore, Enumerable.Range(1, N).MaxBy(v => -v, _comparer));
        }
    }
}
