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
    /// KNOWN GAP: both terminate on the last element, before their source completes, which throws over a cold
    /// source that pushes inside <c>SubscribeAsync</c>; they are driven by
    /// <see cref="Pump.RunUntilDoneAsync{TState, TResult}"/> instead.
    /// </remarks>
    [BenchmarkCategory("Aggregates")]
    public class ElementAtBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask ElementAt() =>
            Pump.RunUntilDoneAsync(N, Consumer, N - 1, static (s, index) => s.ElementAt(index));

        [Benchmark]
        public ValueTask ElementAtOrDefault() =>
            Pump.RunUntilDoneAsync(N, Consumer, N - 1, static (s, index) => s.ElementAtOrDefault(index));
    }
}
