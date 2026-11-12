// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Hot
{
    /// <summary>
    /// S2: raw subject push across the AsyncRx subject variants, with <c>M</c> subscribers (0 = dispatch to
    /// nobody; 5 = multi-subscriber dispatch). Each subject comes in a Sequential (single-producer) and a
    /// Concurrent flavour. Complements <see cref="HotSourceBenchmarks"/>, which pumps operators through a
    /// benchmark-owned single-observer source.
    /// </summary>
    /// <remarks>
    /// The simple subjects copy their observer list per <c>OnNextAsync</c>. The Behavior and Replay subjects take
    /// an <c>AsyncGate</c> per call, so their rows include a thread-pool hop per element.
    /// </remarks>
    [BenchmarkCategory("Hot")]
    public class SubjectBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        [Params(0, 1, 5)]
        public int M;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public ValueTask SequentialSimple_Push() => PushAsync(new SequentialSimpleAsyncSubject<int>());

        [Benchmark]
        public ValueTask ConcurrentSimple_Push() => PushAsync(new ConcurrentSimpleAsyncSubject<int>());

        [Benchmark]
        public ValueTask SequentialAsync_Push() => PushAsync(new SequentialAsyncAsyncSubject<int>());

        [Benchmark]
        public ValueTask ConcurrentAsync_Push() => PushAsync(new ConcurrentAsyncAsyncSubject<int>());

        [Benchmark]
        public ValueTask SequentialBehavior_Push() => PushAsync(new SequentialBehaviorAsyncSubject<int>(-1));

        [Benchmark]
        public ValueTask ConcurrentBehavior_Push() => PushAsync(new ConcurrentBehaviorAsyncSubject<int>(-1));

        [Benchmark]
        public ValueTask SequentialReplay_Push() => PushAsync(new SequentialReplayAsyncSubject<int>());

        [Benchmark]
        public ValueTask SequentialReplayBounded_Push() => PushAsync(new SequentialReplayAsyncSubject<int>(16));

        [Benchmark]
        public ValueTask ConcurrentReplay_Push() => PushAsync(new ConcurrentReplayAsyncSubject<int>());

        private async ValueTask PushAsync(IAsyncSubject<int> subject)
        {
            var m = M;
            var subscriptions = new IAsyncDisposable[m];
            for (var i = 0; i < m; i++)
            {
                subscriptions[i] = await subject.SubscribeNoWaitAsync(_consumer).ConfigureAwait(false);
            }

            await subject.PushAndCompleteAsync(N).ConfigureAwait(false);

            for (var i = 0; i < m; i++)
            {
                await subscriptions[i].DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
