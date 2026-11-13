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
    /// queue on a scheduler. <c>TakeLast</c> passes <c>ImmediateAsyncScheduler</c> explicitly;
    /// <c>_DefaultScheduler</c> calls the overload without a scheduler, which also resolves to
    /// <c>ImmediateAsyncScheduler</c> (<c>AsyncObserver.TakeLast(observer, count)</c>). Both rows therefore drain
    /// inline and neither measures a task-pool hand-off; the second row covers the scheduler-less overload body.
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
