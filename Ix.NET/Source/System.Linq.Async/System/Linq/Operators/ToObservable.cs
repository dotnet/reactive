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
                throw new ArgumentNullException(nameof(source));

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
                var e = _source.GetAsyncEnumerator(ctd.Token);

                var f = default(Action);
                f = () => e.MoveNextAsync().AsTask().ContinueWith(
                    async t =>
                    {
                        if (t.IsFaulted)
                        {
                            observer.OnError(t.Exception);
                            await e.DisposeAsync().ConfigureAwait(false);
                        }
                        else if (t.IsCanceled)
                        {
                            await e.DisposeAsync().ConfigureAwait(false);
                        }
                        else if (t.IsCompleted)
                        {
                            if (t.Result)
                            {
                                observer.OnNext(e.Current);

                                if (!ctd.Token.IsCancellationRequested)
                                {
                                    f();
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
                    }, ctd.Token);

                f();

                return Disposable.Create(ctd, Disposable.Create(() => { e.DisposeAsync(); /* REVIEW: fire-and-forget? */ }));
            }
        }
    }
}
