// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Single
{
    /// <summary>
    /// S1: the side-effect operators. <c>Do</c> with an <c>Action</c>, a <c>ValueTask</c> callback, an
    /// <c>IAsyncObserver</c>, an <c>IObserver</c> and the three-callback forms; <c>Finally</c> with an
    /// <c>Action</c> and a <c>Func&lt;Task&gt;</c>. The callbacks and observers are static, so nothing allocates
    /// per invocation except the operators.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class DoFinallyBenchmarks : OperatorBenchmarkBase
    {
        private static readonly IObserver<int> SyncObserver = new NopObserver();
        private static readonly IAsyncObserver<int> AsyncObserverInstance = new NopAsyncObserver();

        [Benchmark]
        public ValueTask Do_Action() =>
            BenchmarkSources.Range(N).Do(static (int _) => { }).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_Async() =>
            BenchmarkSources.Range(N).Do(static (int _) => default(ValueTask)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_AsyncObserver() =>
            BenchmarkSources.Range(N).Do(AsyncObserverInstance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_Observer() =>
            BenchmarkSources.Range(N).Do(SyncObserver).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_ThreeActions() =>
            BenchmarkSources.Range(N).Do(static _ => { }, static _ => { }, static () => { }).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_ThreeCallbacks_Async() =>
            BenchmarkSources.Range(N).Do(static _ => default(ValueTask), static _ => default(ValueTask), static () => default(ValueTask)).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Finally_Action() =>
            BenchmarkSources.Range(N).Finally(static () => { }).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Finally_Task() =>
            BenchmarkSources.Range(N).Finally(static () => Task.CompletedTask).SubscribeConsumeAsync(Consumer);

        private sealed class NopObserver : IObserver<int>
        {
            public void OnCompleted() { }

            public void OnError(Exception error) { }

            public void OnNext(int value) { }
        }

        private sealed class NopAsyncObserver : IAsyncObserver<int>
        {
            public ValueTask OnCompletedAsync() => default;

            public ValueTask OnErrorAsync(Exception error) => default;

            public ValueTask OnNextAsync(int value) => default;
        }
    }
}
