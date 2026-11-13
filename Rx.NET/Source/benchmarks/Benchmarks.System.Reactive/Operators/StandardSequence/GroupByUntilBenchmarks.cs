// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>GroupByUntil</c> partitions into keyed groups that each close when a per-group duration fires
    /// (here after a few elements), so groups are repeatedly opened and closed. Groups are flattened.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByUntilBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void GroupByUntil() =>
            Observable.Range(1, N)
                .GroupByUntil(static v => v % 8, static g => g.Skip(4))
                .SelectMany(static g => g)
                .SubscribeConsume(Consumer);
    }
}
