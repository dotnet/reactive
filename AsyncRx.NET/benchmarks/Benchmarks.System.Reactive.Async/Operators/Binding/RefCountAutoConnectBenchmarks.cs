// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Binding
{
    /// <summary>
    /// S1 (variant sweep): automatic connection of <c>Publish()</c> over a cold source of N.
    /// <c>RefCount(minObservers)</c> connects on the first subscriber and is disposed after the sequence ends, as
    /// <see cref="RefCountBenchmarks"/>. <c>AutoConnect</c> connects on the first subscriber and never disconnects,
    /// so each invocation builds a fresh <c>Publish()</c>; the <c>_Async</c> row passes a <c>ValueTask</c>
    /// <c>onConnect</c> callback.
    /// </summary>
    [BenchmarkCategory("Binding")]
    public class RefCountAutoConnectBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public ValueTask Publish_RefCount_MinObservers() =>
            BenchmarkSources.Range(N).Publish().RefCount(1).SubscribeConsumeDisposeAsync(Consumer);

        [Benchmark]
        public ValueTask Publish_AutoConnect() =>
            BenchmarkSources.Range(N).Publish().AutoConnect().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Publish_AutoConnect_OnConnect_Async() =>
            BenchmarkSources.Range(N).Publish().AutoConnect(1, static (IAsyncDisposable _) => default(ValueTask)).SubscribeConsumeAsync(Consumer);
    }
}
