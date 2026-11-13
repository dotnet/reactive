// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>
    /// Side-effect callbacks: <c>Do</c> (per-element / on-error) and <c>Finally</c> (on-termination). The
    /// <c>_Error</c> variants drive a faulting source so the <c>onError</c> / error-termination callbacks fire.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class DoFinallyBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Do() => Observable.Range(1, N).Do(static _ => { }).SubscribeConsume(Consumer);

        [Benchmark]
        public void Finally() => Observable.Range(1, N).Finally(static () => { }).SubscribeConsume(Consumer);

        [Benchmark]
        public void Do_Error() => FaultingSource.Faulting(N).Do(static _ => { }, static _ => { }).SubscribeConsume(Consumer);

        [Benchmark]
        public void Finally_Error() => FaultingSource.Faulting(N).Finally(static () => { }).SubscribeConsume(Consumer);
    }
}
