// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>S1: <c>IgnoreElements</c>, which drops every value and forwards only termination.</summary>
    [BenchmarkCategory("Single")]
    public class IgnoreElementsBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask IgnoreElements() =>
            BenchmarkSources.Range(N).IgnoreElements().SubscribeConsumeAsync(Consumer);
    }
}
