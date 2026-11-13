// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Distinct</c> over all-distinct values (the worst case: the set grows to N), with the default and an
    /// explicit comparer. AsyncRx declares <c>Distinct</c> without <c>this</c>, so it is not an extension method
    /// and is called as <c>AsyncObservable.Distinct(source)</c>.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class DistinctBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Distinct() =>
            AsyncObservable.Distinct(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Distinct_Comparer() =>
            AsyncObservable.Distinct(BenchmarkSources.Range(N), EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
