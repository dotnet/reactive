// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1 fan-out: the <c>SelectMany</c> overloads beyond the plain observable selector in
    /// <see cref="SelectManyBenchmarks"/>, each a separate nested sink in <c>SelectMany.cs</c>: with a result selector
    /// (<c>ObservableSelector</c> of the three-type-parameter class), indexed (<c>ObservableSelectorIndexed</c>), the
    /// three-callback onNext/onError/onCompleted form (<c>ObservableSelectors</c>), and an <c>IEnumerable</c>-returning
    /// selector (<c>EnumerableSelector</c>, which enumerates inline rather than subscribing). All are cross-mapped like
    /// <see cref="SelectManyBenchmarks"/>: N outer elements each project to an inner sequence of <c>1,000,000 / N</c>,
    /// so total emitted work is ~constant and N reads as the inner-subscription count. The inner size and the inner
    /// array for the enumerable case are built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyOverloadBenchmarks : OperatorVariantBenchmarkBase
    {
        private int _innerSize;
        private int[] _inner;
        private IObservable<int> _empty;

        [GlobalSetup]
        public void Setup()
        {
            _innerSize = 1_000_000 / N;
            _inner = new int[_innerSize];
            for (var i = 0; i < _inner.Length; i++)
            {
                _inner[i] = i;
            }

            _empty = Observable.Empty<int>();
        }

        [Benchmark]
        public void SelectMany_ResultSelector() =>
            Observable.Range(1, N)
                .SelectMany(v => Observable.Range(v, _innerSize), static (outer, inner) => outer + inner)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void SelectMany_Indexed() =>
            Observable.Range(1, N)
                .SelectMany((v, i) => Observable.Range(v + i, _innerSize))
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void SelectMany_Selectors() =>
            Observable.Range(1, N)
                .SelectMany(v => Observable.Range(v, _innerSize), _ => _empty, () => _empty)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void SelectMany_Enumerable() =>
            Observable.Range(1, N).SelectMany(_ => _inner).SubscribeConsume(Consumer);
    }
}
