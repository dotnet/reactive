// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1 (variant sweep): the count-based tail operators keeping 16 elements. <c>TakeLast(count)</c> drains its
    /// queue on a scheduler, so it is measured with the immediate scheduler and, in <c>_DefaultScheduler</c>, with
    /// its default <c>TaskPoolAsyncScheduler</c>.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class TakeLastSkipLastBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask TakeLast() =>
            BenchmarkSources.Range(N).TakeLast(16, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeLast_DefaultScheduler() =>
            BenchmarkSources.Range(N).TakeLast(16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SkipLast() =>
            BenchmarkSources.Range(N).SkipLast(16).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask TakeLastBuffer() =>
            BenchmarkSources.Range(N).TakeLastBuffer(16).SubscribeConsumeAsync(Consumer);
    }
}
