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
    /// S1: the fixed-arity <c>Zip</c> overloads at 3, 4, 8 and 16 sources. Each arity is a separately generated
    /// <c>Zip&lt;T1, ..., TResult&gt;</c> sink (one typed queue per source), distinct from the binary sink in
    /// <see cref="ZipBenchmarks"/> and the <c>IList</c>-based n-ary sink in <see cref="ZipManyBenchmarks"/>. Every
    /// source is the same cold <c>Range(1, N)</c> instance (built in <c>[GlobalSetup]</c>), so total source work is
    /// arity × N and N tuples are emitted. N is capped at 100,000: the cold sources interleave on the current-thread
    /// trampoline at ~64 B allocated per source element, so the 16-source case at N=1,000,000 handles 16M source
    /// elements and took ~2.5 s and ~1 GB allocated per invocation.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class ZipArityBenchmarks : BenchmarkBase
    {
        [Params(1, 1_000, 100_000)]
        public int N;

        private IObservable<int> _s;

        [GlobalSetup]
        public void Setup() => _s = Observable.Range(1, N);

        [Benchmark]
        public void Zip_3() =>
            Observable.Zip(_s, _s, _s, static (a, b, c) => a + b + c).SubscribeConsume(Consumer);

        [Benchmark]
        public void Zip_4() =>
            Observable.Zip(_s, _s, _s, _s, static (a, b, c, d) => a + b + c + d).SubscribeConsume(Consumer);

        [Benchmark]
        public void Zip_8() =>
            Observable.Zip(
                    _s, _s, _s, _s, _s, _s, _s, _s,
                    static (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h)
                .SubscribeConsume(Consumer);

        [Benchmark]
        public void Zip_16() =>
            Observable.Zip(
                    _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s, _s,
                    static (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p)
                .SubscribeConsume(Consumer);
    }
}
