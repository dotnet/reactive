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
    /// S1 (variant sweep): the collection operators that <see cref="ToDictionaryToLookupBenchmarks"/> and
    /// <see cref="ToListToArrayBenchmarks"/> do not cover: key-only <c>ToDictionary</c> and <c>ToLookup</c>
    /// (synchronous, <c>ValueTask</c>, and for <c>ToDictionary</c> with a comparer), <c>ToLookup</c> with a value
    /// selector and a comparer, and <c>ToHashSet</c> with a comparer. Keys are as in
    /// <see cref="ToDictionaryToLookupBenchmarks"/>: unique (<c>v</c>) for the dictionary and the hash set, 16
    /// keys (<c>v &amp; 15</c>) for the lookup. At N = 1,000,000 each invocation builds a million-entry
    /// collection.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class ToCollectionVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask ToDictionary_Key() =>
            BenchmarkSources.Range(N).ToDictionary(static v => v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToDictionary_Key_Async() =>
            BenchmarkSources.Range(N).ToDictionary(static v => new ValueTask<int>(v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToDictionary_Key_Comparer() =>
            BenchmarkSources.Range(N).ToDictionary(static v => v, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToLookup_Key() =>
            BenchmarkSources.Range(N).ToLookup(static v => v & 15).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToLookup_Key_Async() =>
            BenchmarkSources.Range(N).ToLookup(static v => new ValueTask<int>(v & 15)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToLookup_Comparer() =>
            BenchmarkSources.Range(N).ToLookup(static v => v & 15, static v => v, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToHashSet_Comparer() =>
            BenchmarkSources.Range(N).ToHashSet(EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
