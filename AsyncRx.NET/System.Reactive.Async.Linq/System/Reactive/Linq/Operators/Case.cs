// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources) => Case(selector, sources, Empty<TResult>());

        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources, IAsyncScheduler scheduler) => Case(selector, sources, Empty<TResult>(scheduler));

        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources, IAsyncObservable<TResult> defaultSource)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (defaultSource == null)
                throw new ArgumentNullException(nameof(defaultSource));

            return Create<TResult>(observer =>
            {
                var source = default(IAsyncObservable<TResult>);

                try
                {
                    var value = selector();

                    if (!sources.TryGetValue(value, out source))
                    {
                        source = defaultSource;
                    }
                }
                catch (Exception ex)
                {
                    return Throw<TResult>(ex).SubscribeAsync(observer);
                }

                return source.SubscribeSafeAsync(observer);
            });
        }

        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<Task<TValue>> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources) => Case(selector, sources, Empty<TResult>());

        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<Task<TValue>> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources, IAsyncScheduler scheduler) => Case(selector, sources, Empty<TResult>(scheduler));

        public static IAsyncObservable<TResult> Case<TValue, TResult>(Func<Task<TValue>> selector, IDictionary<TValue, IAsyncObservable<TResult>> sources, IAsyncObservable<TResult> defaultSource)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (defaultSource == null)
                throw new ArgumentNullException(nameof(defaultSource));

            return Create<TResult>(async observer =>
            {
                var source = default(IAsyncObservable<TResult>);

                try
                {
                    var value = await selector().ConfigureAwait(false);

                    if (!sources.TryGetValue(value, out source))
                    {
                        source = defaultSource;
                    }
                }
                catch (Exception ex)
                {
                    return await Throw<TResult>(ex).SubscribeAsync(observer).ConfigureAwait(false);
                }

                return await source.SubscribeSafeAsync(observer).ConfigureAwait(false);
            });
        }
    }
}
