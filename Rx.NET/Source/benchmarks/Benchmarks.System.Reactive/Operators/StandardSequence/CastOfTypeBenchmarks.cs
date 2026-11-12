// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Cast</c> and <c>OfType</c> over a boxed (<c>object</c>) source. The N ints are boxed once in
    /// <c>[GlobalSetup]</c> and replayed via <c>ToObservable()</c>, so the measured region contains the cast/type-test
    /// per element (plus the array-to-observable source) but no per-element boxing.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class CastOfTypeBenchmarks : OperatorBenchmarkBase
    {
        private object[] _boxed = default!;

        [GlobalSetup]
        public void Setup()
        {
            _boxed = new object[N];
            for (var i = 0; i < _boxed.Length; i++)
            {
                _boxed[i] = i + 1;
            }
        }

        [Benchmark]
        public void Cast() => _boxed.ToObservable().Cast<int>().SubscribeConsume(Consumer);

        [Benchmark]
        public void OfType() => _boxed.ToObservable().OfType<int>().SubscribeConsume(Consumer);
    }
}
