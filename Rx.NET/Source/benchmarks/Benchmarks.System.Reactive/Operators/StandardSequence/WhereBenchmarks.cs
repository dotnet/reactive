// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>S1: <c>Where</c> filters a stream; the predicate here keeps roughly half the elements.</summary>
    [BenchmarkCategory("StandardSequence")]
    public class WhereBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Where() => Observable.Range(1, N).Where(static v => (v & 1) == 0).SubscribeConsume(Consumer);
    }
}
