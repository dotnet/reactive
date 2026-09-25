// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>
    /// <c>Using</c> binds a disposable resource's lifetime to the subscription. <c>Using_Error</c> drives a faulting
    /// source so the resource is disposed on the error path.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class UsingBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Using()
        {
            var n = N;
            Observable.Using(static () => Disposable.Empty, _ => Observable.Range(1, n)).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void Using_Error()
        {
            var n = N;
            Observable.Using(static () => Disposable.Empty, _ => FaultingSource.Faulting(n)).SubscribeConsume(Consumer);
        }
    }
}
