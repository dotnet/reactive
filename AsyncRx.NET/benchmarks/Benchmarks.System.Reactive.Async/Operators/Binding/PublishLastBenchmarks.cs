// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (gated sweep): <c>PublishLast</c> (an <c>AsyncAsyncSubject</c> that keeps only the final value) with an
    /// explicit connect, and the selector form.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class PublishLastBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask PublishLast_Connect() =>
            BenchmarkSources.Range(N).PublishLast().SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask PublishLast_Selector() =>
            BenchmarkSources.Range(N).PublishLast(static s => s).SubscribeConsumeAsync(Consumer);
    }
}
