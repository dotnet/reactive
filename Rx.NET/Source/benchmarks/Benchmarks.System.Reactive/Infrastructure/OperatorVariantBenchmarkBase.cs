// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Base class for synchronous throughput (S1) classes that cover <b>overload variants</b> of an operator whose
    /// primary overload is already swept in full by an <see cref="OperatorBenchmarkBase"/> class. It uses a reduced
    /// three-point sweep (N = 1, 1,000, 1,000,000) instead of the seven-point one: N=1 shows per-subscription cost,
    /// N=1,000,000 shows steady-state per-element cost, and 1,000 sits in between. That keeps the many variant
    /// classes affordable to run while still catching scaling regressions. This type is <c>abstract</c> so
    /// auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// As with <see cref="OperatorBenchmarkBase"/>, <c>N</c> is not comparable across classes: for cross-map fan-out
    /// classes it is the inner-subscription count with total work pinned, which each class's summary states.
    /// </remarks>
    public abstract class OperatorVariantBenchmarkBase : BenchmarkBase
    {
        [Params(1, 1_000, 1_000_000)]
        public int N;
    }
}
