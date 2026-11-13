// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: the synchronous (untimed) <c>Generate</c> unfold. (The timed overload lives under Operators/Time.)</summary>
    [BenchmarkCategory("Creation")]
    public class GenerateBenchmarks : OperatorBenchmarkBase
    {
        private Func<int, bool> _belowN = default!;

        // The condition depends on N, so it is built once here: the measured region then allocates no delegate for it.
        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _belowN = i => i < n;
        }

        [Benchmark]
        public void Generate()
        {
            Observable.Generate(0, _belowN, static i => i + 1, static i => i).SubscribeConsume(Consumer);
        }
    }
}
