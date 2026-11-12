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
    /// S1: <c>Append</c>, <c>Prepend</c> and <c>StartWith</c> (an alias of Prepend) with a single value, a
    /// <c>params</c> array, an <c>IEnumerable</c>, and an explicit immediate scheduler. The value arrays are
    /// static so they do not allocate per invocation.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class AppendPrependStartWithBenchmarks : OperatorBenchmarkBase
    {
        private static readonly int[] Values = { -3, -2, -1 };
        private static readonly IEnumerable<int> Enumerable = new List<int> { -3, -2, -1 };

        [Benchmark]
        public ValueTask Append_Value() =>
            BenchmarkSources.Range(N).Append(-1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Append_Params() =>
            BenchmarkSources.Range(N).Append(Values).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Append_Enumerable() =>
            BenchmarkSources.Range(N).Append(Enumerable).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Append_Value_Scheduler() =>
            BenchmarkSources.Range(N).Append(-1, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Value() =>
            BenchmarkSources.Range(N).Prepend(-1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Params() =>
            BenchmarkSources.Range(N).Prepend(Values).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Enumerable() =>
            BenchmarkSources.Range(N).Prepend(Enumerable).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Prepend_Value_Scheduler() =>
            BenchmarkSources.Range(N).Prepend(-1, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask StartWith_Params() =>
            BenchmarkSources.Range(N).StartWith(Values).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask StartWith_Params_Scheduler() =>
            BenchmarkSources.Range(N).StartWith(ImmediateAsyncScheduler.Instance, Values).SubscribeConsumeAsync(Consumer);
    }
}
