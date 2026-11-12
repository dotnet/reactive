// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// Faulting sources for operator <b>error pathways</b>. They are hand-written cold sources rather than
    /// <c>Concat(Throw)</c> or <c>Defer</c> compositions, so only the operator under test is measured. A single
    /// shared exception instance is used, so the error never allocates per invocation.
    /// </summary>
    public static class FaultingSource
    {
        /// <summary>The single exception instance used by every faulting source.</summary>
        public static readonly Exception BenchmarkError = new InvalidOperationException("benchmark fault");

        /// <summary>Emits <c>1..n</c>, then terminates with <see cref="BenchmarkError"/>.</summary>
        public static IAsyncObservable<int> Faulting(int n) => new CountingSource(n, fault: true);

        /// <summary>
        /// Faults the first <paramref name="faults"/> subscriptions (each emitting <c>1..n</c> first), then
        /// emits <c>1..n</c> and completes, so <c>Retry</c> resubscribes that many times before it succeeds. The
        /// attempt counter is per instance, so build it inside the benchmark method.
        /// </summary>
        public static IAsyncObservable<int> FaultThenSucceed(int n, int faults) => new FaultThenSucceedSource(n, faults);

        private sealed class FaultThenSucceedSource : IAsyncObservable<int>
        {
            private readonly int _n;
            private readonly int _faults;
            private int _attempt;

            public FaultThenSucceedSource(int n, int faults)
            {
                _n = n;
                _faults = faults;
            }

            public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<int> observer) =>
                CountingSource.Run(observer, _n, _attempt++ < _faults);
        }

        private sealed class CountingSource : IAsyncObservable<int>
        {
            private readonly int _n;
            private readonly bool _fault;

            public CountingSource(int n, bool fault)
            {
                _n = n;
                _fault = fault;
            }

            public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<int> observer) => Run(observer, _n, _fault);

            public static ValueTask<IAsyncDisposable> Run(IAsyncObserver<int> observer, int n, bool fault)
            {
                for (var i = 1; i <= n; i++)
                {
                    var pending = observer.OnNextAsync(i);
                    if (!pending.IsCompletedSuccessfully)
                    {
                        return SlowAsync(pending, observer, i + 1, n, fault);
                    }
                }

                var terminal = fault ? observer.OnErrorAsync(BenchmarkError) : observer.OnCompletedAsync();
                if (!terminal.IsCompletedSuccessfully)
                {
                    return AwaitAsync(terminal);
                }

                return new ValueTask<IAsyncDisposable>(AsyncDisposable.Nop);
            }

            private static async ValueTask<IAsyncDisposable> SlowAsync(ValueTask pending, IAsyncObserver<int> observer, int next, int n, bool fault)
            {
                await pending.ConfigureAwait(false);

                for (var i = next; i <= n; i++)
                {
                    await observer.OnNextAsync(i).ConfigureAwait(false);
                }

                if (fault)
                {
                    await observer.OnErrorAsync(BenchmarkError).ConfigureAwait(false);
                }
                else
                {
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }

                return AsyncDisposable.Nop;
            }

            private static async ValueTask<IAsyncDisposable> AwaitAsync(ValueTask terminal)
            {
                await terminal.ConfigureAwait(false);
                return AsyncDisposable.Nop;
            }
        }
    }
}
