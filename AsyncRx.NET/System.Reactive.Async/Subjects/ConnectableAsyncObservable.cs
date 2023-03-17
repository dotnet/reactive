// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    internal sealed class ConnectableAsyncObservable<TSource, TResult> : IConnectableAsyncObservable<TResult>
    {
        private readonly IAsyncSubject<TSource, TResult> _subject;
        private readonly IAsyncObservable<TSource> _source;
        private readonly AsyncGate _gate = new();

        private Connection _connection;

        public ConnectableAsyncObservable(IAsyncObservable<TSource> source, IAsyncSubject<TSource, TResult> subject)
        {
            _subject = subject;
            _source = source.AsAsyncObservable();
        }

        public async ValueTask<IAsyncDisposable> ConnectAsync()
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_connection == null)
                {
                    var subscription = await _source.SubscribeAsync(_subject).ConfigureAwait(false);
                    _connection = new Connection(this, subscription);
                }

                return _connection;
            }
        }

        private sealed class Connection : IAsyncDisposable
        {
            private readonly ConnectableAsyncObservable<TSource, TResult> _parent;
            private IAsyncDisposable _subscription;

            public Connection(ConnectableAsyncObservable<TSource, TResult> parent, IAsyncDisposable subscription)
            {
                _parent = parent;
                _subscription = subscription;
            }

            public async ValueTask DisposeAsync()
            {
                using (await _parent._gate.LockAsync().ConfigureAwait(false))
                {
                    if (_subscription != null)
                    {
                        await _subscription.DisposeAsync().ConfigureAwait(false);
                        _subscription = null;

                        _parent._connection = null;
                    }
                }
            }
        }

        public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _subject.SubscribeAsync(observer);
        }
    }
}
