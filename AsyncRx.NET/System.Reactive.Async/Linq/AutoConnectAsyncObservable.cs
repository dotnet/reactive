// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // AutoConnect's observable, a port of Rx.NET's: every subscription goes to the connectable,
    // and the one that takes the count to the threshold connects it, once, handing the
    // connection to the callback if there is one.
    internal sealed class AutoConnectAsyncObservable<TSource> : AsyncObservableBase<TSource>
    {
        private readonly IConnectableAsyncObservable<TSource> _source;
        private readonly int _minObservers;
        private readonly Func<IAsyncDisposable, ValueTask>? _onConnect;
        private int _count;

        public AutoConnectAsyncObservable(IConnectableAsyncObservable<TSource> source, int minObservers, Func<IAsyncDisposable, ValueTask>? onConnect)
        {
            _source = source;
            _minObservers = minObservers;
            _onConnect = onConnect;
        }

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<TSource> observer)
        {
            var subscription = await _source.SubscribeSafeAsync(observer).ConfigureAwait(false);

            if (Volatile.Read(ref _count) < _minObservers && Interlocked.Increment(ref _count) == _minObservers)
            {
                var connection = await _source.ConnectAsync().ConfigureAwait(false);

                if (_onConnect != null)
                {
                    await _onConnect(connection).ConfigureAwait(false);
                }
            }

            return subscription;
        }
    }
}
