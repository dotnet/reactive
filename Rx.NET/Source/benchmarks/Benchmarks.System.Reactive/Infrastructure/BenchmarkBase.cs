// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Engines;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// The shared root for the benchmark base classes: a single <see cref="Consumer"/> sink whose
    /// dead-code-elimination contract is defined in one place. The throughput (<see cref="OperatorBenchmarkBase"/>)
    /// and temporal (<see cref="TemporalBenchmarkBase"/>) families each add their own <c>[Params] N</c> sweep on
    /// top — those sweeps deliberately differ (temporal is capped because every element is a scheduler-queue
    /// entry), so they cannot share a single <c>N</c>. This type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    public abstract class BenchmarkBase
    {
        /// <summary>The BenchmarkDotNet consumer used to defeat dead-code elimination.</summary>
        protected readonly Consumer Consumer = new();
    }
}
