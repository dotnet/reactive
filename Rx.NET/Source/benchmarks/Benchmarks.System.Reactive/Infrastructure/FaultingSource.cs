// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// Faulting sources for exercising operator <b>error pathways</b>. A single shared exception instance is used
    /// so the error itself never allocates per invocation.
    /// </summary>
    public static class FaultingSource
    {
        /// <summary>The single exception instance used by every faulting source.</summary>
        public static readonly Exception BenchmarkError = new InvalidOperationException("benchmark fault");

        /// <summary>Emits <c>1..n</c> then terminates with <see cref="BenchmarkError"/> (<c>OnError</c>).</summary>
        public static IObservable<int> Faulting(int n) =>
            Observable.Range(1, n).Concat(Observable.Throw<int>(BenchmarkError));

        /// <summary>
        /// Faults the first <paramref name="faults"/> subscriptions (each emitting <c>1..n</c> first), then completes
        /// normally — so <c>Retry</c>/<c>RetryWhen</c> genuinely resubscribe that many times before succeeding.
        /// </summary>
        public static IObservable<int> FaultThenSucceed(int n, int faults)
        {
            var attempt = 0;
            return Observable.Defer(() => attempt++ < faults ? Faulting(n) : Observable.Range(1, n));
        }
    }
}
