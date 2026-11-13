// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>Creation-category exemplar (S1): <c>Observable.Range</c> — the canonical synchronous source.</summary>
    [BenchmarkCategory("Creation")]
    public class RangeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Range() => Observable.Range(1, N).SubscribeConsume(Consumer);
    }
}
