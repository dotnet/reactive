// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: <c>Switch</c> flattens a stream of inner sequences, always following the most recent. Cross-mapped so
    /// total emitted work stays ~constant across N (inner size precomputed to avoid a closure). The inners run on
    /// <c>ImmediateScheduler</c>, so each one drains fully inside the outer <c>OnNext</c> before the next switch.
    /// On the default <c>CurrentThreadScheduler</c> the inner and outer <c>Range</c> would interleave on the
    /// trampoline, each outer value would cancel the previous inner after about one element, and the workload
    /// would shrink as N grows. The mid-inner cancellation path is covered by
    /// <c>InterleavedCombiningBenchmarks</c>.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class SwitchBenchmarks : OperatorBenchmarkBase
    {
        private int _innerSize;

        [GlobalSetup]
        public void Setup() => _innerSize = 1_000_000 / N;

        [Benchmark]
        public void Switch() =>
            Observable.Range(1, N).Select(v => Observable.Range(v, _innerSize, ImmediateScheduler.Instance)).Switch().SubscribeConsume(Consumer);
    }
}
