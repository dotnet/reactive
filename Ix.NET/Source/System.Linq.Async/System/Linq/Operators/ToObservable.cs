// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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

            return new ToObservableObservable<TSource>(source, false);
        }


        /// <summary>
        /// Converts an async-enumerable sequence to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <param name="ignoreExceptionsAfterUnsubscribe">If this is <c>true</c>, exceptions that occur after all observers have unsubscribed will be handled and silently ignored.
        /// If <c>false</c>, they will go unobserved, meaning they will eventually emerge through <see cref="TaskScheduler.UnobservedTaskException"/></param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ToObservable<TSource>(this IAsyncEnumerable<TSource> source, bool ignoreExceptionsAfterUnsubscribe)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new ToObservableObservable<TSource>(source, ignoreExceptionsAfterUnsubscribe);
        }


        private sealed class ToObservableObservable<T> : IObservable<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private readonly bool _ignoreExceptionsAfterUnsubscribe;

            public ToObservableObservable(IAsyncEnumerable<T> source, bool ignoreExceptionsAfterUnsubscribe)
            {
                _source = source;
                _ignoreExceptionsAfterUnsubscribe = ignoreExceptionsAfterUnsubscribe;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var ctd = new CancellationTokenDisposable();

                async ValueTask Core()
                {
                    IAsyncEnumerator<T> e;

                    try
                    {
                        e = _source.GetAsyncEnumerator(ctd.Token);
                    }
                    catch (Exception ex)
                    {
                        if (!ctd.Token.IsCancellationRequested)
                        {
                            observer.OnError(ex);
                        }

                        return;
                    }


                    try
                    {
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
                        } while (!ctd.Token.IsCancellationRequested);
                    }
                    finally
                    {
                        if (_ignoreExceptionsAfterUnsubscribe)
                        {
                            try
                            {
                                await e.DisposeAsync().ConfigureAwait(false);
                            }
                            catch
                            {
                                // Ignored
                            }
                        }
                        else
                        {
                            // Exceptions will go in TaskScheduler.UnobservedTaskException.
                            // This behavior is similar to Observable.FromAsync

                            await e.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }

                // Fire and forget
                _ = Core();

                return ctd;
            }
        }
    }
}
