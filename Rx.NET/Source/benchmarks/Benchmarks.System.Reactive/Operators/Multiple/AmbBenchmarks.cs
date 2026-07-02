// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>S1: <c>Amb</c> races two sources; the synchronous <c>Range</c> wins over <c>Never</c>.</summary>
    [BenchmarkCategory("Multiple")]
    public class AmbBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Amb() => Observable.Range(1, N).Amb(Observable.Never<int>()).SubscribeConsume(Consumer);
    }
}
