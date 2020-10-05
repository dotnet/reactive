// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class AsyncObservable
    {
        private sealed class AsyncObservableImpl<TSource, TResult> : AsyncObservableBase<TResult>
        {
            private readonly IAsyncObservable<TSource> _source;
            private readonly Func<IAsyncObservable<TSource>, IAsyncObserver<TResult>, ValueTask<IAsyncDisposable>> _subscribeAsync;

            public AsyncObservableImpl(IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObserver<TResult>, ValueTask<IAsyncDisposable>> subscribeAsync)
            {
                _source = source;
                _subscribeAsync = subscribeAsync ?? throw new ArgumentNullException(nameof(subscribeAsync));
            }

            protected override ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TResult> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException(nameof(observer));

                return _subscribeAsync(_source, observer);
            }
        }

        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new AsyncObservable<T>(subscribeAsync);
        }

        internal static IAsyncObservable<TResult> Create<TSource, TResult>(IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObserver<TResult>, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new AsyncObservableImpl<TSource, TResult>(source, subscribeAsync);
        }

        internal static IAsyncObservable<TSource> Create<TSource>(IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObserver<TSource>, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            return Create<TSource, TSource>(source, subscribeAsync);
        }

        public static ValueTask<IAsyncDisposable> SubscribeSafeAsync<T>(this IAsyncObservable<T> source, IAsyncObserver<T> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return CoreAsync();

            async ValueTask<IAsyncDisposable> CoreAsync()
            {
                try
                {
                    return await source.SubscribeAsync(observer).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);

                    return AsyncDisposable.Nop;
                }
            }
        }
    }
}
