// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Creation
{
    /// <summary>
    /// S1 (variant sweep): the two <c>AsyncObservable.Create</c> forms that <c>CreateBenchmarks</c> does not
    /// cover. <c>Create_Action</c> is the auto-completing form (the callback returns <see cref="ValueTask"/>,
    /// and <c>Create</c> sends <c>OnCompleted</c> when it finishes); <c>Create_DisposeFunc</c> returns a
    /// <c>Func&lt;ValueTask&gt;</c> that <c>Create</c> wraps in a disposable. Both observables are built once
    /// and push 0..N-1 inline.
    /// </summary>
    [BenchmarkCategory("Creation")]
    public class CreateVariantBenchmarks : OperatorVariantBenchmarkBase
    {
        private static readonly Func<ValueTask> NopDispose = static () => default;

        private IAsyncObservable<int> _action = default!;
        private IAsyncObservable<int> _disposeFunc = default!;

        [GlobalSetup]
        public void Setup()
        {
            var n = N;

            Func<IAsyncObserver<int>, CancellationToken, ValueTask> action = async (observer, _) =>
            {
                for (var i = 0; i < n; i++)
                {
                    await observer.OnNextAsync(i).ConfigureAwait(false);
                }

                // No OnCompletedAsync: this form completes the sequence itself when the callback returns.
            };
            _action = AsyncObservable.Create(action);

            Func<IAsyncObserver<int>, CancellationToken, ValueTask<Func<ValueTask>>> disposeFunc = async (observer, _) =>
            {
                for (var i = 0; i < n; i++)
                {
                    await observer.OnNextAsync(i).ConfigureAwait(false);
                }

                await observer.OnCompletedAsync().ConfigureAwait(false);
                return NopDispose;
            };
            _disposeFunc = AsyncObservable.Create(disposeFunc);
        }

        [Benchmark]
        public ValueTask Create_Action() => _action.SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Create_DisposeFunc() => _disposeFunc.SubscribeConsumeAsync(Consumer);
    }
}
