// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>Cast</c> and <c>OfType</c> over a boxed (<c>object</c>) source. The boxing in the source is
    /// inherent to having an <c>IObservable&lt;object&gt;</c>; the benchmark measures the cast/type-test per element.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class CastOfTypeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Cast() => Observable.Range(1, N).Select(static v => (object)v).Cast<int>().SubscribeConsume(Consumer);

        [Benchmark]
        public void OfType() => Observable.Range(1, N).Select(static v => (object)v).OfType<int>().SubscribeConsume(Consumer);
    }
}
