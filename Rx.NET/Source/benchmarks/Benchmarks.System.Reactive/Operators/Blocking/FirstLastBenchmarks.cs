// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

#pragma warning disable CS0618 // The blocking First/Last are obsolete (callers are steered to *Async); benchmarked intentionally.

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// Blocking: <c>First</c> (returns as soon as the first element arrives) and <c>Last</c> (drains the whole
    /// sequence). Both block the calling thread; the synchronous source resolves them without real waiting.
    /// </summary>
    [BenchmarkCategory("Blocking")]
    public class FirstLastBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public int First() => Observable.Range(1, N).First();

        [Benchmark]
        public int Last() => Observable.Range(1, N).Last();
    }
}
