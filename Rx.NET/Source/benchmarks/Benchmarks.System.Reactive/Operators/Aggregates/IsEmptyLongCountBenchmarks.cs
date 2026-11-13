// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>S1: <c>IsEmpty</c> (short-circuits on the first element) and <c>LongCount</c> (drains the stream).</summary>
    [BenchmarkCategory("Aggregates")]
    public class IsEmptyLongCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void IsEmpty() => Observable.Range(1, N).IsEmpty().SubscribeConsume(Consumer);

        [Benchmark]
        public void LongCount() => Observable.Range(1, N).LongCount().SubscribeConsume(Consumer);
    }
}
