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
    /// S1 (variant sweep): <c>Distinct</c> with a key selector (synchronous and <c>ValueTask</c>) and with a key
    /// selector plus comparer. The key is <c>v &amp; 1023</c>, so the key set stops growing at 1,024 entries and
    /// at large <c>N</c> most elements are suppressed by a hash-set lookup.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class DistinctKeyBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Distinct_Key() =>
            BenchmarkSources.Range(N).Distinct(static v => v & 1023).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Distinct_Key_Async() =>
            BenchmarkSources.Range(N).Distinct(static v => new ValueTask<int>(v & 1023)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Distinct_Key_Comparer() =>
            BenchmarkSources.Range(N).Distinct(static v => v & 1023, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
