// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// S1 (variant sweep): <c>SubscribeOn</c> with one scheduler (subscribe and dispose on the task pool) and with
    /// separate subscribe (task pool) and dispose (immediate) schedulers. The source then runs on a pool thread.
    /// </summary>
    [BenchmarkCategory("Concurrency")]
    public class SubscribeOnBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask SubscribeOn_TaskPool() =>
            BenchmarkSources.Range(N).SubscribeOn(TaskPoolAsyncScheduler.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SubscribeOn_TaskPool_DisposeImmediate() =>
            BenchmarkSources.Range(N).SubscribeOn(TaskPoolAsyncScheduler.Default, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);
    }
}
