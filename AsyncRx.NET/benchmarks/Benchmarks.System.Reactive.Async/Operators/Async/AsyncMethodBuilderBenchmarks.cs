// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Async
{
    /// <summary>
    /// Per-call cost of an <c>async</c> method that returns <c>IAsyncObservable&lt;T&gt;</c>, which the compiler
    /// builds through AsyncRx's <c>AsyncObservableMethodBuilder&lt;T&gt;</c>: the method yields once (so it
    /// completes asynchronously, through the builder's async subject), then the result is subscribed and
    /// consumed. <c>IAsyncObservable&lt;T&gt;</c> is not declared task-like, so the method names the builder with a
    /// method-level <c>[AsyncMethodBuilder]</c>.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: when the method completes synchronously (no await, or awaiting an already-completed task), the
    /// builder's <c>TaskObservable</c> emits the value but never calls <c>OnCompletedAsync</c>, so a subscriber
    /// waiting for termination hangs. That path is not measured; restore the commented-out row when the library
    /// is fixed.
    /// </remarks>
    [BenchmarkCategory("Async")]
    public class AsyncMethodBuilderBenchmarks
    {
        private readonly Consumer _consumer = new();

        // KNOWN GAP (see remarks): hangs, because the synchronous-completion path never completes.
        // [Benchmark]
        // public ValueTask AsyncMethod_Synchronous() => ReturnAsync(42).SubscribeConsumeAsync(_consumer);
        //
        // [AsyncMethodBuilder(typeof(AsyncObservableMethodBuilder<>))]
        // private static async IAsyncObservable<int> ReturnAsync(int value) => value;

        [Benchmark]
        public ValueTask AsyncMethod_Yield() => YieldAsync(42).SubscribeConsumeAsync(_consumer);

        [AsyncMethodBuilder(typeof(AsyncObservableMethodBuilder<>))]
        private static async IAsyncObservable<int> YieldAsync(int value)
        {
            await Task.Yield();
            return value;
        }
    }
}
