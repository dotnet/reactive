// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>S1: <c>Aggregate</c> folds the whole stream into a single value emitted on completion.</summary>
    [BenchmarkCategory("Aggregates")]
    public class AggregateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Aggregate() => Observable.Range(1, N).Aggregate(0L, static (acc, v) => acc + v).SubscribeConsume(Consumer);
    }
}
