// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Overhead
{
    /// <summary>
    /// S1 (variant sweep): the delegate-based <c>SubscribeAsync</c> extensions from <c>AsyncObservableExtensions</c>,
    /// which wrap the callbacks in an anonymous observer: an <see cref="Action{T}"/> <c>onNext</c>, a
    /// <c>ValueTask</c>-returning <c>onNext</c>, and the three-callback <c>onNext</c> / <c>onError</c> /
    /// <c>onCompleted</c> form. The cold source of N completes inside <c>SubscribeAsync</c>; each row awaits the
    /// subscription and then disposes it. The delegates are built once per instance.
    /// </summary>
    [BenchmarkCategory("Overhead")]
    public class SubscribeDelegateBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly Action<int> _onNext;
        private readonly Func<int, ValueTask> _onNextAsync;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public SubscribeDelegateBenchmarks()
        {
            var consumer = Consumer;
            _onNext = v => consumer.Consume(v);
            _onNextAsync = v =>
            {
                consumer.Consume(v);
                return default;
            };
            _onError = e => consumer.Consume(e);
            _onCompleted = static () => { };
        }

        [Benchmark]
        public async ValueTask SubscribeAsync_Action()
        {
            var subscription = await BenchmarkSources.Range(N).SubscribeAsync(_onNext).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask SubscribeAsync_Func()
        {
            var subscription = await BenchmarkSources.Range(N).SubscribeAsync(_onNextAsync).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }

        [Benchmark]
        public async ValueTask SubscribeAsync_Actions()
        {
            var subscription = await BenchmarkSources.Range(N).SubscribeAsync(_onNext, _onError, _onCompleted).ConfigureAwait(false);
            await subscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}
