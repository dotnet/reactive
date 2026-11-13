// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// Resubscription. <c>Repeat(count)</c> re-runs the source. <c>Retry</c> (happy path) never errors so runs once;
    /// <c>Retry_Error</c> drives a source that faults 3× before succeeding, so Retry actually resubscribes.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class RetryRepeatBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Retry() => Observable.Range(1, N).Retry().SubscribeConsume(Consumer);

        [Benchmark]
        public void Repeat() => Observable.Range(1, N).Repeat(2).SubscribeConsume(Consumer);

        [Benchmark]
        public void Retry_Error() => FaultingSource.FaultThenSucceed(N, 3).Retry().SubscribeConsume(Consumer);
    }
}
