// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: <c>CombineLatest</c> emits the combination whenever either source produces, once both have a value.
    /// (Reimplemented cleanly — the old benchmark delegated to a unit test and measured its assertions.)
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CombineLatestBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void CombineLatest() =>
            Observable.CombineLatest(Observable.Range(1, N), Observable.Range(1, N), static (a, b) => a + b).SubscribeConsume(Consumer);
    }
}
