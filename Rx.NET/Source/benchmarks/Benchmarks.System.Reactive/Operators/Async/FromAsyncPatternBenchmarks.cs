// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

#pragma warning disable CS0618 // FromAsyncPattern is obsolete; benchmarked intentionally with a hand-rolled IAsyncResult.

namespace Benchmarks.System.Reactive.Operators.Async
{
    /// <summary>
    /// <c>FromAsyncPattern</c> bridges the APM (Begin/End) pattern to an observable. The common
    /// delegate-<c>BeginInvoke</c> form is unsupported on modern .NET, so this uses a hand-rolled, synchronously
    /// completing <see cref="IAsyncResult"/> (which works on every target framework).
    /// </summary>
    [BenchmarkCategory("Async")]
    public class FromAsyncPatternBenchmarks
    {
        private readonly Consumer _consumer = new();
        private Func<IObservable<int>> _invoke = default!;

        [GlobalSetup]
        public void Setup() => _invoke = Observable.FromAsyncPattern<int>(BeginSynchronously, static _ => 1);

        [Benchmark]
        public void FromAsyncPattern() => _invoke().SubscribeConsume(_consumer);

        private static IAsyncResult BeginSynchronously(AsyncCallback callback, object state)
        {
            var result = new ImmediateAsyncResult(state);
            callback?.Invoke(result);
            return result;
        }

        private sealed class ImmediateAsyncResult : IAsyncResult
        {
            private ManualResetEvent _handle;

            public ImmediateAsyncResult(object state) => AsyncState = state;

            public object AsyncState { get; }

            public WaitHandle AsyncWaitHandle => _handle ??= new ManualResetEvent(true);

            public bool CompletedSynchronously => true;

            public bool IsCompleted => true;
        }
    }
}
