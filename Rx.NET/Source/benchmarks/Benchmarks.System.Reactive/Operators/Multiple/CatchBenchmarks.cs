// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// Error-continuation. Plain <c>Catch</c> runs the happy path: the source completes, the second source is never
    /// subscribed, and N elements are emitted. <c>OnErrorResumeNext</c> moves on to the second source after normal
    /// completion as well as after an error, so both of its methods subscribe the second source and emit 2N
    /// elements; the plain one continues from <c>OnCompleted</c> and the <c>_Error</c> one from <c>OnError</c>.
    /// <c>Catch_Error</c> drives an <c>OnError</c> through, so its second source is subscribed too (2N elements).
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
