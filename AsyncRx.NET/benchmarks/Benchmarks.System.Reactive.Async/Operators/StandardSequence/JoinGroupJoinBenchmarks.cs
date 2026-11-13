// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// The window-join operators, run in virtual time so the overlap is controlled: each element's window is a
    /// virtual <c>Timer</c> spanning about two arrivals, so the output is about O(N) pairs rather than the O(N²)
    /// that open-ended windows would produce. Both sources are pushed in step.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class JoinGroupJoinBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Join() =>
            RunTimedPairAsync(0, static (left, right, scheduler, _) =>
                left.Join(
                    right,
                    _ => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 2), scheduler),
                    _ => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 2), scheduler),
                    static (l, r) => l + r));

        [Benchmark]
        public ValueTask GroupJoin() =>
            RunTimedPairAsync(0, static (left, right, scheduler, _) =>
                left.GroupJoin(
                    right,
                    _ => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 2), scheduler),
                    _ => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 2), scheduler),
                    static (l, rights) => rights)
                .SelectMany(static rights => rights));
    }
}
