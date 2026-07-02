// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

#pragma warning disable CS0618 // Observable.ForEach is obsolete (callers are steered to async); benchmarked intentionally.

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>Blocking: <c>ForEach</c> invokes a callback per element and blocks until the sequence completes.</summary>
    [BenchmarkCategory("Blocking")]
    public class ForEachBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void ForEach() => Observable.Range(1, N).ForEach(v => Consumer.Consume(v));
    }
}
