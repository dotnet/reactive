// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Joins
{
    /// <summary>
    /// S1 (variant sweep): join patterns over more than two sources, built with the generated
    /// <c>AsyncPattern.And</c> chain: <c>When(s.And(s).And(s).Then(...))</c> with a synchronous and a
    /// <c>ValueTask</c> selector, and the chain to eight sources with the 8-argument selector. One cold source of N
    /// is reused for every position, as in <see cref="AndThenWhenBenchmarks"/>.
    /// </summary>
    [BenchmarkCategory("Joins")]
    public class AndThenWhenArityBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask When_And3()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.When(s.And(s).And(s).Then(static (x, y, z) => x + y + z)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask When_And3_Async()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.When(s.And(s).And(s).Then(static (x, y, z) => new ValueTask<int>(x + y + z))).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask When_And8()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.When(
                    s.And(s).And(s).And(s).And(s).And(s).And(s).And(s)
                        .Then(static (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h))
                .SubscribeConsumeAsync(Consumer);
        }
    }
}
