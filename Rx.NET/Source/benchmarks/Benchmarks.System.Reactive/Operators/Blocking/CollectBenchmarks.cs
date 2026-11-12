// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Blocking
{
    /// <summary>
    /// Blocking: <c>Collect</c> folds notifications into a collector that the pulling side takes on each
    /// <c>MoveNext</c>. The synchronous <c>Range</c> has already pushed all N elements into the one <c>List</c>
    /// collector by the time the enumerator is pulled, so this measures the per-element merge plus a single
    /// hand-over. (Moved here from the legacy <c>ComparisonBenchmarks</c> grid.)
    /// </summary>
    [BenchmarkCategory("Blocking")]
    public class CollectBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Collect()
        {
            foreach (var v in Observable.Range(1, N).Collect(static () => new List<int>(), static (a, b) => { a.Add(b); return a; }))
            {
                Consumer.Consume(v);
            }
        }
    }
}
