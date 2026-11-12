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
    /// S1 (gated sweep): <c>SelectMany</c> with total emitted work pinned at 100,000 elements. <c>N</c> is the
    /// number of inner subscriptions, each emitting <c>100,000 / N</c> elements, so the sweep moves cost from
    /// per-element to per-inner-subscription. The inner source and selector are built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyBenchmarks : GatedOperatorBenchmarkBase
    {
        private const int TotalWork = 100_000;

        private Func<int, IAsyncObservable<int>> _selector = default!;

        [GlobalSetup]
        public void Setup()
        {
            var inner = BenchmarkSources.Range(TotalWork / N);
            _selector = _ => inner;
        }

        [Benchmark]
        public ValueTask SelectMany() =>
            BenchmarkSources.Range(N).SelectMany(_selector).SubscribeConsumeAsync(Consumer);
    }
}
