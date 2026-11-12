// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;

namespace Benchmarks.System.Reactive.Infrastructure
{
    /// <summary>
    /// A minimal synchronous cold source that pushes a pre-built array and completes. It lets benchmarks feed an
    /// operator a <i>pre-typed</i> sequence (e.g. <c>IObservable&lt;decimal&gt;</c>) so the operator's type-specific
    /// overload is hit, without putting a per-element <c>Select</c> conversion (or <c>ToObservable</c>'s scheduler
    /// loop) inside the measured region. The array is built once in <c>[GlobalSetup]</c>.
    /// </summary>
    /// <remarks>
    /// Like <c>Observable.Range</c> on the immediate path, it runs to completion inside <c>Subscribe</c> and ignores
    /// disposal, so it suits operators that consume the whole stream (aggregates). Not for operators that
    /// unsubscribe early.
    /// </remarks>
    public sealed class ArraySource<T> : IObservable<T>
    {
        private readonly T[] _items;

        public ArraySource(T[] items) => _items = items ?? throw new ArgumentNullException(nameof(items));

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                observer.OnNext(items[i]);
            }

            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
