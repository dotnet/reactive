// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>S1: the count/predicate-based Take/Skip family (the time-based variants live under Operators/Time).</summary>
    [BenchmarkCategory("StandardSequence")]
    public class TakeSkipBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Take() => Observable.Range(1, 2 * N).Take(N).SubscribeConsume(Consumer);

        [Benchmark]
        public void Skip() => Observable.Range(1, 2 * N).Skip(N).SubscribeConsume(Consumer);

        [Benchmark]
        public void TakeWhile()
        {
            var n = N;
            Observable.Range(1, 2 * N).TakeWhile(v => v <= n).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void SkipWhile()
        {
            var n = N;
            Observable.Range(1, 2 * N).SkipWhile(v => v <= n).SubscribeConsume(Consumer);
        }
    }
}
