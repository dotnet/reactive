// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Subjects;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// A late subscriber to replay subjects that already hold N elements and have completed: each invocation
    /// subscribes and receives the full replay plus completion. The subjects are filled once in
    /// <c>[GlobalSetup]</c>; replay does not consume them.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class LateSubscriberBenchmarks : GatedOperatorBenchmarkBase
    {
        private SequentialReplayAsyncSubject<int> _sequential = default!;
        private ConcurrentReplayAsyncSubject<int> _concurrent = default!;
        private SequentialReplayAsyncSubject<int> _bounded = default!;

        [GlobalSetup]
        public void Setup()
        {
            _sequential = new SequentialReplayAsyncSubject<int>();
            _concurrent = new ConcurrentReplayAsyncSubject<int>();
            _bounded = new SequentialReplayAsyncSubject<int>(16);
            Fill(_sequential).AsTask().GetAwaiter().GetResult();
            Fill(_concurrent).AsTask().GetAwaiter().GetResult();
            Fill(_bounded).AsTask().GetAwaiter().GetResult();
        }

        [Benchmark]
        public ValueTask SequentialReplay_LateSubscribe() => _sequential.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ConcurrentReplay_LateSubscribe() => _concurrent.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SequentialReplayBounded_LateSubscribe() => _bounded.SubscribeConsumeAsync(Consumer);

        private ValueTask Fill(IAsyncSubject<int> subject) => subject.PushAndCompleteAsync(N);
    }
}
