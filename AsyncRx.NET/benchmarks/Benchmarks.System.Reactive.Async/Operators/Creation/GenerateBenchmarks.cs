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
    /// S1: <c>Generate</c> on the immediate scheduler with synchronous and <c>ValueTask</c> callbacks. The
    /// synchronous overload wraps each callback in a <c>ValueTask</c> internally. The condition captures N, so one
    /// closure is allocated per invocation.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class GenerateBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Generate()
        {
            return AsyncObservable.Generate(0, i => i < N, static i => i + 1, static i => i, ImmediateAsyncScheduler.Instance)
                .SubscribeConsumeAsync(Consumer);
        }

        [Benchmark]
        public ValueTask Generate_Async()
        {
            return AsyncObservable.Generate(
                    0,
                    i => new ValueTask<bool>(i < N),
                    static i => new ValueTask<int>(i + 1),
                    static i => new ValueTask<int>(i),
                    ImmediateAsyncScheduler.Instance)
                .SubscribeConsumeAsync(Consumer);
        }
    }
}
