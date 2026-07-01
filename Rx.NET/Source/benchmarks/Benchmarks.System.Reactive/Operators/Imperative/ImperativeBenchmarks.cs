// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Imperative
{
    /// <summary>
    /// S1 imperative combinators: <c>For</c> (concatenate a projected sequence per source item), and the
    /// condition-driven loops <c>While</c> / <c>DoWhile</c>. (<c>If</c> is covered by IfBenchmarks.)
    /// </summary>
    [BenchmarkCategory("Imperative")]
    public class ImperativeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void For() =>
            Observable.For(Enumerable.Range(1, N), static i => Observable.Return(i)).SubscribeConsume(Consumer);

        [Benchmark]
        public void While()
        {
            var i = 0;
            var n = N;
            Observable.While(() => i++ < n, Observable.Return(1)).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void DoWhile()
        {
            var i = 0;
            var n = N;
            Observable.DoWhile(Observable.Return(1), () => ++i < n).SubscribeConsume(Consumer);
        }
    }
}
