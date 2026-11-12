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
    /// S1 (gated sweep): <c>Replay</c> with an unbounded buffer (it retains all N), a 16-element buffer, and the
    /// selector form. The replay subject takes an <c>AsyncGate</c> per element.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class ReplayBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Replay_Connect() =>
            BenchmarkSources.Range(N).Replay().SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_BufferSize_Connect() =>
            BenchmarkSources.Range(N).Replay(16).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_Selector() =>
            BenchmarkSources.Range(N).Replay(static s => s).SubscribeConsumeAsync(Consumer);
    }
}
