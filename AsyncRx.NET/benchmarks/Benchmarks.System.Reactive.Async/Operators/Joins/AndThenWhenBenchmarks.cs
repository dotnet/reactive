// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Joins
{
    /// <summary>
    /// S1 (gated sweep): join patterns. <c>When(a.And(b).Then(...))</c> over two cold sources of N, a
    /// single-source plan <c>When(a.Then(...))</c>, and the <c>ValueTask</c> selector. The join observers take an
    /// <c>AsyncGate</c> per element.
    /// </summary>
    /// <remarks>
    /// Patterns over three or more sources (<c>a.And(b).And(c)</c> and longer chains) are measured in
    /// <see cref="AndThenWhenArityBenchmarks"/>.
    /// </remarks>
    [BenchmarkCategory("Joins")]
    public class AndThenWhenBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask When_Single()
        {
            var a = BenchmarkSources.Range(N);
            return AsyncObservable.When(a.Then(static x => x + 1)).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask When_Single_Async()
        {
            var a = BenchmarkSources.Range(N);
            return AsyncObservable.When(a.Then(static x => new ValueTask<int>(x + 1))).SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask When_And2()
        {
            var s = BenchmarkSources.Range(N);
            return AsyncObservable.When(s.And(s).Then(static (x, y) => x + y)).SubscribeConsumeAsync(Consumer);
        }
    }
}
