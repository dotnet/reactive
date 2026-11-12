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
    /// S1 (variant sweep): the resource-scoping operators. <c>Using</c> and <c>UsingAsync</c> take an
    /// <see cref="IDisposable"/> resource; <c>UsingAwait</c> and <c>UsingAwaitAsync</c> take an
    /// <see cref="IAsyncDisposable"/>. Each wraps a source of N built in <c>[GlobalSetup]</c>, so the rows show the
    /// per-subscription resource cost on top of the source.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class UsingBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<int> _inner = default!;

        [GlobalSetup]
        public void Setup() => _inner = BenchmarkSources.Range(N);

        [Benchmark]
        public ValueTask Using()
        {
            var inner = _inner;
            return AsyncObservable.Using(static () => new Resource(), _ => inner).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask UsingAsync()
        {
            var inner = _inner;
            return AsyncObservable.UsingAsync(static () => new ValueTask<Resource>(new Resource()), _ => new ValueTask<IAsyncObservable<int>>(inner)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask UsingAwait()
        {
            var inner = _inner;
            return AsyncObservable.UsingAwait(static () => new Resource(), _ => inner).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask UsingAwaitAsync()
        {
            var inner = _inner;
            return AsyncObservable.UsingAwaitAsync(static () => new ValueTask<Resource>(new Resource()), _ => new ValueTask<IAsyncObservable<int>>(inner)).SubscribeConsumeAsync(Consumer);
        }

        private sealed class Resource : IDisposable, IAsyncDisposable
        {
            public void Dispose() { }

            public ValueTask DisposeAsync() => default;
        }
    }
}
