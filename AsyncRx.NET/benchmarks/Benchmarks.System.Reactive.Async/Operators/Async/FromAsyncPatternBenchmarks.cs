// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Async
{
    /// <summary>
    /// Per-call cost of the APM bridge <c>FromAsyncPattern</c> at arity 0 and 2, with a <c>Begin</c> method that
    /// completes synchronously (it invokes the callback inline with a pre-completed <see cref="IAsyncResult"/>).
    /// Each call creates an <c>AsyncAsyncSubject</c>.
    /// </summary>
    [BenchmarkCategory("Async")]
    public class FromAsyncPatternBenchmarks
    {
        private readonly Consumer _consumer = new();
        private Func<IAsyncObservable<int>> _invoke0 = default!;
        private Func<int, int, IAsyncObservable<int>> _invoke2 = default!;

        [GlobalSetup]
        public void Setup()
        {
            _invoke0 = AsyncObservable.FromAsyncPattern<int>(BeginSynchronously, static _ => 1);
            _invoke2 = AsyncObservable.FromAsyncPattern<int, int, int>(static (a, b, callback, state) => BeginSynchronously(callback, state), static _ => 3);
        }

        [Benchmark(Baseline = true)]
        public ValueTask FromAsyncPattern_Arity0() => _invoke0().SubscribeConsumeAsync(_consumer);

        [Benchmark]
        public ValueTask FromAsyncPattern_Arity2() => _invoke2(1, 2).SubscribeConsumeAsync(_consumer);

        private static IAsyncResult BeginSynchronously(AsyncCallback callback, object? state)
        {
            var result = new ImmediateAsyncResult(state);
            callback?.Invoke(result);
            return result;
        }

        private sealed class ImmediateAsyncResult : IAsyncResult
        {
            private ManualResetEvent? _handle;

            public ImmediateAsyncResult(object? state) => AsyncState = state;

            public object? AsyncState { get; }

            public WaitHandle AsyncWaitHandle => _handle ??= new ManualResetEvent(true);

            public bool CompletedSynchronously => true;

            public bool IsCompleted => true;
        }
    }
}
