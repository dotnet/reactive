// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create(
                source,
                observer,
                static (source, observer, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, observer)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Create(
                source,
                onNext,
                static (source, onNext, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Create(
                source,
                onError,
                static (source, onError, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onError)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create(
                source,
                onCompleted,
                static (source, onCompleted, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onCompleted)));
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

            return Create(
                source,
                (onNext, onError, onCompleted),
                static (source, state, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, state.onNext, state.onError, state.onCompleted)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, IAsyncObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Create(
                source,
                observer,
                static (source, observer, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, observer)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Create(
                source,
                onNext,
                static (source, onNext, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onNext)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<Exception, ValueTask> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Create(
                source,
                onError,
                static (source, onError, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onError)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<ValueTask> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create(
                source,
                onCompleted,
                static (source, onCompleted, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, onCompleted)));
        }

        public static IAsyncObservable<TSource> Do<TSource>(this IAsyncObservable<TSource> source, Func<TSource, ValueTask> onNext, Func<Exception, ValueTask> onError, Func<ValueTask> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Create(
                source,
                (onNext, onError, onCompleted),
                static (source, state, target) => source.SubscribeSafeAsync(AsyncObserver.Do(target, state.onNext, state.onError, state.onCompleted)));
        }
    }

    public partial class AsyncObserver
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

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask> onNext)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return Do(observer, Create(onNext));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<Exception, ValueTask> onError)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Do(observer, Create<TSource>(_ => default, onError, () => default));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<ValueTask> onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, Create<TSource>(_ => default, _ => default, onCompleted));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Func<TSource, ValueTask> onNext, Func<Exception, ValueTask> onError, Func<ValueTask> onCompleted)
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

            return Do(observer, x => { onNext(x); return default; });
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action<Exception> onError)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return Do(observer, Create<TSource>(_ => default, ex => { onError(ex); return default; }, () => default));
        }

        public static IAsyncObserver<TSource> Do<TSource>(IAsyncObserver<TSource> observer, Action onCompleted)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return Do(observer, Create<TSource>(_ => default, _ => default, () => { onCompleted(); return default; }));
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

            return Do(observer, x => { onNext(x); return default; }, ex => { onError(ex); return default; }, () => { onCompleted(); return default; });
        }
    }
}
