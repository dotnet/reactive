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
    /// S1 (variant sweep): <c>Concat</c> in its binary, <c>params</c> and <c>IEnumerable</c> forms, with total work
    /// of about N elements split across the sources.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class ConcatBenchmarks : OperatorVariantBenchmarkBase
    {
        private IAsyncObservable<int> _half = default!;
        private IAsyncObservable<int> _quarter = default!;
        private IEnumerable<IAsyncObservable<int>> _sequence = default!;

        [GlobalSetup]
        public void Setup()
        {
            _half = BenchmarkSources.Range(Math.Max(1, N / 2));
            _quarter = BenchmarkSources.Range(Math.Max(1, N / 4));
            _sequence = new List<IAsyncObservable<int>> { _quarter, _quarter, _quarter, _quarter };
        }

        [Benchmark]
        public ValueTask Concat_Binary() => _half.Concat(_half).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Concat_Params() => AsyncObservable.Concat(_quarter, _quarter, _quarter, _quarter).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Concat_Enumerable() => _sequence.Concat().SubscribeConsumeAsync(Consumer);
    }
}
