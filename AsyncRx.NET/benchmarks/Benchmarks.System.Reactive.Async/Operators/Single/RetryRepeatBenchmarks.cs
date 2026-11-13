// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1 (variant sweep): resubscription operators. <c>Retry</c> over a source that faults twice before it
    /// succeeds (so it really resubscribes), <c>Retry(count)</c> with the same source, <c>Repeat(count)</c> over a
    /// successful source, and <c>Retry_Error</c>, where the retry budget runs out and the error reaches the sink.
    /// Each attempt emits <c>N</c> elements.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class RetryRepeatBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Retry() =>
            FaultingSource.FaultThenSucceed(N, 2).Retry().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Retry_Count() =>
            FaultingSource.FaultThenSucceed(N, 2).Retry(3).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Retry_Error() =>
            FaultingSource.Faulting(N).Retry(3).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Repeat_Count() =>
            BenchmarkSources.Range(N).Repeat(3).SubscribeConsumeAsync(Consumer);
    }
}
