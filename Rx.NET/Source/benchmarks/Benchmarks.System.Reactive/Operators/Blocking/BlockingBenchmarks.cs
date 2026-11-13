// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// Blocking-category exemplar: <c>Wait</c> blocks the calling thread until the (finite) sequence completes
    /// and returns its last element. Driven by a synchronous <c>Range</c>, so it completes without real waiting.
    /// </summary>
    [BenchmarkCategory("Blocking")]
    public class BlockingBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public int Wait() => Observable.Range(1, N).Wait();
    }
}
