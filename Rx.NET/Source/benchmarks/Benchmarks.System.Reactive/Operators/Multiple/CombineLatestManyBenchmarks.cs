// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: N-ary <c>CombineLatest</c> across a varying number of cold sources — the variadic
    /// <c>IList</c>-selector overload, whose sink tracks a per-source "has latest" array (a distinct code path
    /// from the specialised binary overload in <see cref="CombineLatestBenchmarks"/>). Total emitted work is held
    /// ~constant (each source emits <c>1,000,000 / Sources</c> elements) so the sweep isolates the per-source
    /// bookkeeping cost rather than raw element volume.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestManyBenchmarks
    {
        [Params(2, 4, 8, 16)]
        public int Sources;

        private readonly Consumer _consumer = new();
        private IObservable<int>[] _sources = default!;

        [GlobalSetup]
        public void Setup()
        {
            var per = 1_000_000 / Sources;
            _sources = new IObservable<int>[Sources];
            for (var i = 0; i < Sources; i++)
            {
                _sources[i] = Observable.Range(i * per, per);
            }
        }

        [Benchmark]
        public void CombineLatest() =>
            Observable.CombineLatest(_sources, static (IList<int> values) => values.Count).SubscribeConsume(_consumer);
    }
}
