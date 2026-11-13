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
    /// S1: <c>TakeUntil(stopPredicate)</c>, the predicate overload with its own <c>TakeUntilPredicate</c> sink (a
    /// per-element predicate call, then complete-and-unsubscribe on the first match). The source is 2N long and the
    /// predicate first matches at element N, so N elements are emitted (the matching one included) and the
    /// unsubscribe-from-a-running-Range path is exercised, mirroring the <c>TakeWhile</c> shape in
    /// <c>TakeSkipBenchmarks</c>.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class TakeUntilPredicateBenchmarks : OperatorVariantBenchmarkBase
    {
        private Func<int, bool> _reachedN = default!;

        // The predicate depends on N, so it is built once here: the measured region then allocates no delegate for it.
        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _reachedN = v => v >= n;
        }

        [Benchmark]
        public void TakeUntil_Predicate()
        {
            Observable.Range(1, 2 * N).TakeUntil(_reachedN).SubscribeConsume(Consumer);
        }
    }
}
