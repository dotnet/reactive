// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace Benchmarks.System.Reactive.Async.Infrastructure
{
    /// <summary>
    /// A minimal cold source that pushes a pre-built array and completes. It feeds an operator a pre-typed
    /// sequence (for example <c>IAsyncObservable&lt;decimal&gt;</c>, boxed objects, strings, or inner observables)
    /// without a per-element <c>Select</c> conversion inside the measured region. Build the array once in
    /// <c>[GlobalSetup]</c>.
    /// </summary>
    /// <remarks>
    /// While the downstream observer completes each call synchronously, the loop stays on the fast path and
    /// allocates nothing. If a call completes asynchronously it continues in an async slow path. It ignores
    /// disposal, so it suits operators that consume the whole stream.
    /// </remarks>
    public sealed class ArrayAsyncSource<T> : IAsyncObservable<T>
    {
        private readonly T[] _items;

        public ArrayAsyncSource(T[] items) => _items = items ?? throw new ArgumentNullException(nameof(items));

        public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                var pending = observer.OnNextAsync(items[i]);
                if (!pending.IsCompletedSuccessfully)
                {
                    return SlowAsync(pending, observer, items, i + 1);
                }
            }

            return Pump.TerminateAsync(observer, null);
        }

        // The element loop stays here rather than in Pump because it walks an array of T; Pump's loops push int ranges.
        private static async ValueTask<IAsyncDisposable> SlowAsync(ValueTask pending, IAsyncObserver<T> observer, T[] items, int next)
        {
            await pending.ConfigureAwait(false);

            for (var i = next; i < items.Length; i++)
            {
                await observer.OnNextAsync(items[i]).ConfigureAwait(false);
            }

            return await Pump.TerminateAsync(observer, null).ConfigureAwait(false);
        }
    }
}
