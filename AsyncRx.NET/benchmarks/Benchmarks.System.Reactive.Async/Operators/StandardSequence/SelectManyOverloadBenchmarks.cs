// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (gated sweep): the other <c>SelectMany</c> overloads (result selector, <c>ValueTask</c> selectors,
    /// indexed) with the same pinned total work as <see cref="SelectManyBenchmarks"/>: <c>N</c> inner
    /// subscriptions of <c>100,000 / N</c> elements each.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyOverloadBenchmarks : GatedOperatorBenchmarkBase
    {
        private const int TotalWork = 100_000;

        private IAsyncObservable<int> _inner = default!;
        private Func<int, IAsyncObservable<int>> _selector = default!;
        private Func<int, ValueTask<IAsyncObservable<int>>> _selectorAsync = default!;
        private Func<int, int, IAsyncObservable<int>> _selectorIndexed = default!;
        private Func<int, int, ValueTask<IAsyncObservable<int>>> _selectorIndexedAsync = default!;

        [GlobalSetup]
        public void Setup()
        {
            var inner = BenchmarkSources.Range(TotalWork / N);
            _inner = inner;
            _selector = _ => inner;
            _selectorAsync = _ => new ValueTask<IAsyncObservable<int>>(inner);
            _selectorIndexed = (_, _) => inner;
            _selectorIndexedAsync = (_, _) => new ValueTask<IAsyncObservable<int>>(inner);
        }

        [Benchmark]
        public ValueTask SelectMany_ResultSelector() =>
            BenchmarkSources.Range(N).SelectMany(_selector, static (o, i) => o + i).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Async() =>
            BenchmarkSources.Range(N).SelectMany(_selectorAsync).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_ResultSelector_Async() =>
            BenchmarkSources.Range(N).SelectMany(_selectorAsync, static (o, i) => new ValueTask<int>(o + i)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Indexed() =>
            BenchmarkSources.Range(N).SelectMany(_selectorIndexed).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Indexed_Async() =>
            BenchmarkSources.Range(N).SelectMany(_selectorIndexedAsync).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SelectMany_Indexed_ResultSelector() =>
            BenchmarkSources.Range(N).SelectMany(_selectorIndexed, static (o, oi, i, ii) => o + i).SubscribeConsumeAsync(Consumer);
    }
}
