// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Lifecycle
{
    /// <summary>
    /// Lifecycle: tears down a <b>still-live</b> subscription mid-stream — the hot <c>Subject</c> sources never
    /// complete, so the <c>using</c> <c>Dispose()</c> at method end lands on an active subscription. This exercises
    /// the unsubscribe/cleanup path of operators with non-trivial disposal, which every cold-source benchmark
    /// misses (their sources complete synchronously before <c>Dispose</c> is ever reached).
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class DisposeBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Merge_Dispose()
        {
            var a = new Subject<int>();
            var b = new Subject<int>();
            using var subscription = Observable.Merge(a, b).SubscribeConsume(_consumer);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                a.OnNext(i);
                b.OnNext(i);
            }
        }

        [Benchmark]
        public void CombineLatest_Dispose()
        {
            var a = new Subject<int>();
            var b = new Subject<int>();
            using var subscription = Observable.CombineLatest(a, b, static (x, y) => x + y).SubscribeConsume(_consumer);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                a.OnNext(i);
                b.OnNext(i);
            }
        }

        [Benchmark]
        public void Switch_Dispose()
        {
            var outer = new Subject<IObservable<int>>();
            var inner = new Subject<int>();
            using var subscription = outer.Switch().SubscribeConsume(_consumer);
            outer.OnNext(inner);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                inner.OnNext(i);
            }
        }

        [Benchmark]
        public void GroupBy_Dispose()
        {
            var source = new Subject<int>();
            using var subscription = source.GroupBy(static v => v % 8).SelectMany(static g => g).SubscribeConsume(_consumer);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                source.OnNext(i);
            }
        }

        [Benchmark]
        public void Window_Dispose()
        {
            var source = new Subject<int>();
            using var subscription = source.Window(16).SelectMany(static w => w).SubscribeConsume(_consumer);

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                source.OnNext(i);
            }
        }

        [Benchmark]
        public void Publish_Dispose()
        {
            var source = new Subject<int>();
            var published = source.Publish();
            using var subscription = published.SubscribeConsume(_consumer);
            using var connection = published.Connect();

            var half = N / 2;
            for (var i = 0; i < half; i++)
            {
                source.OnNext(i);
            }
        }
    }
}
