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
    /// S1 (gated sweep): <c>Publish().RefCount()</c>: the first subscriber connects the cold source, and the
    /// subscription is disposed after the sequence ends, so the connection is released.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class RefCountBenchmarks : GatedOperatorBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_RefCount() =>
            BenchmarkSources.Range(N).Publish().RefCount().SubscribeConsumeDisposeAsync(Consumer);
    }
}
