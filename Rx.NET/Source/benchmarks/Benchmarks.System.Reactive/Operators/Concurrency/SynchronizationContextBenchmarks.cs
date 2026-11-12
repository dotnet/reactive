// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Concurrency
{
    /// <summary>
    /// S1: the <see cref="SynchronizationContext"/> overloads of <c>ObserveOn</c> and <c>SubscribeOn</c>, which use
    /// their own code paths (<c>ObserveOn.Context</c>, which calls <c>Post</c> once per notification, and
    /// <c>SubscribeOnCtxObservable</c>) rather than the <c>IScheduler</c> paths in <see cref="ObserveOnBenchmarks"/>/
    /// <see cref="SubscribeOnBenchmarks"/>. A private context whose <c>Post</c> runs the callback inline keeps the
    /// benchmark synchronous and deterministic (no thread hop), so it measures Rx's per-notification marshalling
    /// overhead, not a real UI-thread hand-off.
    /// </summary>
    [BenchmarkCategory("Concurrency")]
    public class SynchronizationContextBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly SynchronizationContext _context = new InlineSynchronizationContext();

        [Benchmark]
        public void ObserveOn_Context() => Observable.Range(1, N).ObserveOn(_context).SubscribeConsume(Consumer);

        [Benchmark]
        public void SubscribeOn_Context() => Observable.Range(1, N).SubscribeOn(_context).SubscribeConsume(Consumer);

        /// <summary>A synchronization context that runs every posted or sent callback inline on the calling thread.</summary>
        private sealed class InlineSynchronizationContext : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state) => d(state);

            public override void Send(SendOrPostCallback d, object state) => d(state);

            public override SynchronizationContext CreateCopy() => this;
        }
    }
}
