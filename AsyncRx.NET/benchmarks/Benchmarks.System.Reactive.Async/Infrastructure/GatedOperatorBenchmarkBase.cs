// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Base class for throughput (S1) benchmarks of operators whose <b>per-element path takes an
    /// <c>AsyncGate</c></b> (Merge, Zip, CombineLatest, SelectMany, Switch, GroupBy, Window, Synchronize,
    /// the Behavior/Replay subjects, ...). It uses the same sweep as <see cref="OperatorBenchmarkBase"/>
    /// (N = 1 to 1,000,000). This type is <c>abstract</c> so auto-discovery skips it.
    /// </summary>
    /// <remarks>
    /// The type only marks a class as "takes a gate per element". <c>AsyncGate.LockAsync</c> on a free gate
    /// returns an already-completed <c>ValueTask</c> under a monitor, and the releasing thread completes a
    /// contended waiter inline, so there is no thread-pool hop per element and no reason to cap the sweep. A
    /// gated class whose N = 1,000,000 case is too slow or too large for a run derives from
    /// <see cref="BenchmarkBase"/> instead and declares its own capped <c>[Params]</c>, with a one-line reason.
    /// </remarks>
    public abstract class GatedOperatorBenchmarkBase : BenchmarkBase
    {
        [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
        public int N;
    }
}
