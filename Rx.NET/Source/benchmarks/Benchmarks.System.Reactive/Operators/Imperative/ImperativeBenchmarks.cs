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

        // The loop counter is a field, so the condition delegates capture only 'this' and no display class is
        // allocated inside the measured region.
        private int _i;

        [Benchmark]
        public void While()
        {
            _i = 0;
            Observable.While(() => _i++ < N, Observable.Return(1)).SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void DoWhile()
        {
            _i = 0;
            Observable.DoWhile(Observable.Return(1), () => ++_i < N).SubscribeConsume(Consumer);
        }
    }
}
