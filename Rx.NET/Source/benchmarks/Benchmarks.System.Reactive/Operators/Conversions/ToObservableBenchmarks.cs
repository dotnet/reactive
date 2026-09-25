// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Conversions
{
    /// <summary>S1: <c>ToObservable</c> bridges a pull sequence (<c>IEnumerable</c>) into a push sequence.</summary>
    [BenchmarkCategory("Conversions")]
    public class ToObservableBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ToObservable() => Enumerable.Range(1, N).ToObservable().SubscribeConsume(Consumer);
    }
}
