// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// <c>ResetExceptionDispatchState</c> only acts on the <c>OnError</c> path (clearing accumulated dispatch state
    /// so a re-thrown exception doesn't grow its stack trace). The plain method is a pass-through (wrapper cost);
    /// <c>_Error</c> routes repeated faults through it across a <c>Retry</c> — the scenario it exists for.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class ResetExceptionDispatchStateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ResetExceptionDispatchState() =>
            Observable.Range(1, N).ResetExceptionDispatchState().SubscribeConsume(Consumer);

        [Benchmark]
        public void ResetExceptionDispatchState_Error() =>
            FaultingSource.FaultThenSucceed(N, 3).ResetExceptionDispatchState().Retry().SubscribeConsume(Consumer);
    }
}
