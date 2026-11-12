// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.StandardSequence
{
    /// <summary>
    /// S1: <c>DefaultIfEmpty</c> over a non-empty source (the pass-through path that every element pays), with and
    /// without an explicit default value.
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class DefaultIfEmptyBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask DefaultIfEmpty() =>
            BenchmarkSources.Range(N).DefaultIfEmpty().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask DefaultIfEmpty_Value() =>
            BenchmarkSources.Range(N).DefaultIfEmpty(-1).SubscribeConsumeAsync(Consumer);
    }
}
