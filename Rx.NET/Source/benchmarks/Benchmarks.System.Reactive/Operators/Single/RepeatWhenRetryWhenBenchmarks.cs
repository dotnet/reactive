// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// Signal-driven resubscription. <c>RepeatWhen</c> re-runs on the handler's signal (here once → source runs
    /// twice). <c>RetryWhen</c> (happy path) never errors, so the identity handler drains the source exactly once;
    /// <c>RetryWhen_Error</c> drives a source that faults 3× before succeeding, with a handler that permits 3 retries.
    /// The handlers use an indexed <c>TakeWhile</c>, not <c>Take(count)</c>: <c>Take</c> completes the handler in
    /// the same call that requests the last resubscription, which cancels that run before it emits anything.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class RepeatWhenRetryWhenBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void RepeatWhen() => Observable.Range(1, N).RepeatWhen(static signals => signals.TakeWhile(static (_, i) => i < 1)).SubscribeConsume(Consumer);

        [Benchmark]
        public void RetryWhen() => Observable.Range(1, N).RetryWhen(static errors => errors).SubscribeConsume(Consumer);

        [Benchmark]
        public void RetryWhen_Error() =>
            FaultingSource.FaultThenSucceed(N, 3).RetryWhen(static errors => errors.TakeWhile(static (_, i) => i < 3)).SubscribeConsume(Consumer);
    }
}
