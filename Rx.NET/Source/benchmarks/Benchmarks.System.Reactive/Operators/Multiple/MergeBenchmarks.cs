// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// Combining-category exemplar (S1): <c>Merge</c> across a varying number of cold sources. Total emitted
    /// work is held ~constant (each source emits <c>1,000,000 / Sources</c> elements) so the sweep isolates the
    /// per-source subscription/bookkeeping cost.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class MergeBenchmarks
    {
        [Params(2, 10, 100, 1000)]
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
        public void Merge() => Observable.Merge(_sources).SubscribeConsume(_consumer);
    }
}
