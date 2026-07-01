// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Base class for throughput-style (S1) operator benchmarks: a standard element-count sweep and the
    /// shared <see cref="BenchmarkBase.Consumer"/> sink. BenchmarkDotNet honours <c>[Params]</c>/<c>[Benchmark]</c>
    /// declared on base classes; this type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// <c>N</c> is <b>not</b> directly comparable across classes. For linear operators it is the element/source
    /// count (work = O(N)); but the cross-map fan-out classes (SelectMany/Switch/Merge) hold total emitted work at
    /// ~1,000,000 so N is the inner-<i>subscription</i> count, and the two-source combiners build two sources of N.
    /// Read the Ratio/absolute numbers <i>within</i> a class, not across classes at the same N.
    /// </remarks>
    public abstract class OperatorBenchmarkBase : BenchmarkBase
    {
        [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
        public int N;
    }
}
