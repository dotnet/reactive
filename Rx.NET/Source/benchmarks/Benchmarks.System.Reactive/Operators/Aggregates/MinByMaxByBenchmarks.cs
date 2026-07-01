// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>MinBy</c>/<c>MaxBy</c> keep a running list of the elements sharing the current best key (a keyed
    /// key-selector plus list churn), returning an <c>IList</c> on completion.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class MinByMaxByBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void MinBy() => Observable.Range(1, N).MinBy(static v => v % 256).SubscribeConsume(Consumer);

        [Benchmark]
        public void MaxBy() => Observable.Range(1, N).MaxBy(static v => v % 256).SubscribeConsume(Consumer);
    }
}
