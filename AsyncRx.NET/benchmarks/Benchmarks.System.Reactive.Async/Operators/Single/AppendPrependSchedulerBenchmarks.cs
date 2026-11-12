// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1 (variant sweep): the multi-value <c>Append</c> and <c>Prepend</c> overloads that take an explicit
    /// scheduler, with a <c>params</c> array and with an <c>IEnumerable</c>. The scheduler is the immediate one,
    /// so the extra values are pushed inline. The value collections are static, so they do not allocate per
    /// invocation. The enumerable is held in a field typed <c>IEnumerable&lt;int&gt;</c>, because an
    /// <c>int[]</c> would bind the <c>params</c> overload.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class AppendPrependSchedulerBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly int[] Values = { -3, -2, -1 };
        private static readonly IEnumerable<int> Enumerable = new List<int> { -3, -2, -1 };

        [Benchmark]
        public ValueTask Append_Params_Scheduler() =>
            BenchmarkSources.Range(N).Append(ImmediateAsyncScheduler.Instance, Values).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Append_Enumerable_Scheduler() =>
            BenchmarkSources.Range(N).Append(ImmediateAsyncScheduler.Instance, Enumerable).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Params_Scheduler() =>
            BenchmarkSources.Range(N).Prepend(ImmediateAsyncScheduler.Instance, Values).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Enumerable_Scheduler() =>
            BenchmarkSources.Range(N).Prepend(ImmediateAsyncScheduler.Instance, Enumerable).SubscribeConsumeAsync(Consumer);
    }
}
