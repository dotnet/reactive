// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>
    /// Subscription-cost (S4) exemplars for the scalar factories, which ignore N: <c>Return</c> (one element),
    /// <c>Empty</c> (immediate completion), <c>Throw</c> (immediate error). Measures the per-subscription
    /// construct + terminate cost.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class ScalarCreationBenchmarks
    {
        private readonly Consumer _consumer = new();
        private readonly Exception _error = new InvalidOperationException("benchmark");

        [Benchmark]
        public void Return() => Observable.Return(1).SubscribeConsume(_consumer);

        [Benchmark]
        public void Empty() => Observable.Empty<int>().SubscribeConsume(_consumer);

        [Benchmark]
        public void Throw() => Observable.Throw<int>(_error).SubscribeConsume(_consumer);
    }
}
