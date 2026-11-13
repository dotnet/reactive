// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>S1: <c>AsAsyncObservable</c>, the identity-hiding wrapper: one extra observer hop per element.</summary>
    [BenchmarkCategory("Single")]
    public class AsAsyncObservableBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask AsAsyncObservable() =>
            BenchmarkSources.Range(N).AsAsyncObservable().SubscribeConsumeAsync(Consumer);
    }
}
