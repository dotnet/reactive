// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal abstract class ScheduledAsyncObserverBase<T> : AsyncObserverBase<T>, IScheduledAsyncObserver<T>
    {
        private readonly IAsyncObserver<T> _observer;

        private readonly AsyncGate _lock = new();
        private readonly Queue<T> _queue = new();

        private bool _hasFaulted = false;
        private bool _busy = false;
        private bool _done;
        private Exception _error;

        public ScheduledAsyncObserverBase(IAsyncObserver<T> observer)
        {
            _observer = observer;
        }

        public ValueTask EnsureActive() => EnsureActive(1);

        public async ValueTask EnsureActive(int count)
        {
            var shouldRun = false;

            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (!_hasFaulted && !_busy)
                {
                    _busy = true;
                    shouldRun = true;
                }
            }

            if (shouldRun)
            {
                await ScheduleAsync().ConfigureAwait(false);
            }
        }

        protected abstract ValueTask ScheduleAsync();

        protected async ValueTask RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var values = default(T[]);
                var error = default(Exception);
                var done = false;

                using (await RendezVous(_lock.LockAsync()))
                {
                    if (_queue.Count > 0)
                    {
                        values = _queue.ToArray();
                        _queue.Clear();
                    }

                    if (_done)
                    {
                        done = _done;
                        error = _error;
                    }

                    if (values == null && !done)
                    {
                        _busy = false;
                        break;
                    }
                }

                try
                {
                    if (values != null)
                    {
                        foreach (var value in values)
                        {
                            await RendezVous(_observer.OnNextAsync(value));
                        }
                    }

                    if (done)
                    {
                        if (error != null)
                        {
                            await RendezVous(_observer.OnErrorAsync(error));
                        }
                        else
                        {
                            await RendezVous(_observer.OnCompletedAsync());
                        }

                        break;
                    }
                }
                catch
                {
                    using (await RendezVous(_lock.LockAsync()))
                    {
                        _hasFaulted = true;
                        _queue.Clear();
                    }

                    throw;
                }
            }
        }

        protected override async ValueTask OnCompletedAsyncCore()
        {
            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (!_hasFaulted)
                {
                    _done = true;
                }
            }
        }

        protected override async ValueTask OnErrorAsyncCore(Exception error)
        {
            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (!_hasFaulted)
                {
                    _done = true;
                    _error = error;
                }
            }
        }

        protected override async ValueTask OnNextAsyncCore(T value)
        {
            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (!_hasFaulted)
                {
                    _queue.Enqueue(value);
                }
            }
        }

        protected abstract ValueTaskAwaitable RendezVous(ValueTask task);

        protected abstract ValueTaskAwaitable<R> RendezVous<R>(ValueTask<R> task);

        public abstract ValueTask DisposeAsync();
    }
}
