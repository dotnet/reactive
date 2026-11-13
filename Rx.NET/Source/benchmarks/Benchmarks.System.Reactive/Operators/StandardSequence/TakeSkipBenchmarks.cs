// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>S1: the count/predicate-based Take/Skip family (the time-based variants live under Operators/Time).</summary>
    [BenchmarkCategory("StandardSequence")]
    public class TakeSkipBenchmarks : OperatorBenchmarkBase
    {
        private Func<int, bool> _upToN = default!;

        // The predicate depends on N, so it is built once here: the measured region then allocates no delegate for it.
        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _upToN = v => v <= n;
        }

        [Benchmark]
        public void Take() => Observable.Range(1, 2 * N).Take(N).SubscribeConsume(Consumer);

        [Benchmark]
        public void Skip() => Observable.Range(1, 2 * N).Skip(N).SubscribeConsume(Consumer);

        [Benchmark]
        public void TakeWhile()
        {
            Observable.Range(1, 2 * N).TakeWhile(_upToN).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void SkipWhile()
        {
            Observable.Range(1, 2 * N).SkipWhile(_upToN).SubscribeConsume(Consumer);
        }
    }
}
