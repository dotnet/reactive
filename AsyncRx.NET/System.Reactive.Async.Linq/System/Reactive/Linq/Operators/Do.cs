// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, observer)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onError)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onCompleted)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext, onError, onCompleted)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, IAsyncObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, observer)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<Exception, Task> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onError)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<Task> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onCompleted)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create<TSource>(target => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext, onError, onCompleted)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, IAsyncObserver<TSource> witness)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (witness == null)
                throw new ArgumentNullException(nameof(witness));

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        await witness.OnNextAsync(x).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(x).ConfigureAwait(false);
                },
                async error =>
                {
                    try
                    {
                        await witness.OnErrorAsync(error).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnErrorAsync(error).ConfigureAwait(false);
                },
                async () =>
                {
                    try
                    {
                        await witness.OnCompletedAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<TSource, Task> onNext)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Do(observer, Create(onNext));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<Exception, Task> onError)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Do(observer, Create<TSource>(_ => Task.CompletedTask, onError, () => Task.CompletedTask));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<Task> onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, Create<TSource>(_ => Task.CompletedTask, _ => Task.CompletedTask, onCompleted));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<TSource, Task> onNext, Func<Exception, Task> onError, Func<Task> onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, Create(onNext, onError, onCompleted));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, IObserver<TSource> witness)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (witness == null)
                throw new ArgumentNullException(nameof(witness));

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        witness.OnNext(x);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnNextAsync(x).ConfigureAwait(false);
                },
                async error =>
                {
                    try
                    {
                        witness.OnError(error);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnErrorAsync(error).ConfigureAwait(false);
                },
                async () =>
                {
                    try
                    {
                        witness.OnCompleted();
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action<TSource> onNext)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Do(observer, x => { onNext(x); return Task.CompletedTask; });
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action<Exception> onError)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Do(observer, Create<TSource>(_ => Task.CompletedTask, ex => { onError(ex); return Task.CompletedTask; }, () => Task.CompletedTask));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, Create<TSource>(_ => Task.CompletedTask, _ => Task.CompletedTask, () => { onCompleted(); return Task.CompletedTask; }));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, x => { onNext(x); return Task.CompletedTask; }, ex => { onError(ex); return Task.CompletedTask; }, () => { onCompleted(); return Task.CompletedTask; });
        }
    }
}
