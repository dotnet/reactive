// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Hot
{
    /// <summary>
    /// S2: raw push into the replay-subject variants that <see cref="SubjectBenchmarks"/> does not cover, with
    /// <c>M</c> subscribers (0, 1, 5): the sequential single-element replay (<c>bufferSize = 1</c>, its own
    /// implementation), the concurrent bounded replay, and the sequential bounded and time-windowed replays with an
    /// explicit immediate scheduler. The window is one day, so every element stays inside it.
    /// </summary>
    /// <remarks>
    /// A replay subject with a scheduler delivers through scheduled observers; only the immediate scheduler is used,
    /// so delivery stays on the pushing thread.
    /// </remarks>
    [BenchmarkCategory("Hot")]
    public class ReplaySubjectVariantBenchmarks
    {
        private static readonly TimeSpan Window = TimeSpan.FromDays(1);

        [Params(1_000, 10_000, 100_000)]
        public int N;

        [Params(0, 1, 5)]
        public int M;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask SequentialReplayOne_Push() => PushAsync(new SequentialReplayAsyncSubject<int>(1));

        [Benchmark]
        public ValueTask ConcurrentReplayBounded_Push() => PushAsync(new ConcurrentReplayAsyncSubject<int>(16));

        [Benchmark]
        public ValueTask SequentialReplayBounded_Scheduler_Push() =>
            PushAsync(new SequentialReplayAsyncSubject<int>(16, ImmediateAsyncScheduler.Instance));

        [Benchmark]
        public ValueTask SequentialReplayWindow_Scheduler_Push() =>
            PushAsync(new SequentialReplayAsyncSubject<int>(Window, ImmediateAsyncScheduler.Instance));

        private ValueTask PushAsync(IAsyncSubject<int> subject) => Pump.PushToSubscribersAsync(subject, M, N, _consumer);
    }
}
