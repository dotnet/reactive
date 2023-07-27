// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Defer<TSource>(Func<IAsyncObservable<TSource>> observableFactory)
        {
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Defer(() => new ValueTask<IAsyncObservable<TSource>>(observableFactory()));
        }

        public static IAsyncObservable<TSource> DeferAsync<TSource>(Func<ValueTask<IAsyncObservable<TSource>>> observableFactory) => Defer(observableFactory);

        public static IAsyncObservable<TSource> Defer<TSource>(Func<ValueTask<IAsyncObservable<TSource>>> observableFactory)
        {
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Create<TSource>(async observer =>
            {
                var source = default(IAsyncObservable<TSource>);

                try
                {
                    source = await observableFactory().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                return await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
            });
        }

        public static IAsyncObservable<TSource> DeferAsync<TSource>(Func<CancellationToken, ValueTask<IAsyncObservable<TSource>>> observableFactory) => Defer(observableFactory);

        public static IAsyncObservable<TSource> Defer<TSource>(Func<CancellationToken, ValueTask<IAsyncObservable<TSource>>> observableFactory)
        {
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return StartAsync(observableFactory).Merge();
        }
    }
}
