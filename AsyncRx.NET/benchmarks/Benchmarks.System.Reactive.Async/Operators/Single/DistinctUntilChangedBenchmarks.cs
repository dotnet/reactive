// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1: <c>DistinctUntilChanged</c> over a strictly increasing source (every element passes): plain, with a
    /// key selector (synchronous and <c>ValueTask</c>), and with a comparer. AsyncRx declares these without
    /// <c>this</c>, so they are called as static methods.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class DistinctUntilChangedBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask DistinctUntilChanged() =>
            AsyncObservable.DistinctUntilChanged(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask DistinctUntilChanged_Key() =>
            AsyncObservable.DistinctUntilChanged(BenchmarkSources.Range(N), static v => v >> 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask DistinctUntilChanged_Key_Async() =>
            AsyncObservable.DistinctUntilChanged(BenchmarkSources.Range(N), static v => new ValueTask<int>(v >> 1)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask DistinctUntilChanged_Comparer() =>
            AsyncObservable.DistinctUntilChanged(BenchmarkSources.Range(N), EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
