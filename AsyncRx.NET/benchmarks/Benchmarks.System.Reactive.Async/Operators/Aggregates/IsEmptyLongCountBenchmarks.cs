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
    /// S1: <c>IsEmpty</c> (stops at the first element) and <c>LongCount</c> (scans everything). <c>IsEmpty</c>
    /// terminates early, so it is driven by <see cref="Pump.RunUntilDoneAsync{TResult}"/>, a producer that stops
    /// once the sink has terminated. (A cold source cannot be cancelled before it returns its subscription, so it
    /// would push N - 1 elements that the terminated observer drops.)
    /// </summary>
    [BenchmarkCategory("Aggregates")]
    public class IsEmptyLongCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask IsEmpty() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.IsEmpty());

        [Benchmark]
        public ValueTask LongCount() =>
            BenchmarkSources.Range(N).LongCount().SubscribeConsumeAsync(Consumer);
    }
}
