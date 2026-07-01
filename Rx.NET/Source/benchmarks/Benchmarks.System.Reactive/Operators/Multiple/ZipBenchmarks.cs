// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: <c>Zip</c> pairs elements positionally from two sources. (Reimplemented cleanly — the old benchmark
    /// delegated to a unit test and measured its assertions.) The first source's elements queue until the
    /// second produces its match, so this also exercises Zip's internal queueing.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class ZipBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Zip() =>
            Observable.Zip(Observable.Range(1, N), Observable.Range(1, N), static (a, b) => a + b).SubscribeConsume(Consumer);
    }
}
