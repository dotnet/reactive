// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Aggregates
{
    /// <summary>
    /// S1: <c>ElementAt</c> and <c>ElementAtOrDefault</c> for the last element, so the whole source is scanned.
    /// </summary>
    /// <remarks>
    /// Both terminate on the last element, just before the cold source completes. The source's completion then
    /// reaches an observer that has already terminated and is dropped, so the rows use the cold <c>Range</c>
    /// shape and do no extra work.
    /// </remarks>
    [BenchmarkCategory("Aggregates")]
    public class ElementAtBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask ElementAt() =>
            BenchmarkSources.Range(N).ElementAt(N - 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ElementAtOrDefault() =>
            BenchmarkSources.Range(N).ElementAtOrDefault(N - 1).SubscribeConsumeAsync(Consumer);
    }
}
