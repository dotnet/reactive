// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: <c>Amb</c> races two sources; the synchronous <c>Range</c> wins over <c>Never</c>. <c>Amb_Three</c> is
    /// the n-ary (<c>params</c>) overload with the winner last, so two losing <c>Never</c> subscriptions are made and
    /// torn down first. (Moved here from the legacy <c>ComparisonBenchmarks</c> grid, where it was <c>AmbThree</c>.)
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class AmbBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Amb() => Observable.Range(1, N).Amb(Observable.Never<int>()).SubscribeConsume(Consumer);

        [Benchmark]
        public void Amb_Three() =>
            Observable.Amb(Observable.Never<int>(), Observable.Never<int>(), Observable.Range(1, N)).SubscribeConsume(Consumer);
    }
}
