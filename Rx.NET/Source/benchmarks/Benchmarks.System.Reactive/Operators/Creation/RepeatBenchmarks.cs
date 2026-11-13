// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Creation
{
    /// <summary>S1: <c>Repeat(value, count)</c> emits a constant N times.</summary>
    [BenchmarkCategory("Creation")]
    public class RepeatBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void Repeat() => Observable.Repeat(1, N).SubscribeConsume(Consumer);
    }
}
