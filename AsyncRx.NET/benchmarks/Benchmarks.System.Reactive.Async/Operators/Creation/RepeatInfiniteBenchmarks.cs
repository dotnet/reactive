// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// The infinite <c>Repeat</c> forms, each cut off after N elements by <c>Take(N)</c>:
    /// <c>Repeat(value, scheduler)</c> on the task pool, and <c>source.Repeat()</c> resubscribing a hot source
    /// that the benchmark pushes one element into and then completes, N times (per-resubscription cost).
    /// </summary>
    /// <remarks>
    /// <para>
    /// <c>Repeat(value, scheduler)</c> cannot run on the immediate scheduler: its push loop runs inside
    /// <c>SubscribeAsync</c>, so the subscription is never returned and <c>Take</c> cannot cancel it. The row
    /// therefore uses <see cref="TaskPoolAsyncScheduler.Default"/>, and every element pays a
    /// <c>RendezVous</c> on the task pool. The sequence completes on a pool thread. After <c>Take</c> completes,
    /// cancellation throws one <c>OperationCanceledException</c> inside the scheduled loop, which the scheduler
    /// swallows; that fixed cost dominates the N = 1 row.
    /// </para>
    /// <para>
    /// <c>source.Repeat()</c> over a cold source that completes synchronously is also an endless loop, for the
    /// same reason. The row drives a <see cref="PushSource{T}"/> instead, and stops pushing as soon as the sink
    /// has terminated.
    /// </para>
    /// </remarks>
    [BenchmarkCategory("Creation")]
    public class RepeatInfiniteBenchmarks : BenchmarkBase
    {
        [Params(1, 100, 10_000)]
        public int N;

        [Benchmark]
        public ValueTask Repeat_Value_Infinite_TaskPool() =>
            AsyncObservable.Repeat(42, TaskPoolAsyncScheduler.Default).Take(N).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public async ValueTask Repeat_Source_Infinite()
        {
            var source = new PushSource<int>();
            var sink = Sinks.For<int>(Consumer);
            await source.Repeat().Take(N).SubscribeAsync(sink).ConfigureAwait(false);

            for (var i = 0; i < N && !sink.Completion.IsSet; i++)
            {
                await source.OnNextAsync(i).ConfigureAwait(false);
                await source.OnCompletedAsync().ConfigureAwait(false);
            }

            await sink.Completion.WaitAsync().ConfigureAwait(false);
        }
    }
}
