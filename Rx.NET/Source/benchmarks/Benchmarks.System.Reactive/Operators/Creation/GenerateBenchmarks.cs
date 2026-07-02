// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: the synchronous (untimed) <c>Generate</c> unfold. (The timed overload lives under Operators/Time.)</summary>
    [BenchmarkCategory("Creation")]
    public class GenerateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Generate()
        {
            var n = N;
            Observable.Generate(0, i => i < n, static i => i + 1, static i => i).SubscribeConsume(Consumer);
        }
    }
}
