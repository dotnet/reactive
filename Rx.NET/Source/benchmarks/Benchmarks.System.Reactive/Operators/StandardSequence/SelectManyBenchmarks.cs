// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// S1 fan-out: <c>SelectMany</c> projects each element to an inner sequence and flattens. The cross-map inner
    /// size (<c>1,000,000 / N</c>) holds total emitted work ~constant, isolating the per-inner-subscription cost.
    /// The inner size is precomputed in <c>[GlobalSetup]</c> so the selector captures a field (no per-invocation
    /// display-class closure).
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class SelectManyBenchmarks : OperatorBenchmarkBase
    {
        private int _innerSize;

        [GlobalSetup]
        public void Setup() => _innerSize = 1_000_000 / N;

        [Benchmark]
        public void SelectMany() =>
            Observable.Range(1, N).SelectMany(v => Observable.Range(v, _innerSize)).SubscribeConsume(Consumer);
    }
}
