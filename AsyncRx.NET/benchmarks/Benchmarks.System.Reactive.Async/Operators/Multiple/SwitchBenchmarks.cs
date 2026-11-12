// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (capped sweep): <c>Switch</c> over N cold inner sources with total work pinned at 100,000 elements (each
    /// inner emits <c>100,000 / N</c>). Each inner runs to completion before the next arrives, so this measures the
    /// per-inner switch cost plus the gated per-element path.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class SwitchBenchmarks : BenchmarkBase
    {
        private const int TotalWork = 100_000;

        // Capped: each inner emits TotalWork / N elements, so N above TotalWork would give empty inners.
        [Params(1, 100, 10_000)]
        public int N;

        private IAsyncObservable<IAsyncObservable<int>> _outer = default!;

        [GlobalSetup]
        public void Setup()
        {
            var inner = BenchmarkSources.Range(TotalWork / N);
            var inners = new IAsyncObservable<int>[N];
            for (var i = 0; i < inners.Length; i++)
            {
                inners[i] = inner;
            }

            _outer = new ArrayAsyncSource<IAsyncObservable<int>>(inners);
        }

        [Benchmark]
        public ValueTask Switch() => _outer.Switch().SubscribeConsumeAsync(Consumer);
    }
}
