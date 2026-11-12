// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// S1: <c>Repeat(value, count)</c> on the immediate scheduler, and <c>source.Repeat(count)</c> resubscribing a
    /// one-element source N times (per-resubscription cost).
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class RepeatBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Repeat_Value() =>
            AsyncObservable.Repeat(42, N, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Repeat_Source() =>
            BenchmarkSources.Return(42).Repeat(N).SubscribeConsumeAsync(Consumer);
    }
}
