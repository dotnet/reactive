// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Hot
{
    /// <summary>
    /// Hot-push (S2) variants of the core transformation operators — driven by pumping a <see cref="Subject{T}"/>
    /// rather than a cold <c>Observable.Range</c>. This exercises the real push path (per-element <c>OnNext</c>
    /// dispatch, re-entrancy) with none of <c>Range</c>'s synchronous fast-path.
    /// </summary>
    [BenchmarkCategory("Hot")]
    public class HotSourceBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Select_Hot()
        {
            var source = new Subject<int>();
            source.Select(static v => v + 1).SubscribeConsume(_consumer);
            Pump(source);
        }

        [Benchmark]
        public void Where_Hot()
        {
            var source = new Subject<int>();
            source.Where(static v => (v & 1) == 0).SubscribeConsume(_consumer);
            Pump(source);
        }

        [Benchmark]
        public void SelectMany_Hot()
        {
            var source = new Subject<int>();
            source.SelectMany(static v => Observable.Return(v)).SubscribeConsume(_consumer);
            Pump(source);
        }

        [Benchmark]
        public void Scan_Hot()
        {
            var source = new Subject<int>();
            source.Scan(0L, static (acc, v) => acc + v).SubscribeConsume(_consumer);
            Pump(source);
        }

        [Benchmark]
        public void GroupBy_Hot()
        {
            var source = new Subject<int>();
            source.GroupBy(static v => v % 8).SelectMany(static g => g).SubscribeConsume(_consumer);
            Pump(source);
        }

        [Benchmark]
        public void Merge_Hot()
        {
            var a = new Subject<int>();
            var b = new Subject<int>();
            Observable.Merge(a, b).SubscribeConsume(_consumer);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                a.OnNext(i);
                b.OnNext(i);
            }

            a.OnCompleted();
            b.OnCompleted();
        }

        private void Pump(Subject<int> source)
        {
            var n = N;
            for (var i = 0; i < n; i++)
            {
                source.OnNext(i);
            }

            source.OnCompleted();
        }
    }
}
