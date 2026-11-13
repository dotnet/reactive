// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
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
    /// What remains of the historic broad comparison grid (predates the per-operator suite): a raw <c>for</c> loop
    /// (the class baseline) and LINQ-to-Objects alongside the Rx pipelines that have no identical counterpart in a
    /// dedicated class — chained operators (<c>SelectSelect</c>, <c>WhereWhere</c>), the over-sized-source variants
    /// (<c>Where</c> over 2N, <c>TakeLast</c> over 2N, <c>Concat</c> of N + N/2), the <c>SelectMany</c> cross-map,
    /// subject push, <c>Amb</c> with the winner second, <c>Timeout</c>, <c>TakeUntil(Never)</c>,
    /// <c>CurrentThreadScheduler</c> recursion and <c>Repeat().ToList()</c>. They are kept for baseline continuity and
    /// so the Ratio column shows Rx overhead against the raw loop in one table. Nothing here is the sole coverage of
    /// an operator: pipelines duplicated exactly by a dedicated class were removed, and the ones that were unique
    /// were moved out (nested <c>Concat</c>/<c>Merge</c> to <c>NestedConcatMergeBenchmarks</c>, n-ary <c>Amb</c> to
    /// <c>AmbBenchmarks</c>, <c>Collect</c> to <c>CollectBenchmarks</c>). <c>SelectManyCrossMap</c> pins total work at
    /// ~1M elements, so for it N reads as fan-out width, not element count.
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
        public void TakeUntil() =>
            Observable.Range(1, N)
                .TakeUntil(Observable.Never<int>())
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
        public void SelectManyCrossMap()
        {
            var m = 1000 * 1000 / N;

            Observable.Range(1, N).SelectMany(v => Observable.Range(v, m))
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
        public void Timeout() =>
            Observable.Range(1, N)
                .Timeout(TimeSpan.FromHours(1))
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
        public void ToList() =>
            Observable.Repeat(1, N).ToList()
                .SubscribeConsume(Consumer);
    }
}
