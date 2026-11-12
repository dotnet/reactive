// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S1: the exception-typed handler overload <c>Catch&lt;TSource, TException&gt;(handler)</c>, which runs the
    /// dedicated <c>Catch&lt;TSource, TException&gt;</c> sink (a type test on the error, then a handler call),
    /// distinct from the source-list <c>Catch(second)</c> in <see cref="CatchBenchmarks"/>. <c>Catch_Handler</c> is
    /// the happy path (source completes, handler never runs); <c>Catch_Handler_Error</c> faults after N elements
    /// with an <see cref="InvalidOperationException"/>, so the handler is invoked and its N-element fallback is
    /// subscribed.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CatchHandlerBenchmarks : OperatorVariantBenchmarkBase
    {
        [Benchmark]
        public void Catch_Handler()
        {
            var fallback = Observable.Range(1, N);
            Observable.Range(1, N)
                .Catch((InvalidOperationException _) => fallback)
                .SubscribeConsume(Consumer);
        }

        [Benchmark]
        public void Catch_Handler_Error()
        {
            var fallback = Observable.Range(1, N);
            FaultingSource.Faulting(N)
                .Catch((InvalidOperationException _) => fallback)
                .SubscribeConsume(Consumer);
        }
    }
}
