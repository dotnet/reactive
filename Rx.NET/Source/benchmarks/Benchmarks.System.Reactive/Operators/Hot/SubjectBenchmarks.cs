// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Hot
{
    /// <summary>
    /// S2: raw subject push across the subject variants, with <c>M</c> concurrent subscribers
    /// (0 = dispatch to nobody, the cheapest path; 5 = the multi-subscriber array dispatch).
    /// Complements <see cref="HotSourceBenchmarks"/>, which pumps operators through a single subject.
    /// </summary>
    [BenchmarkCategory("Hot")]
    public class SubjectBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        [Params(0, 1, 5)]
        public int M;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Subject_Push() => Push(new Subject<int>());

        [Benchmark]
        public void AsyncSubject_Push() => Push(new AsyncSubject<int>());

        [Benchmark]
        public void BehaviorSubject_Push() => Push(new BehaviorSubject<int>(-1));

        [Benchmark]
        public void ReplaySubject_Push() => Push(new ReplaySubject<int>());

        private void Push<TSubject>(TSubject subject)
            where TSubject : ISubject<int>
        {
            var m = M;
            var subscriptions = new IDisposable[m];
            for (var i = 0; i < m; i++)
            {
                subscriptions[i] = subject.SubscribeConsume(_consumer);
            }

            var n = N;
            for (var i = 0; i < n; i++)
            {
                subject.OnNext(i);
            }

            subject.OnCompleted();

            for (var i = 0; i < m; i++)
            {
                subscriptions[i].Dispose();
            }
        }
    }
}
