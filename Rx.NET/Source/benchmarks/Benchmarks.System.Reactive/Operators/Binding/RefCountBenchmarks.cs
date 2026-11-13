// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Binding
{
    /// <summary>
    /// S2: <c>Publish().RefCount()</c> connects on the first subscription and disconnects when the last drops.
    /// The single synchronous subscription connects, drains, and auto-disconnects on completion.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class RefCountBenchmarks : OperatorBenchmarkBase
    {
        [Benchmark]
        public void RefCount() => Observable.Range(1, N).Publish().RefCount().SubscribeConsume(Consumer);
    }
}
