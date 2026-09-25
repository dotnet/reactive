// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>S1: <c>AsObservable</c> hides the source's concrete type behind a thin wrapper.</summary>
    [BenchmarkCategory("Single")]
    public class AsObservableBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void AsObservable() => Observable.Range(1, N).AsObservable().SubscribeConsume(Consumer);
    }
}
