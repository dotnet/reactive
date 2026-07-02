// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Aggregates
{
    /// <summary>S1: <c>SequenceEqual</c> compares two streams element-by-element; the equal inputs force a full scan.</summary>
    [BenchmarkCategory("Aggregates")]
    public class SequenceEqualBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void SequenceEqual() =>
            Observable.Range(1, N).SequenceEqual(Observable.Range(1, N)).SubscribeConsume(Consumer);
    }
}
