// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// Combining-category exemplar (S1): <c>Merge</c> across a varying number of cold inner sources. Total
    /// emitted work is held at 100,000 elements (each inner emits <c>100,000 / Sources</c>), so the sweep isolates
    /// the per-inner subscription and bookkeeping cost. The rows use the
    /// <c>IAsyncObservable&lt;IAsyncObservable&lt;T&gt;&gt;</c> overload (the binary, <c>params</c> and
    /// <c>IEnumerable</c> forms forward to it), with an <see cref="ArrayAsyncSource{T}"/> as the outer. Merge takes an uncontended <c>AsyncGate</c> per
    /// element.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class MergeBenchmarks
    {
        private const int TotalWork = 100_000;

        [Params(2, 10, 100, 1000)]
        public int Sources;

        private readonly Consumer _consumer = new();
        private IAsyncObservable<IAsyncObservable<int>> _outer = default!;

        [GlobalSetup]
        public void Setup()
        {
            var per = TotalWork / Sources;
            var sources = new IAsyncObservable<int>[Sources];
            for (var i = 0; i < Sources; i++)
            {
                sources[i] = BenchmarkSources.Range(i * per, per);
            }

            _outer = new ArrayAsyncSource<IAsyncObservable<int>>(sources);
        }

        [Benchmark]
        public ValueTask Merge() => _outer.Merge().SubscribeConsumeAsync(_consumer);
    }
}
