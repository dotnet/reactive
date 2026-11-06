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

            return new DeferAsyncObservable<TSource>(observableFactory);
        }

        public static IAsyncObservable<TSource> DeferAsync<TSource>(Func<CancellationToken, ValueTask<IAsyncObservable<TSource>>> observableFactory) => Defer(observableFactory);

        public static IAsyncObservable<TSource> Defer<TSource>(Func<CancellationToken, ValueTask<IAsyncObservable<TSource>>> observableFactory)
        {
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            // Deferred, as Rx.NET's DeferAsync is: StartAsync alone would run the factory when
            // the sequence is built rather than on each subscription.
            return Defer(() => StartAsync(observableFactory).Merge());
        }
    }

    // Defer as a class rather than a Create, so that a concatenating sink can evaluate the
    // factory (IEvaluatableAsyncObservable) and flatten what it yields, as Rx.NET's does.
    internal sealed class DeferAsyncObservable<TSource> : AsyncObservableBase<TSource>, IEvaluatableAsyncObservable<TSource>
    {
        private readonly Func<ValueTask<IAsyncObservable<TSource>>> _observableFactory;

        public DeferAsyncObservable(Func<ValueTask<IAsyncObservable<TSource>>> observableFactory)
        {
            _observableFactory = observableFactory;
        }

        public ValueTask<IAsyncObservable<TSource>> EvalAsync() => _observableFactory();

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var source = default(IAsyncObservable<TSource>);

            try
            {
                source = await _observableFactory().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                return AsyncDisposable.Nop;
            }

            return await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
        }
    }
}
