// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    internal sealed class ConnectableAsyncObservable<TSource, TResult> : IConnectableAsyncObservable<TResult>
    {
        private readonly IAsyncSubject<TSource, TResult> subject;
        private readonly IAsyncObservable<TSource> source;
        private readonly AsyncLock gate = new AsyncLock();

        private Connection connection;

        public ConnectableAsyncObservable(IAsyncObservable<TSource> source, IAsyncSubject<TSource, TResult> subject)
        {
            this.subject = subject;
            this.source = source.AsAsyncObservable();
        }

        public async Task<IAsyncDisposable> ConnectAsync()
        {
            using (await gate.LockAsync().ConfigureAwait(false))
            {
                if (connection == null)
                {
                    var subscription = await source.SubscribeAsync(subject).ConfigureAwait(false);
                    connection = new Connection(this, subscription);
                }

                return connection;
            }
        }

        private sealed class Connection : IAsyncDisposable
        {
            private readonly ConnectableAsyncObservable<TSource, TResult> parent;
            private IAsyncDisposable subscription;

            public Connection(ConnectableAsyncObservable<TSource, TResult> parent, IAsyncDisposable subscription)
            {
                this.parent = parent;
                this.subscription = subscription;
            }

            public async Task DisposeAsync()
            {
                using (await parent.gate.LockAsync().ConfigureAwait(false))
                {
                    if (subscription != null)
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                        subscription = null;

                        parent.connection = null;
                    }
                }
            }
        }

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return subject.SubscribeAsync(observer);
        }
    }
}
