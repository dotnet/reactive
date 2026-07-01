// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>GroupBy</c> partitions the stream into keyed sub-observables (allocating a group per key), then
    /// the groups are flattened. Fixed at 8 keys so each group carries a meaningful share of the stream.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void GroupBy() =>
            Observable.Range(1, N).GroupBy(static v => v % 8).SelectMany(static g => g).SubscribeConsume(Consumer);
    }
}
