// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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
                IAsyncEnumerator<T> e = _source.GetAsyncEnumerator(ctd.Token);

                async void Core()
                {
                    bool hasNext;
                    try
                    {
                        hasNext = await e.MoveNextAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        if (!ctd.Token.IsCancellationRequested)
                        {
                            observer.OnError(ex);
                            await e.DisposeAsync().ConfigureAwait(false);
                        }

                        return;
                    }

                    if (hasNext)
                    {
                        observer.OnNext(e.Current);

                        if (!ctd.Token.IsCancellationRequested)
                        {
                            Core();
                        }

                        // In case cancellation is requested, this could only have happened
                        // by disposing the returned composite disposable (see below).
                        // In that case, e will be disposed too, so there is no need to dispose e here.
                    }
                    else
                    {
                        observer.OnCompleted();
                        await e.DisposeAsync().ConfigureAwait(false);
                    }
                }

                Core();

                // REVIEW: Safety of concurrent dispose operation; fire-and-forget nature of dispose?

                return Disposable.Create(ctd, Disposable.Create(() => { e.DisposeAsync(); }));
            }
        }
    }
}
