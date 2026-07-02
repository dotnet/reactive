// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>S1: <c>WithLatestFrom</c> combines each element of the first source with the latest of the second.</summary>
    [BenchmarkCategory("Multiple")]
    public class WithLatestFromBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void WithLatestFrom() =>
            Observable.Range(1, N).WithLatestFrom(Observable.Range(1, N), static (a, b) => a + b).SubscribeConsume(Consumer);
    }
}
