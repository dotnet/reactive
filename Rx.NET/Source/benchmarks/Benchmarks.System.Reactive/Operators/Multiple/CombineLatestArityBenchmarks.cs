// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: the fixed-arity <c>CombineLatest</c> overloads at 3, 4, 8 and 16 sources. Each arity is a separately
    /// generated <c>CombineLatest&lt;T1, ..., TResult&gt;</c> sink (typed per-source observers and fields), distinct
    /// from the binary sink in <see cref="CombineLatestBenchmarks"/> and the <c>IList</c>-based n-ary sink in
    /// <see cref="CombineLatestManyBenchmarks"/>. Every source is the same cold <c>Range(1, N)</c> instance (built
    /// in <c>[GlobalSetup]</c>), so total source work is arity × N; compare rows within an arity, and across arities
    /// read it as per-source cost. N is capped at 100,000: the cold sources interleave on the current-thread
    /// trampoline at ~64 B allocated per source element, so the 16-source case at N=1,000,000 handles 16M source
    /// elements and took ~2.5 s and ~1 GB allocated per invocation.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestArityBenchmarks : BenchmarkBase
    {
        [Params(1, 1_000, 100_000)]
        public int N;

        private IObservable<int> _s;

        [GlobalSetup]
        public void Setup() => _s = Observable.Range(1, N);

        [Benchmark]
        public void CombineLatest_3() =>
            Observable.CombineLatest(_s, _s, _s, static (a, b, c) => a + b + c).SubscribeConsume(Consumer);

        [Benchmark]
        public void CombineLatest_4() =>
            Observable.CombineLatest(_s, _s, _s, _s, static (a, b, c, d) => a + b + c + d).SubscribeConsume(Consumer);

        [Benchmark]
        public void CombineLatest_8() =>
            Observable.CombineLatest(
                    _s, _s, _s, _s, _s, _s, _s, _s,
                    static (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void CombineLatest_16() =>
            Observable.CombineLatest(
                    _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s,
                    static (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)
                .SubscribeConsume(Consumer);
    }
}
