// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// Error-continuation. The plain methods run the happy path (source completes, so the handler/second source is
    /// never needed); the <c>_Error</c> methods drive an <c>OnError</c> through, so the handler is actually
    /// subscribed — the path that matters for these operators.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CatchBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Catch() => Observable.Range(1, N).Catch(Observable.Range(1, N)).SubscribeConsume(Consumer);

        [Benchmark]
        public void OnErrorResumeNext() =>
            Observable.OnErrorResumeNext(Observable.Range(1, N), Observable.Range(1, N)).SubscribeConsume(Consumer);

        [Benchmark]
        public void Catch_Error() =>
            FaultingSource.Faulting(N).Catch(Observable.Range(1, N)).SubscribeConsume(Consumer);

        [Benchmark]
        public void OnErrorResumeNext_Error() =>
            Observable.OnErrorResumeNext(FaultingSource.Faulting(N), Observable.Range(1, N)).SubscribeConsume(Consumer);
    }
}
