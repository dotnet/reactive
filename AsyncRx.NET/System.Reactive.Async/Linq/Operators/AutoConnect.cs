// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> AutoConnect<TSource>(this IConnectableAsyncObservable<TSource> source, int minObservers = 1, Action<IAsyncDisposable>? onConnect = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return AutoConnectCore(source, minObservers, onConnect == null ? null : new Func<IAsyncDisposable, ValueTask>(d => { onConnect(d); return default; }));
        }

        public static IAsyncObservable<TSource> AutoConnect<TSource>(this IConnectableAsyncObservable<TSource> source, int minObservers, Func<IAsyncDisposable, ValueTask> onConnect)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onConnect == null)
                throw new ArgumentNullException(nameof(onConnect));

            return AutoConnectCore(source, minObservers, onConnect);
        }

        private static IAsyncObservable<TSource> AutoConnectCore<TSource>(IConnectableAsyncObservable<TSource> source, int minObservers, Func<IAsyncDisposable, ValueTask>? onConnect)
        {
            if (minObservers <= 0)
            {
                // As Rx.NET's: a non-positive count connects at once, when the operator is
                // applied, and the result is the connectable with its identity hidden.
                var ignored = ConnectNowAsync(source, onConnect);

                return source.AsAsyncObservable();
            }

            return new AutoConnectAsyncObservable<TSource>(source, minObservers, onConnect);
        }

        private static async ValueTask ConnectNowAsync<TSource>(IConnectableAsyncObservable<TSource> source, Func<IAsyncDisposable, ValueTask>? onConnect)
        {
            var connection = await source.ConnectAsync().ConfigureAwait(false);

            if (onConnect != null)
            {
                await onConnect(connection).ConfigureAwait(false);
            }
        }
    }
}
