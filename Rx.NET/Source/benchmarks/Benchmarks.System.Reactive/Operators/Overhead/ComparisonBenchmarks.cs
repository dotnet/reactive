// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Overhead
{
    /// <summary>
    /// The historic broad comparison grid (predates the per-operator suite), retained for baseline
    /// continuity: a raw <c>for</c> loop and LINQ-to-Objects against a wide spread of Rx pipelines in
    /// one table. For focused per-operator measurements prefer the dedicated classes; the cross-map
    /// variants here (<c>ConcatCrossMap</c>/<c>SelectManyCrossMap</c>/<c>MergeCrossMap</c>) pin total
    /// work at ~1M elements, so for those N reads as fan-out width, not element count.
    /// </summary>
    [BenchmarkCategory("Overhead")]
    public class ComparisonBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public void ForLoopBaseLine()
        {
            var n = N;
            for (var i = 0; i < n; i++)
            {
                Consumer.Consume(i);
            }
        }

        [Benchmark]
        public void EnumerableBaseLine()
        {
            foreach (var v in Enumerable.Range(1, N))
            {
                Consumer.Consume(v);
            }
        }

        [Benchmark]
        public void Return() => Observable.Return(1).SubscribeConsume(Consumer);

        [Benchmark]
        public void Range() => Observable.Range(1, N).SubscribeConsume(Consumer);

        [Benchmark]
        public void Select() =>
            Observable.Range(1, N)
                .Select(static v => v + 1)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void SelectSelect() =>
            Observable.Range(1, N)
                .Select(static v => v + 1)
                .Select(static v => v + 1)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Where() =>
            Observable.Range(1, 2 * N)
                .Where(static v => (v & 1) != 0)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void WhereWhere() =>
            Observable.Range(1, 4 * N)
                .Where(static v => (v & 1) != 0)
                .Where(static v => (v & 2) != 0)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Take() =>
            Observable.Range(1, 2 * N)
                .Take(N)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Skip() =>
            Observable.Range(1, 2 * N)
                .Skip(N)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void TakeUntil() =>
            Observable.Range(1, N)
                .TakeUntil(Observable.Never<int>())
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void ToObservable() =>
            Enumerable.Range(1, N)
                .ToObservable()
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Concat()
        {
            var m = N - N / 2;

            Observable.Concat(
                    Observable.Range(1, N),
                    Observable.Range(1, m))
                .SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void ConcatCrossMap()
        {
            var m = 1000 * 1000 / N;

            Observable.Concat(Observable.Range(1, N).Select(v => Observable.Range(v, m)))
                .SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void SelectManyCrossMap()
        {
            var m = 1000 * 1000 / N;

            Observable.Range(1, N).SelectMany(v => Observable.Range(v, m))
                .SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void MergeCrossMap()
        {
            var m = 1000 * 1000 / N;

            Observable.Merge(Observable.Range(1, N).Select(v => Observable.Range(v, m)))
                .SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void AsyncSubjectPush()
        {
            var subj = new AsyncSubject<int>();
            subj.SubscribeConsume(Consumer);

            var n = N;
            for (var i = 0; i < n; i++)
            {
                subj.OnNext(i);
            }

            subj.OnCompleted();
        }

        [Benchmark]
        public void SubjectPush()
        {
            var subj = new Subject<int>();
            subj.SubscribeConsume(Consumer);

            var n = N;
            for (var i = 0; i < n; i++)
            {
                subj.OnNext(i);
            }

            subj.OnCompleted();
        }

        [Benchmark]
        public void AmbTwo() =>
            Observable.Never<int>().Amb(Observable.Range(1, N))
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void AmbThree() =>
            Observable.Amb(Observable.Never<int>(), Observable.Never<int>(), Observable.Range(1, N))
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Timeout() =>
            Observable.Range(1, N)
                .Timeout(TimeSpan.FromHours(1))
                .SubscribeConsume(Consumer);

#pragma warning disable CS0618 // Type or member is obsolete
        [Benchmark]
        public void First() => Consumer.Consume(Observable.Range(1, N).First());

        [Benchmark]
        public void Last() => Consumer.Consume(Observable.Range(1, N).Last());
#pragma warning restore CS0618 // Type or member is obsolete

        [Benchmark]
        public void Buffer_Exact() =>
            Observable.Range(1, 1000)
                .Buffer(1)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Skip() =>
            Observable.Range(1, 1000)
                .Buffer(1, 2)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Buffer_Overlap() =>
            Observable.Range(1, 1000)
                .Buffer(2, 1)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void CurrentThreadSchedulerRepeated()
        {
            var n = N;
            var scheduler = CurrentThreadScheduler.Instance;
            for (var i = 0; i < n; i++)
            {
                scheduler.Schedule(i, (_, v) =>
                {
                    Consumer.Consume(v);
                    return Disposable.Empty;
                });
            }
        }

        [Benchmark]
        public void TakeLast() =>
            Observable.Range(1, 2 * N).TakeLast(N)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Repeat() =>
            Observable.Repeat(1, N)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void ToList() =>
            Observable.Repeat(1, N).ToList()
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Generate() =>
            Observable.Generate(0, s => s < N, static s => s + 1, static s => s)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Collect()
        {
            foreach (var v in Observable.Range(1, N).Collect(static () => new List<int>(), static (a, b) => { a.Add(b); return a; }))
            {
                Consumer.Consume(v);
            }
        }
    }
}
