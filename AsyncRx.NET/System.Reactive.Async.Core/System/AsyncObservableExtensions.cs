// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Threading.Tasks;

namespace System
{
    public static class AsyncObservableExtensions
    {
        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, ex => Task.FromException(ex), () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, onErrorAsync, () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Task> onCompletedAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, ex => Task.FromException(ex), onCompletedAsync));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => Task.FromException(ex), () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => { onError(ex); return Task.CompletedTask; }, () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => Task.FromException(ex), () => { onCompleted(); return Task.CompletedTask; }));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => { onError(ex); return Task.CompletedTask; }, () => { onCompleted(); return Task.CompletedTask; }));
        }
    }
}
