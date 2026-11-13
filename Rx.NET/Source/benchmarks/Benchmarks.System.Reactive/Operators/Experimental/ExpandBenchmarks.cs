// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Experimental
{
    /// <summary>
    /// <c>ObservableEx.Expand</c>: recursively expands each element through a selector, breadth-first, on a scheduler.
    /// The selector maps <c>v</c> to <c>Return(v + 1)</c> until N and then to <c>Empty</c>, so a single seed unfolds
    /// into a chain of exactly N elements and terminates. Every step costs a scheduled work item, a queue round-trip
    /// and a fresh inner subscription (with a lock and a <c>CompositeDisposable</c> add/remove), so per-element cost is
    /// far above a plain operator. N is therefore capped at 100,000 (1,000,000 would take seconds per invocation).
    /// The trampolining <see cref="CurrentThreadScheduler"/> (the operator's default) is passed explicitly, keeping it
    /// synchronous and deterministic.
    /// </summary>
    [BenchmarkCategory("Experimental")]
    public class ExpandBenchmarks : BenchmarkBase
    {
        [Params(1, 1_000, 100_000)]
        public int N;

        [Benchmark]
        public void Expand()
        {
            var n = N;
            Observable.Return(1)
                .Expand(v => v < n ? Observable.Return(v + 1) : Observable.Empty<int>(), CurrentThreadScheduler.Instance)
                .SubscribeConsume(Consumer);
        }
    }
}
