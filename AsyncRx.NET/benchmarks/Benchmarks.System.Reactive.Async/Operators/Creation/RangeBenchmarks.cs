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
    /// S1: <c>Range</c> on the immediate scheduler, the cold source every other S1 benchmark uses. Its cost is the
    /// floor that the operator benchmarks build on.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class RangeBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Range() =>
            AsyncObservable.Range(1, N, ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);
    }
}
