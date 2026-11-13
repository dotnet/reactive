// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// S1 (variant sweep): <c>Range</c> on its default <c>TaskPoolAsyncScheduler</c>, which moves the push loop to
    /// the thread pool and completes the sink on a pool thread.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class RangeDefaultSchedulerBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Range_DefaultScheduler() =>
            AsyncObservable.Range(1, N).SubscribeConsumeAsync(Consumer);
    }
}
