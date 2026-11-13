// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (variant sweep): the source-sequence error handlers. Plain rows run over a source that completes, so
    /// they measure the pass-through cost; <c>_Error</c> rows fault after N elements and switch to a second
    /// source of N. <c>OnErrorResumeNext</c> moves on after both completion and error.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CatchBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Catch() =>
            BenchmarkSources.Range(N).Catch(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Catch_Error() =>
            FaultingSource.Faulting(N).Catch(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Catch_Params_Error() =>
            AsyncObservable.Catch(FaultingSource.Faulting(N), FaultingSource.Faulting(N), BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask OnErrorResumeNext() =>
            BenchmarkSources.Range(N).OnErrorResumeNext(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask OnErrorResumeNext_Error() =>
            FaultingSource.Faulting(N).OnErrorResumeNext(BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask OnErrorResumeNext_Params() =>
            AsyncObservable.OnErrorResumeNext(FaultingSource.Faulting(N), BenchmarkSources.Range(N), BenchmarkSources.Range(N)).SubscribeConsumeAsync(Consumer);
    }
}
