// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// S1 (variant sweep): <c>Defer</c> with the synchronous, <c>ValueTask</c> and <c>CancellationToken</c>
    /// factories, each returning a source of N built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class DeferBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<int> _deferred = default!;
        private IAsyncObservable<int> _deferredAsync = default!;
        private IAsyncObservable<int> _deferredToken = default!;

        [GlobalSetup]
        public void Setup()
        {
            var inner = BenchmarkSources.Range(N);
            _deferred = AsyncObservable.Defer(() => inner);
            _deferredAsync = AsyncObservable.Defer(() => new ValueTask<IAsyncObservable<int>>(inner));
            _deferredToken = AsyncObservable.Defer(_ => new ValueTask<IAsyncObservable<int>>(inner));
        }

        [Benchmark]
        public ValueTask Defer() => _deferred.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Defer_Async() => _deferredAsync.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Defer_CancellationToken() => _deferredToken.SubscribeConsumeAsync(Consumer);
    }
}
