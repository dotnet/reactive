// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>S1: <c>DefaultIfEmpty</c> passes the stream through (the non-empty source substitutes nothing).</summary>
    [BenchmarkCategory("StandardSequence")]
    public class DefaultIfEmptyBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void DefaultIfEmpty() => Observable.Range(1, N).DefaultIfEmpty().SubscribeConsume(Consumer);
    }
}
