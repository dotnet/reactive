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
    /// S1 (variant sweep): the other <c>GroupByUntil</c> overloads: <c>ValueTask</c> key selector, element
    /// selector (synchronous and <c>ValueTask</c>), capacity hint, key comparer, and all of them together. As in
    /// <see cref="GroupByUntilBenchmarks"/>, the source is partitioned into 8 groups (<c>v &amp; 7</c>) whose
    /// duration (<c>Never</c>) does not fire, and only the group observables are consumed, so the rows measure
    /// the per-element routing and duration bookkeeping. Capacity is 8; the comparer is
    /// <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    /// <remarks>
    /// The <c>ValueTask</c>-key rows without a comparer type the duration selector's parameter as
    /// <c>IGroupedAsyncObservable&lt;int, int&gt;</c>. With an untyped <c>_ =&gt;</c> the call is ambiguous (CS0121)
    /// with the synchronous-key overload inferred at <c>TKey = ValueTask&lt;int&gt;</c>; the typed parameter fixes
    /// <c>TKey = int</c>, so only the <c>ValueTask</c> overload applies.
    /// </remarks>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByUntilOverloadBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask GroupByUntil_Async() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => new ValueTask<int>(v & 7), static (IGroupedAsyncObservable<int, int> _) => AsyncObservable.Never<int>())
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Element() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static v => v + 1, static _ => AsyncObservable.Never<int>())
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Element_Async() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => new ValueTask<int>(v & 7), static v => new ValueTask<int>(v + 1), static (IGroupedAsyncObservable<int, int> _) => AsyncObservable.Never<int>())
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Capacity() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static _ => AsyncObservable.Never<int>(), 8)
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Comparer() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static _ => AsyncObservable.Never<int>(), EqualityComparer<int>.Default)
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Element_Capacity_Comparer() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => v & 7, static v => v + 1, static _ => AsyncObservable.Never<int>(), 8, EqualityComparer<int>.Default)
                .SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask GroupByUntil_Element_Capacity_Comparer_Async() =>
            BenchmarkSources.Range(N)
                .GroupByUntil(static v => new ValueTask<int>(v & 7), static v => new ValueTask<int>(v + 1), static _ => AsyncObservable.Never<int>(), 8, EqualityComparer<int>.Default)
                .SubscribeConsumeAsync(Consumer);
    }
}
