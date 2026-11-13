// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (variant sweep): the <c>CombineLatest</c> and <c>Zip</c> overloads the arity classes do not reach: the
    /// generated 8-source selector forms, <c>CombineLatest(params)</c> over 8 sources, the
    /// <c>IEnumerable</c>-of-sources forms with a list selector (4 sources), and <c>Zip</c> against an
    /// <c>IEnumerable</c> of N. Every observable source is one cold source of N, reused, as in
    /// <see cref="CombineLatestArityBenchmarks"/>.
    /// </summary>
    /// <remarks>
    /// The params row passes a pre-built array: with eight separate arguments the generated 8-arity tuple overload
    /// is applicable in its normal form and wins over the expanded params form. The <c>IEnumerable</c>-of-sources
    /// rows pass a <see cref="List{T}"/> so the params overload cannot bind.
    /// </remarks>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestZipVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<int>[] _eightSources = default!;
        private List<IAsyncObservable<int>> _fourSources = default!;
        private IEnumerable<int> _enumerable = default!;

        [GlobalSetup]
        public void Setup()
        {
            var s = BenchmarkSources.Range(N);
            _eightSources = new[] { s, s, s, s, s, s, s, s };
            _fourSources = new List<IAsyncObservable<int>> { s, s, s, s };

            var values = new int[N];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = i + 1;
            }

            _enumerable = values;
        }

        [Benchmark]
        public ValueTask CombineLatest_8_Selector()
        {
            var s = BenchmarkSources.Range(N);
            return s.CombineLatest(s, s, s, s, s, s, s, static (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)
                .SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Zip_8_Selector()
        {
            var s = BenchmarkSources.Range(N);
            return s.Zip(s, s, s, s, s, s, s, static (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)
                .SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask CombineLatest_Params_8() =>
            AsyncObservable.CombineLatest(_eightSources).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask CombineLatest_Enumerable_Selector() =>
            AsyncObservable.CombineLatest(_fourSources, static (IList<int> l) => l[0] + l[3]).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask CombineLatest_Enumerable_Selector_Async() =>
            AsyncObservable.CombineLatest(_fourSources, static (IList<int> l) => new ValueTask<int>(l[0] + l[3])).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Zip_Enumerable() =>
            BenchmarkSources.Range(N).Zip(_enumerable).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Zip_Enumerable_Selector() =>
            BenchmarkSources.Range(N).Zip(_enumerable, static (int a, int b) => a + b).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Zip_Sources_Selector() =>
            AsyncObservable.Zip(_fourSources, static (IList<int> l) => l[0] + l[3]).SubscribeConsumeAsync(Consumer);
    }
}
