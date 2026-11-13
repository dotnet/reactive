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
    /// S1: the element operators. First and FirstOrDefault stop at the first element; Last and LastOrDefault scan
    /// everything. Single and SingleOrDefault fault (more than one element) once N &gt; 1, which the consuming sink
    /// absorbs (they pass <c>allowErrors: true</c>), so those rows measure the error path for N &gt; 1. The
    /// AsyncRx <c>*Async</c> aliases forward to these and are not measured separately.
    /// </summary>
    /// <remarks>
    /// Design note: First, FirstOrDefault, Single and SingleOrDefault terminate before their source does, so they
    /// are driven by <see cref="Pump.RunUntilDoneAsync{TResult}"/>, which pushes into a hot source and stops once
    /// the sink has terminated, the way a producer that honours termination would. A cold source that pushes
    /// inside <c>SubscribeAsync</c> cannot be cancelled before it has returned its subscription, so it would push
    /// its remaining elements into an observer that drops them, and the rows would no longer do constant work.
    /// </remarks>
    [BenchmarkCategory("Aggregates")]
    public class FirstLastSingleBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask First() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.First());

        [Benchmark]
        public ValueTask FirstOrDefault() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.FirstOrDefault());

        [Benchmark]
        public ValueTask Last() =>
            BenchmarkSources.Range(N).Last().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask LastOrDefault() =>
            BenchmarkSources.Range(N).LastOrDefault().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Single() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.Single(), allowErrors: true);

        [Benchmark]
        public ValueTask SingleOrDefault() =>
            Pump.RunUntilDoneAsync(N, Consumer, static s => s.SingleOrDefault(), allowErrors: true);
    }
}
