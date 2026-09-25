// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Single
{
    /// <summary>S1: <c>IgnoreElements</c> drops every value and forwards only the terminal notification.</summary>
    [BenchmarkCategory("Single")]
    public class IgnoreElementsBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void IgnoreElements() => Observable.Range(1, N).IgnoreElements().SubscribeConsume(Consumer);
    }
}
