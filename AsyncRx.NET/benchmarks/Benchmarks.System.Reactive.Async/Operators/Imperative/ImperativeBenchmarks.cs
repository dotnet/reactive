// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Imperative
{
    /// <summary>
    /// S1 (variant sweep): the imperative combinators, each running a source of N once: <c>While</c> (condition
    /// true for one pass), <c>DoWhile</c> (condition false), <c>For</c> over a one-element sequence and
    /// <c>Case</c> selecting from a one-entry dictionary, each with synchronous and <c>ValueTask</c> callbacks.
    /// The loop conditions keep a counter, so they allocate one closure per invocation.
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class ImperativeBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly int[] OneElement = { 0 };

        private IAsyncObservable<int> _source = default!;
        private Dictionary<int, IAsyncObservable<int>> _cases = default!;

        [GlobalSetup]
        public void Setup()
        {
            _source = BenchmarkSources.Range(N);
            _cases = new Dictionary<int, IAsyncObservable<int>> { [1] = _source };
        }

        [Benchmark]
        public ValueTask While()
        {
            var passes = 0;
            return AsyncObservable.While(() => passes++ == 0, _source).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask While_Async()
        {
            var passes = 0;
            return AsyncObservable.While(() => new ValueTask<bool>(passes++ == 0), _source).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask DoWhile() =>
            AsyncObservable.DoWhile(_source, static () => false).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask DoWhile_Async() =>
            AsyncObservable.DoWhile(_source, static () => new ValueTask<bool>(false)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask For()
        {
            var source = _source;
            return AsyncObservable.For(OneElement, _ => source).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask For_Async()
        {
            var source = _source;
            return AsyncObservable.For(OneElement, _ => new ValueTask<IAsyncObservable<int>>(source)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Case() =>
            AsyncObservable.Case(static () => 1, _cases).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Case_Async() =>
            AsyncObservable.Case(static () => new ValueTask<int>(1), _cases).SubscribeConsumeAsync(Consumer);
    }
}
