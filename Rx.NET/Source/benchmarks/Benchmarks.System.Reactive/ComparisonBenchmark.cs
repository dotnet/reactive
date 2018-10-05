// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive
{
    [MemoryDiagnoser]
    public class ComparisonBenchmark
    {
        [Params(1, 10, 100, 1000, 10000, 100000, 1000000)]
        public int N;
        private int _store;

        [Benchmark]
        public void ForLoopBaseLine()
        {
            var n = N;
            for (var i = 0; i < N; i++)
            {
                Volatile.Write(ref _store, i);
            }
        }

        [Benchmark]
        public void EnumerableBaseLine()
        {
            foreach (var v in Enumerable.Range(1, N))
            {
                Volatile.Write(ref _store, v);
            }
        }

        [Benchmark]
        public void Return()
        {
            Observable.Return(1).Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Range()
        {
            Observable.Range(1, N).Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Select()
        {
            Observable.Range(1, N)
                .Select(v => v + 1)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void SelectSelect()
        {
            Observable.Range(1, N)
                .Select(v => v + 1)
                .Select(v => v + 1)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Where()
        {
            Observable.Range(1, 2 * N)
                .Where(v => (v & 1) != 0)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void WhereWhere()
        {
            Observable.Range(1, 4 * N)
                .Where(v => (v & 1) != 0)
                .Where(v => (v & 2) != 0)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Take()
        {
            Observable.Range(1, 2 * N)
                .Take(N)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Skip()
        {
            Observable.Range(1, 2 * N)
                .Skip(N)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void TakeUntil()
        {
            Observable.Range(1, N)
                .TakeUntil(Observable.Never<int>())
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void ToObservable()
        {
            Enumerable.Range(1, N)
                .ToObservable()
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Concat()
        {
            var M = N - N / 2;

            Observable.Concat(
                    Observable.Range(1, N),
                    Observable.Range(1, M)
                )
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void ConcatCrossMap()
        {
            var M = 1000 * 1000 / N;

            Observable.Concat(Observable.Range(1, N).Select(v => Observable.Range(v, M)))
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void SelectManyCrossMap()
        {
            var M = 1000 * 1000 / N;

            Observable.Range(1, N).SelectMany(v => Observable.Range(v, M))
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void MergeCrossMap()
        {
            var M = 1000 * 1000 / N;

            Observable.Merge(Observable.Range(1, N)
                    .Select(v => Observable.Range(v, M))
                )
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void AsyncSubjectPush()
        {
            var subj = new AsyncSubject<int>();
            subj.Subscribe(v => Volatile.Write(ref _store, v));

            var n = N;
            for (var i = 0; i < N; i++)
            {
                subj.OnNext(i);
            }
            subj.OnCompleted();
        }

        [Benchmark]
        public void SubjectPush()
        {
            var subj = new Subject<int>();
            subj.Subscribe(v => Volatile.Write(ref _store, v));

            var n = N;
            for (var i = 0; i < N; i++)
            {
                subj.OnNext(i);
            }
            subj.OnCompleted();
        }

        [Benchmark]
        public void AmbTwo()
        {
            Observable.Never<int>().Amb(Observable.Range(1, N))
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void AmbThree()
        {
            Observable.Amb(Observable.Never<int>(), Observable.Never<int>(), Observable.Range(1, N))
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Timeout()
        {
            Observable.Range(1, N)
                .Timeout(TimeSpan.FromHours(1))
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

#pragma warning disable CS0618 // Type or member is obsolete
        [Benchmark]
        public void First()
        {
            Volatile.Write(ref _store, Observable.Range(1, N)
                .First());
        }

        [Benchmark]
        public void Last()
        {
            Volatile.Write(ref _store, Observable.Range(1, N)
                .Last());
        }
#pragma warning restore CS0618 // Type or member is obsolete

        private IList<int> _bufferStore;

        [Benchmark]
        public void Buffer_Exact()
        {
            Observable.Range(1, 1000)
                .Buffer(1)
                .Subscribe(v => Volatile.Write(ref _bufferStore, v));
        }

        [Benchmark]
        public void Buffer_Skip()
        {
            Observable.Range(1, 1000)
                .Buffer(1, 2)
                .Subscribe(v => Volatile.Write(ref _bufferStore, v));
        }

        [Benchmark]
        public void Buffer_Overlap()
        {
            Observable.Range(1, 1000)
                .Buffer(2, 1)
                .Subscribe(v => Volatile.Write(ref _bufferStore, v));
        }

        [Benchmark]
        public void CurrentThreadSchedulerRepeated()
        {
            var n = N;
            var scheduler = CurrentThreadScheduler.Instance;
            for (var i = 0; i < n; i++)
            {
                scheduler.Schedule(i, (_, v) =>
                {
                    Volatile.Write(ref _store, v);
                    return Disposable.Empty;
                });
            }
        }

        [Benchmark]
        public void TakeLast()
        {
            Observable.Range(1, 2 * N).TakeLast(N)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Repeat()
        {
            Observable.Repeat(1, N)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void ToList()
        {
            Observable.Repeat(1, N).ToList()
                .Subscribe(v => Volatile.Write(ref _bufferStore, v));
        }

        [Benchmark]
        public void Generate()
        {
            Observable.Generate(0, s => s < N, s => s + 1, s => s)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        [Benchmark]
        public void Collect()
        {
            foreach (var v in Observable.Range(1, N).Collect(() => new List<int>(), (a, b) => { a.Add(b); return a; }))
            {
                Volatile.Write(ref _bufferStore, v);
            }
        }
    }
}
