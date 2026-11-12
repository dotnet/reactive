// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>MinBy</c> and <c>MaxBy</c> (which return the list of elements sharing the extreme key), with the
    /// synchronous key selector, the <c>ValueTask</c> key selector and an explicit comparer. AsyncRx declares
    /// these without <c>this</c>, so they are called as static methods.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinByMaxByBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask MinBy() =>
            AsyncObservable.MinBy(BenchmarkSources.Range(N), static v => v & 1023).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask MaxBy() =>
            AsyncObservable.MaxBy(BenchmarkSources.Range(N), static v => v & 1023).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask MaxBy_Async() =>
            AsyncObservable.MaxBy(BenchmarkSources.Range(N), static v => new ValueTask<int>(v & 1023)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask MaxBy_Comparer() =>
            AsyncObservable.MaxBy(BenchmarkSources.Range(N), static v => v & 1023, Comparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
