// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// The shared cold sources for throughput (S1) benchmarks.
    /// </summary>
    /// <remarks>
    /// AsyncRx's <c>Range(start, count)</c> defaults to <see cref="TaskPoolAsyncScheduler.Default"/>, which moves
    /// the whole push loop to the thread pool. With <see cref="ImmediateAsyncScheduler.Instance"/> the scheduler
    /// awaits the action inline, so <see cref="Range(int)"/> pushes every element on the subscribing thread
    /// inside <c>SubscribeAsync</c>. That keeps the source deterministic and free of thread-pool noise. The
    /// default-scheduler path has its own rows in <c>RangeBenchmarks</c>.
    /// </remarks>
    public static class BenchmarkSources
    {
        /// <summary>Cold, deterministic: pushes <c>1..n</c> inline on the subscribing thread, then completes.</summary>
        public static IAsyncObservable<int> Range(int n) =>
            AsyncObservable.Range(1, n, ImmediateAsyncScheduler.Instance);

        /// <summary>As <see cref="Range(int)"/> but starting at <paramref name="start"/>.</summary>
        public static IAsyncObservable<int> Range(int start, int n) =>
            AsyncObservable.Range(start, n, ImmediateAsyncScheduler.Instance);

        /// <summary>A single value on the immediate scheduler.</summary>
        public static IAsyncObservable<T> Return<T>(T value) =>
            AsyncObservable.Return(value, ImmediateAsyncScheduler.Instance);
    }
}
