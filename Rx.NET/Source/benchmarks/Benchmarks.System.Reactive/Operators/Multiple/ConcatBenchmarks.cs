// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>S1: <c>Concat</c> relays two sources one after the other.</summary>
    [BenchmarkCategory("Multiple")]
    public class ConcatBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Concat() =>
            Observable.Concat(Observable.Range(1, N), Observable.Range(1, N)).SubscribeConsume(Consumer);
    }
}
