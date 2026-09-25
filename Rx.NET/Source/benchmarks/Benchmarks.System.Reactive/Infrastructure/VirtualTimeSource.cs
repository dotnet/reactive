// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Builders that emit "N elements spread across a virtual timeline" for temporal (S3) benchmarks.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Observable.Generate{TState, TResult}(TState, Func{TState, bool}, Func{TState, TState}, Func{TState, TResult}, Func{TState, TimeSpan}, IScheduler)"/>
    /// rather than the recording <c>TestScheduler.CreateHotObservable</c>, so the only allocations in the
    /// measured region belong to the operator under test (Generate keeps a single self-rescheduling item).
    /// Driven by a virtual scheduler (e.g. <see cref="HistoricalScheduler"/>), <c>Start()</c> drains the whole
    /// timeline synchronously with zero wall-clock waiting.
    /// </remarks>
    public static class VirtualTimeSource
    {
        /// <summary>
        /// Emits <paramref name="n"/> ascending integers on <paramref name="scheduler"/>, each spaced
        /// <paramref name="step"/> of virtual time apart, followed by <c>OnCompleted</c>.
        /// </summary>
        public static IObservable<int> Timed(IScheduler scheduler, int n, TimeSpan step) =>
            Observable.Generate(0, i => i < n, i => i + 1, i => i, _ => step, scheduler);

        /// <summary>
        /// As <see cref="Timed(IScheduler, int, TimeSpan)"/> but the first element is delayed by
        /// <paramref name="offset"/> (subsequent elements <paramref name="step"/> apart) — used to interleave two
        /// sources on a shared clock.
        /// </summary>
        public static IObservable<int> Timed(IScheduler scheduler, int n, TimeSpan step, TimeSpan offset) =>
            Observable.Generate(0, i => i < n, i => i + 1, i => i, i => i == 0 ? offset : step, scheduler);
    }
}
