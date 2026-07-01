// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Operators.Joins
{
    /// <summary>
    /// Join-patterns-category exemplar (S1): <c>And</c>/<c>Then</c>/<c>When</c> (the Rx join calculus) pairs
    /// elements from two sources. N is capped because unmatched elements are queued (O(N) memory).
    /// </summary>
    [BenchmarkCategory("Joins")]
    public class AndThenWhenBenchmarks
    {
        [Params(100, 1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();

        [Benchmark]
        public void When()
        {
            var left = Observable.Range(1, N);
            var right = Observable.Range(1, N);

            Observable.When(left.And(right).Then(static (a, b) => a + b))
                .Subscribe(new Infrastructure.ConsumingObserver<int>(_consumer));
        }
    }
}
