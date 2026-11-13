// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (gated sweep): the time-windowed <c>Replay</c> overloads with a one-day window, so every element stays
    /// inside it: a window only, and a 16-element buffer plus a window. The replay subject delivers through its
    /// scheduler, so the immediate scheduler is used (a virtual-time scheduler would need pumping).
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class ReplayTimeBenchmarks : GatedOperatorBenchmarkBase
    {
        private static readonly TimeSpan Window = TimeSpan.FromDays(1);

        [Benchmark]
        public ValueTask Replay_Window_Connect() =>
            BenchmarkSources.Range(N).Replay(Window, ImmediateAsyncScheduler.Instance).SubscribeConnectConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Replay_BufferSize_Window_Connect() =>
            BenchmarkSources.Range(N).Replay(16, Window, ImmediateAsyncScheduler.Instance).SubscribeConnectConsumeAsync(Consumer);
    }
}
