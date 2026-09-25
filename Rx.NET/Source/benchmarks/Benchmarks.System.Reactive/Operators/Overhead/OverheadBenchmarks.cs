// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Overhead
{
    /// <summary>
    /// The suite's reference point: the same "increment each element" work done as a raw <c>for</c> loop
    /// (baseline), as LINQ-to-Objects, and as an Rx <c>Select</c> pipeline. The Ratio column then shows the
    /// per-element cost of Rx relative to a hand loop and to <c>IEnumerable</c> — the "cost of Rx" figure the
    /// per-operator absolute numbers can't give on their own.
    /// </summary>
    [BenchmarkCategory("Overhead")]
    public class OverheadBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public void ForLoop()
        {
            var n = N;
            for (var i = 1; i <= n; i++)
            {
                Consumer.Consume(i + 1);
            }
        }

        [Benchmark]
        public void Enumerable_Select()
        {
            foreach (var v in Enumerable.Range(1, N).Select(static v => v + 1))
            {
                Consumer.Consume(v);
            }
        }

        [Benchmark]
        public void Rx_Select() => Observable.Range(1, N).Select(static v => v + 1).SubscribeConsume(Consumer);
    }
}
