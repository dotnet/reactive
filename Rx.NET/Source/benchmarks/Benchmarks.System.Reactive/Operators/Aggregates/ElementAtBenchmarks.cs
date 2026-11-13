// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>S1: <c>ElementAt</c>/<c>ElementAtOrDefault</c> the last index, counting through the whole stream first.</summary>
    [BenchmarkCategory("Aggregates")]
    public class ElementAtBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ElementAt() => Observable.Range(1, N).ElementAt(N - 1).SubscribeConsume(Consumer);

        [Benchmark]
        public void ElementAtOrDefault() => Observable.Range(1, N).ElementAtOrDefault(N - 1).SubscribeConsume(Consumer);
    }
}
