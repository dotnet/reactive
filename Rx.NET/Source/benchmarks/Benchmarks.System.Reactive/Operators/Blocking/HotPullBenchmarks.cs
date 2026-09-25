// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// The hot-source blocking-pull operators, driven by a <see cref="Subject{T}"/> pumped alongside the pull.
    /// <c>Latest</c>/<c>MostRecent</c>/<c>Chunkify</c> pull deterministically on the producing thread; <c>Next</c>
    /// requires a concurrent producer (it only captures values that arrive while a pull is blocked). These are
    /// inherently timing-dependent, so expect higher variance than the cold-source benchmarks. N is capped.
    /// </summary>
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 3, iterationCount: 10)]   // timing-dependent (Next races a producer) → repeatable job
    [BenchmarkCategory("Blocking")]
    public class HotPullBenchmarks
    {
        [Params(1_000, 10_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Latest()
        {
            var subject = new Subject<int>();
            using var enumerator = subject.Latest().GetEnumerator();
            for (var i = 0; i < N; i++)
            {
                subject.OnNext(i);
                enumerator.MoveNext();
                _consumer.Consume(enumerator.Current);
            }

            subject.OnCompleted();
        }

        [Benchmark]
        public void MostRecent()
        {
            var subject = new Subject<int>();
            using var enumerator = subject.MostRecent(0).GetEnumerator();
            for (var i = 0; i < N; i++)
            {
                subject.OnNext(i);
                enumerator.MoveNext();
                _consumer.Consume(enumerator.Current);
            }

            subject.OnCompleted();
        }

        [Benchmark]
        public void Chunkify()
        {
            var subject = new Subject<int>();
            using var enumerator = subject.Chunkify().GetEnumerator();
            for (var i = 0; i < N; i++)
            {
                subject.OnNext(i);
                if ((i & 15) == 0)
                {
                    enumerator.MoveNext();
                    _consumer.Consume(enumerator.Current);
                }
            }

            subject.OnCompleted();
        }

        [Benchmark]
        public void Next()
        {
            var subject = new Subject<int>();
            var n = N;
            var pump = Task.Run(() =>
            {
                for (var i = 0; i < n; i++)
                {
                    subject.OnNext(i);
                }

                subject.OnCompleted();
            });

            foreach (var value in subject.Next())
            {
                _consumer.Consume(value);
            }

            pump.Wait();
        }
    }
}
