// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Reactive.Subjects;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// The hot-source blocking-pull operators that can be driven deterministically: a <see cref="Subject{T}"/> is
    /// pumped and pulled alternately on the benchmark thread, so <c>Latest</c>/<c>MostRecent</c>/<c>Chunkify</c>
    /// never actually wait and the default job applies. N is capped. (<c>Next</c> only captures values that arrive
    /// while a pull is blocked, so it needs a concurrent producer; it lives in <see cref="HotPullNextBenchmarks"/>.)
    /// </summary>
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
    }
}
