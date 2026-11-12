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
    /// S1 (variant sweep): <c>MinBy</c> with a <c>ValueTask</c> key selector and with an explicit key comparer.
    /// The key is <c>v &amp; 1023</c>, so at large <c>N</c> several elements share the minimum key and the result
    /// list grows to about <c>N / 1024</c> entries.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinByVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask MinBy_Async() =>
            BenchmarkSources.Range(N).MinBy(static v => new ValueTask<int>(v & 1023)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask MinBy_Comparer() =>
            BenchmarkSources.Range(N).MinBy(static v => v & 1023, Comparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
