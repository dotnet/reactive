// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Operators.Lifecycle
{
    /// <summary>
    /// Construction and disposal cost of <see cref="StableCompositeDisposable"/> over array and list inputs.
    /// N is the number of inner disposables (small values dominate real operator usage; 100 probes the
    /// scaling of the copy). Benchmarks return the created object so it is consumed, not eliminated.
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class StableCompositeDisposableBenchmarks
    {
        [Params(3, 4, 5, 6, 7, 8, 9, 10, 100)]
        public int N;

        private IDisposable[] _array;

        private List<IDisposable> _list;

        [GlobalSetup]
        public void Setup()
        {
            _array = new IDisposable[N];
            for (var i = 0; i < N; i++)
            {
                _array[i] = Disposable.Empty;
            }

            _list = new List<IDisposable>(_array);
        }

        [Benchmark]
        public object Create_Array()
        {
            return StableCompositeDisposable.Create(_array);
        }

        [Benchmark]
        public object Create_Trusted_Array()
        {
            return CreateTrusted(_array);
        }

        [Benchmark]
        public object Create_List()
        {
            return StableCompositeDisposable.Create(_list);
        }

        [Benchmark]
        public object Dispose_Array()
        {
            var scd = StableCompositeDisposable.Create(_array);
            scd.Dispose();
            return scd;
        }

        [Benchmark]
        public object Dispose_List()
        {
            var scd = StableCompositeDisposable.Create(_list);
            scd.Dispose();
            return scd;
        }

        [Benchmark]
        public object Dispose_Trusted_Array()
        {
            var scd = CreateTrusted(_array);
            scd.Dispose();
            return scd;
        }

        // The StableCompositeDisposable.CreateTrusted is inaccessible and
        // adding the InternalsVisibleTo attribute doesn't work (needs signed assemblies?)
        internal static ICancelable CreateTrusted(params IDisposable[] disposables)
        {
            return new NAryTrustedArray(disposables);
        }

        private sealed class NAryTrustedArray : StableCompositeDisposable
        {
            private IDisposable[] _disposables;

            public NAryTrustedArray(IDisposable[] disposables)
            {
                Volatile.Write(ref _disposables, disposables);
            }

            public override bool IsDisposed => Volatile.Read(ref _disposables) == null;

            public override void Dispose()
            {
                var old = Interlocked.Exchange(ref _disposables, null);
                if (old != null)
                {
                    foreach (var d in old)
                    {
                        d.Dispose();
                    }
                }
            }
        }
    }
}
