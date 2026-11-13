// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Lifecycle
{
    /// <summary>
    /// Splits a pipeline's cost into construction and subscription-plus-drain. <c>_Full</c> (baseline) does
    /// everything; <c>_Create</c> only builds the operator chain and returns it unsubscribed; <c>_Subscribe</c>
    /// subscribes and drains a pipeline built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Lifecycle")]
    public class ConstructionBenchmarks : OperatorBenchmarkBase
    {
        private IAsyncObservable<int> _prebuilt = default!;

        [GlobalSetup]
        public void Setup() => _prebuilt = BenchmarkSources.Range(N).Select(static v => v + 1);

        [Benchmark(Baseline = true)]
        public ValueTask Select_Full() => BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public IAsyncObservable<int> Select_Create() => BenchmarkSources.Range(N).Select(static v => v + 1);

        [Benchmark]
        public ValueTask Select_Subscribe() => _prebuilt.SubscribeConsumeAsync(Consumer);
    }
}
