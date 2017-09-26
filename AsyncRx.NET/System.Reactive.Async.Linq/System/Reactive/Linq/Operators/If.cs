// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> If<TResult>(Func<bool> condition, IAsyncObservable<TResult> thenSource) => If(condition, thenSource, Empty<TResult>());

        public static IAsyncObservable<TResult> If<TResult>(Func<bool> condition, IAsyncObservable<TResult> thenSource, IAsyncScheduler scheduler) => If(condition, thenSource, Empty<TResult>(scheduler));

        public static IAsyncObservable<TResult> If<TResult>(Func<bool> condition, IAsyncObservable<TResult> thenSource, IAsyncObservable<TResult> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return Create<TResult>(observer =>
            {
                var b = default(bool);

                try
                {
                    b = condition();
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex).SubscribeAsync(observer);
                }

                return (b ? thenSource : elseSource).SubscribeSafeAsync(observer);
            });
        }

        public static IAsyncObservable<TResult> If<TResult>(Func<Task<bool>> condition, IAsyncObservable<TResult> thenSource) => If(condition, thenSource, Empty<TResult>());

        public static IAsyncObservable<TResult> If<TResult>(Func<Task<bool>> condition, IAsyncObservable<TResult> thenSource, IAsyncScheduler scheduler) => If(condition, thenSource, Empty<TResult>(scheduler));

        public static IAsyncObservable<TResult> If<TResult>(Func<Task<bool>> condition, IAsyncObservable<TResult> thenSource, IAsyncObservable<TResult> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return Create<TResult>(async observer =>
            {
                var b = default(bool);

                try
                {
                    b = await condition().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return await Throw<TResult>(ex).SubscribeAsync(observer).ConfigureAwait(false);
                }

                return await (b ? thenSource : elseSource).SubscribeSafeAsync(observer).ConfigureAwait(false);
            });
        }
    }
}
