// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> If<TSource>(Func<bool> condition, IAsyncObservable<TSource> thenSource) => If(condition, thenSource, Empty<TSource>());

        public static IAsyncObservable<TSource> If<TSource>(Func<bool> condition, IAsyncObservable<TSource> thenSource, IAsyncScheduler scheduler) => If(condition, thenSource, Empty<TSource>(scheduler));

        public static IAsyncObservable<TSource> If<TSource>(Func<bool> condition, IAsyncObservable<TSource> thenSource, IAsyncObservable<TSource> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return Create<TSource>(observer =>
            {
                var b = default(bool);

                try
                {
                    b = condition();
                }
                catch (Exception ex)
                {
                    return Throw<TSource>(ex).SubscribeAsync(observer);
                }

                return (b ? thenSource : elseSource).SubscribeSafeAsync(observer);
            });
        }

        public static IAsyncObservable<TSource> If<TSource>(Func<Task<bool>> condition, IAsyncObservable<TSource> thenSource) => If(condition, thenSource, Empty<TSource>());

        public static IAsyncObservable<TSource> If<TSource>(Func<Task<bool>> condition, IAsyncObservable<TSource> thenSource, IAsyncScheduler scheduler) => If(condition, thenSource, Empty<TSource>(scheduler));

        public static IAsyncObservable<TSource> If<TSource>(Func<Task<bool>> condition, IAsyncObservable<TSource> thenSource, IAsyncObservable<TSource> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return Create<TSource>(async observer =>
            {
                var b = default(bool);

                try
                {
                    b = await condition().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return await Throw<TSource>(ex).SubscribeAsync(observer).ConfigureAwait(false);
                }

                return await (b ? thenSource : elseSource).SubscribeSafeAsync(observer).ConfigureAwait(false);
            });
        }
    }
}
