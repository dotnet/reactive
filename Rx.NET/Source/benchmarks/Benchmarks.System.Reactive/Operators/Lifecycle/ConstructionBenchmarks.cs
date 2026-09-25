// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Lifecycle
{
    /// <summary>
    /// Decomposes a pipeline's cost into construction vs subscription+drain (the legacy Prepend_Create/_Subscribe
    /// technique). <c>_Full</c> (baseline) does everything; <c>_Create</c> only builds the operator chain (returns
    /// it unsubscribed); <c>_Subscribe</c> subscribes and drains a pipeline pre-built in <c>[GlobalSetup]</c>. The
    /// Ratio isolates producer-construction from per-subscription sink allocation + per-element cost.
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class ConstructionBenchmarks : OperatorBenchmarkBase
    {
        private IObservable<int> _prebuilt = default!;

        [GlobalSetup]
        public void Setup() => _prebuilt = Observable.Range(1, N).Select(static v => v + 1);

        [Benchmark(Baseline = true)]
        public void Select_Full() => Observable.Range(1, N).Select(static v => v + 1).SubscribeConsume(Consumer);

        [Benchmark]
        public IObservable<int> Select_Create() => Observable.Range(1, N).Select(static v => v + 1);

        [Benchmark]
        public void Select_Subscribe() => _prebuilt.SubscribeConsume(Consumer);
    }
}
