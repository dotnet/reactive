// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

#pragma warning disable CS0618 // The blocking operators are obsolete (callers are steered to async); benchmarked intentionally.

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// The remaining blocking gate/pull operators: <c>Single</c> (predicate matching exactly one, so it scans the
    /// whole stream), <c>FirstOrDefault</c>/<c>LastOrDefault</c>, and the pull <c>GetEnumerator</c>. (<c>Latest</c>/
    /// <c>MostRecent</c>/<c>Next</c>/<c>Chunkify</c> are designed for hot sources and are exercised via the workload
    /// harness rather than here.)
    /// </summary>
    [BenchmarkCategory("Blocking")]
    public class BlockingPullBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public int Single() => Observable.Range(1, N).Single(static v => v == 1);

        [Benchmark]
        public int SingleOrDefault() => Observable.Range(1, N).SingleOrDefault(static v => v == 1);

        [Benchmark]
        public int FirstOrDefault() => Observable.Range(1, N).FirstOrDefault();

        [Benchmark]
        public int LastOrDefault() => Observable.Range(1, N).LastOrDefault();

        [Benchmark]
        public void GetEnumerator()
        {
            using var enumerator = Observable.Range(1, N).GetEnumerator();
            while (enumerator.MoveNext())
            {
                Consumer.Consume(enumerator.Current);
            }
        }
    }
}
