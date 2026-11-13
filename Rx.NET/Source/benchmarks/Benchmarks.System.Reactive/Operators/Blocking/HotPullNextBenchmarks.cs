// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// <c>Next</c>: each <c>MoveNext</c> blocks until the <i>next</i> value arrives after the pull began, so it can
    /// only be exercised against a concurrent producer. A background task pushes into a <see cref="Subject{T}"/>
    /// continuously until told to stop; the benchmark thread performs exactly N <c>MoveNext</c>/<c>Current</c>
    /// pulls, then stops and joins the producer. The measured quantity is therefore "N blocking pulls against a
    /// free-running producer" — well-defined regardless of how the two threads interleave (unlike draining a
    /// finite producer, where the number of values <c>Next</c> observes varies from 0 to N).
    /// </summary>
    /// <remarks>
    /// Each pull still depends on a cross-thread wake-up, so per-invocation timing is scheduler-sensitive. This class
    /// therefore uses a fixed, multi-launch Monitoring job (3 launches × 20 iterations) rather than the auto-tuned
    /// default, so results are repeatable and launch-to-launch variance is visible.
    /// </remarks>
    [SimpleJob(RunStrategy.Monitoring, launchCount: 3, warmupCount: 5, iterationCount: 20)]
    [BenchmarkCategory("Blocking")]
    public class HotPullNextBenchmarks
    {
        [Params(1_000, 10_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void Next()
        {
            var subject = new Subject<int>();
            var n = N;
            var stop = 0;

            // Subscribe before the producer starts, so every pull is satisfied by the free-running producer.
            var enumerator = subject.Next().GetEnumerator();
            var producer = Task.Run(() =>
            {
                try
                {
                    var i = 0;
                    while (Volatile.Read(ref stop) == 0)
                    {
                        subject.OnNext(i++);
                    }
                }
                finally
                {
                    // On every exit path (including a fault) release any pull still blocked in MoveNext.
                    subject.OnCompleted();
                }
            });

            try
            {
                for (var i = 0; i < n; i++)
                {
                    if (!enumerator.MoveNext())
                    {
                        // Only reachable if the producer terminated early; producer.Wait() below surfaces its fault.
                        throw new InvalidOperationException("Producer stopped before N pulls completed.");
                    }

                    _consumer.Consume(enumerator.Current);
                }
            }
            finally
            {
                Volatile.Write(ref stop, 1);    // never leave the producer spinning, whatever happened above
                producer.Wait();
                enumerator.Dispose();
            }
        }
    }
}
