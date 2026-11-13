// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Multiple
{
    /// <summary>
    /// S1 (variant sweep): <c>Catch&lt;TSource, TException&gt;(handler)</c>, where the source faults after N
    /// elements and the handler returns a recovery source of N. Synchronous and <c>ValueTask</c> handlers. The
    /// handlers are built in <c>[GlobalSetup]</c>.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class CatchHandlerBenchmarks : OperatorVariantBenchmarkBase
    {
        private Func<InvalidOperationException, IAsyncObservable<int>> _handler = default!;
        private Func<InvalidOperationException, ValueTask<IAsyncObservable<int>>> _handlerAsync = default!;

        [GlobalSetup]
        public void Setup()
        {
            var recovery = BenchmarkSources.Range(N);
            _handler = _ => recovery;
            _handlerAsync = _ => new ValueTask<IAsyncObservable<int>>(recovery);
        }

        [Benchmark]
        public ValueTask Catch_Handler() =>
            FaultingSource.Faulting(N).Catch(_handler).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Catch_Handler_Async() =>
            FaultingSource.Faulting(N).Catch(_handlerAsync).SubscribeConsumeAsync(Consumer);
    }
}
