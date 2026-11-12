// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>GroupBy</c> and <c>Distinct</c> with a caller-supplied <see cref="IEqualityComparer{T}"/>. The Rx sinks
    /// are the same as for the comparer-less overloads in <c>GroupByBenchmarks</c>/<c>DistinctBenchmarks</c>, but
    /// those pass <c>EqualityComparer&lt;int&gt;.Default</c>, which on .NET Core lets the underlying
    /// <c>Dictionary</c>/<c>HashSet</c> take its devirtualised default-comparer path. A custom comparer forces an
    /// interface call per lookup, which is the distinct runtime path measured here. Shapes match the originals:
    /// 8 groups flattened with <c>SelectMany</c>, and N distinct keys.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class GroupByDistinctComparerBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly IEqualityComparer<int> _comparer = new IntEqualityComparer();

        [Benchmark]
        public void GroupBy_Comparer() =>
            Observable.Range(1, N).GroupBy(static v => v % 8, _comparer).SelectMany(static g => g).SubscribeConsume(Consumer);

        [Benchmark]
        public void Distinct_Comparer() => Observable.Range(1, N).Distinct(_comparer).SubscribeConsume(Consumer);

        /// <summary>A non-default comparer with the same semantics as <c>EqualityComparer&lt;int&gt;.Default</c>.</summary>
        private sealed class IntEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y) => x == y;

            public int GetHashCode(int obj) => obj;
        }
    }
}
