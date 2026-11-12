// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: the count- and predicate-based filters. <c>Take</c> and <c>Skip</c> split at N/2; the predicate rows
    /// pass every element, so they measure the per-element predicate cost.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: when <c>Take(count)</c> completes early over a cold source that pushes inside
    /// <c>SubscribeAsync</c> (such as <c>Range</c> on the immediate scheduler), the source has not yet returned its
    /// subscription, so it cannot be cancelled. It keeps calling the terminated observer, which throws
    /// <c>InvalidOperationException("The observer has already terminated.")</c>. <c>Take</c> is therefore driven
    /// by <see cref="Pump.RunUntilDoneAsync{TState, TResult}"/>, which pushes into a hot source and stops once the
    /// sink has terminated.
    /// </remarks>
    [BenchmarkCategory("StandardSequence")]
    public class TakeSkipBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Take() =>
            Pump.RunUntilDoneAsync(N, Consumer, Math.Max(1, N / 2), static (s, count) => s.Take(count));

        [Benchmark]
        public ValueTask Skip() =>
            BenchmarkSources.Range(N).Skip(N / 2).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeWhile() =>
            BenchmarkSources.Range(N).TakeWhile(static v => v > 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeWhile_Async() =>
            BenchmarkSources.Range(N).TakeWhile(static v => new ValueTask<bool>(v > 0)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SkipWhile() =>
            BenchmarkSources.Range(N).SkipWhile(static v => v < 0).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SkipWhile_Async() =>
            BenchmarkSources.Range(N).SkipWhile(static v => new ValueTask<bool>(v < 0)).SubscribeConsumeAsync(Consumer);
    }
}
