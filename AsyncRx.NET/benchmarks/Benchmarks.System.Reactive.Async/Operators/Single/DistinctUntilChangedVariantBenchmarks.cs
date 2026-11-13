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
    /// S1 (variant sweep): <c>DistinctUntilChanged</c> with a synchronous key selector and an explicit key
    /// comparer. The key (<c>v &gt;&gt; 1</c>) is the one <see cref="DistinctUntilChangedBenchmarks"/> uses, so
    /// every second element of the increasing source is suppressed.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class DistinctUntilChangedVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask DistinctUntilChanged_Key_Comparer() =>
            AsyncObservable.DistinctUntilChanged(BenchmarkSources.Range(N), static v => v >> 1, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
