// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1 (variant sweep): the non-observable <c>TakeUntil</c> forms. The stop predicate (synchronous and
    /// <c>ValueTask</c>) is <c>v &lt; 0</c>, which never matches, and the cancellation token belongs to a live
    /// <see cref="CancellationTokenSource"/> that is never cancelled. Every row therefore passes the whole source
    /// through and measures the per-element check, not early termination.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class TakeUntilPredicateBenchmarks : OperatorVariantBenchmarkBase
    {
        private CancellationTokenSource _cts = default!;

        [GlobalSetup]
        public void Setup() => _cts = new CancellationTokenSource();

        [GlobalCleanup]
        public void Cleanup() => _cts.Dispose();

        [Benchmark]
        public ValueTask TakeUntil_Predicate() =>
            BenchmarkSources.Range(N).TakeUntil(static v => v < 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeUntil_Predicate_Async() =>
            BenchmarkSources.Range(N).TakeUntil(static v => new ValueTask<bool>(v < 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeUntil_CancellationToken() =>
            BenchmarkSources.Range(N).TakeUntil(_cts.Token).SubscribeConsumeAsync(Consumer);
    }
}
