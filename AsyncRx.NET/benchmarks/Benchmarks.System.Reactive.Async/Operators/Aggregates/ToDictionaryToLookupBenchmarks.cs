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
    /// S1: <c>ToDictionary</c> (unique keys) and <c>ToLookup</c> (16 keys), with synchronous selectors,
    /// <c>ValueTask</c> selectors and an explicit comparer.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class ToDictionaryToLookupBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask ToDictionary() =>
            BenchmarkSources.Range(N).ToDictionary(static v => v, static v => v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToDictionary_Async() =>
            BenchmarkSources.Range(N).ToDictionary(static v => new ValueTask<int>(v), static v => new ValueTask<int>(v)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToDictionary_Comparer() =>
            BenchmarkSources.Range(N).ToDictionary(static v => v, static v => v, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToLookup() =>
            BenchmarkSources.Range(N).ToLookup(static v => v & 15, static v => v).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ToLookup_Async() =>
            BenchmarkSources.Range(N).ToLookup(static v => new ValueTask<int>(v & 15), static v => new ValueTask<int>(v)).SubscribeConsumeAsync(Consumer);
    }
}
