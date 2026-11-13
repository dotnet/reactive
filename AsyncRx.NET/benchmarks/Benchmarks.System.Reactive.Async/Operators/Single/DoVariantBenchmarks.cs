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
    /// S1 (variant sweep): the one- and two-callback <c>Do</c> overloads that <see cref="DoFinallyBenchmarks"/>
    /// does not cover: completion only (<c>Action</c> and <c>Func&lt;ValueTask&gt;</c>), next plus completion
    /// (synchronous and <c>ValueTask</c>), and next plus error. The callbacks are cached in static fields typed
    /// as the exact delegate each overload takes, so overload resolution cannot pick a sibling and nothing
    /// allocates per invocation except the operator.
    /// </summary>
    [BenchmarkCategory("Single")]
    public class DoVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly Action OnCompleted = static () => { };
        private static readonly Func<ValueTask> OnCompletedAsync = static () => default;
        private static readonly Action<int> OnNext = static _ => { };
        private static readonly Func<int, ValueTask> OnNextAsync = static _ => default;
        private static readonly Action<Exception> OnError = static _ => { };

        [Benchmark]
        public ValueTask Do_OnCompleted() =>
            BenchmarkSources.Range(N).Do(OnCompleted).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_OnCompleted_Async() =>
            BenchmarkSources.Range(N).Do(OnCompletedAsync).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_OnNext_OnCompleted() =>
            BenchmarkSources.Range(N).Do(OnNext, OnCompleted).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_OnNext_OnError() =>
            BenchmarkSources.Range(N).Do(OnNext, OnError).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Do_OnNext_OnCompleted_Async() =>
            BenchmarkSources.Range(N).Do(OnNextAsync, OnCompletedAsync).SubscribeConsumeAsync(Consumer);
    }
}
