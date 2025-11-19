// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
#if INCLUDE_RELOCATED_TO_INTERACTIVE_ASYNC
    public static partial class AsyncEnumerable
    {
        // Moved to AsyncEnumerableEx in System.Interactive.Async.
        // System.Linq.AsyncEnumerable has chosen not to implement this. We continue to implement this because
        // we believe it is a useful feature, but since it's now in the category of LINQ-adjacent functionality
        // not built into the .NET runtime libraries, it now lives in System.Interactive.Async.

        /// <summary>
        /// Converts an async-enumerable sequence to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new ToObservableObservable<TSource>(source);
        }

        private sealed class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> _source;

            public ToObservableObservable(IAsyncEnumerable<T> source)
            {
                _source = source;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var ctd = new CancellationTokenDisposable();

                async void Core()
                {
                    await using var e = _source.GetAsyncEnumerator(ctd.Token);
                    do
                    {
                        bool hasNext;
                        var value = default(T)!;

                        try
                        {
                            hasNext = await e.MoveNextAsync().ConfigureAwait(false);
                            if (hasNext)
                            {
                                value = e.Current;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (!ctd.Token.IsCancellationRequested)
                            {
                                observer.OnError(ex);
                            }

                            return;
                        }

                        if (!hasNext)
                        {
                            observer.OnCompleted();
                            return;
                        }

                        observer.OnNext(value);
                    }
                    while (!ctd.Token.IsCancellationRequested);
                }

                // Fire and forget
                Core();

                return ctd;
            }
        }
    }
#endif // INCLUDE_RELOCATED_TO_INTERACTIVE_ASYNC
}
