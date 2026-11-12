// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1 (variant sweep): <c>SequenceEqual</c> against an <c>IEnumerable</c> second sequence, with and without
    /// a comparer. The second sequence is an <c>int[]</c> holding <c>1..N</c>, built once in
    /// <c>[GlobalSetup]</c> and held in a field typed <c>IEnumerable&lt;int&gt;</c>, so the sequences are equal
    /// and every row compares all <c>N</c> elements.
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class SequenceEqualEnumerableBenchmarks : OperatorVariantBenchmarkBase
    {
        private IEnumerable<int> _second = default!;

        [GlobalSetup]
        public void Setup()
        {
            var items = new int[N];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = i + 1;
            }

            _second = items;
        }

        [Benchmark]
        public ValueTask SequenceEqual_Enumerable() =>
            BenchmarkSources.Range(N).SequenceEqual(_second).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SequenceEqual_Enumerable_Comparer() =>
            BenchmarkSources.Range(N).SequenceEqual(_second, EqualityComparer<int>.Default).SubscribeConsumeAsync(Consumer);
    }
}
