// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1 materialization: <c>ToList</c> and <c>ToArray</c> buffer the whole stream. Prime zero-allocation
    /// candidates — <c>ToArray</c> in particular does List-doubling growth plus a final exact-size copy.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class ToListToArrayBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ToList() => Observable.Range(1, N).ToList().SubscribeConsume(Consumer);

        [Benchmark]
        public void ToArray() => Observable.Range(1, N).ToArray().SubscribeConsume(Consumer);
    }
}
