// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public abstract class ReplayAsyncSubject<T> : IAsyncSubject<T>
    {
        protected readonly IAsyncSubject<T> _impl;

        public ReplayAsyncSubject(bool concurrent)
            : this(concurrent, int.MaxValue)
        {
        }

        public ReplayAsyncSubject(bool concurrent, int bufferSize)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            if (bufferSize == 1)
            {
                _impl = new ReplayOne(concurrent, CreateImmediateObserver);
            }
            else if (bufferSize == int.MaxValue)
            {
                _impl = new ReplayAll(concurrent, CreateImmediateObserver);
            }
            else
            {
                _impl = new ReplayMany(concurrent, CreateImmediateObserver, bufferSize);
            }
        }

        public ReplayAsyncSubject(bool concurrent, IAsyncScheduler scheduler)
            : this(concurrent, int.MaxValue, scheduler)
        {
        }

        public ReplayAsyncSubject(bool concurrent, int bufferSize, IAsyncScheduler scheduler)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            if (bufferSize == 1)
            {
                _impl = new ReplayOne(concurrent, o => CreateScheduledObserver(o, scheduler));
            }
            else if (bufferSize == int.MaxValue)
            {
                _impl = new ReplayAll(concurrent, o => CreateScheduledObserver(o, scheduler));
            }
            else
            {
                _impl = new ReplayMany(concurrent, o => CreateScheduledObserver(o, scheduler), bufferSize);
            }
        }

        public ReplayAsyncSubject(bool concurrent, TimeSpan window)
        {
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));

            _impl = new ReplayTime(concurrent, ImmediateAsyncScheduler.Instance, int.MaxValue, window, CreateImmediateObserver);
        }

        public ReplayAsyncSubject(bool concurrent, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            _impl = new ReplayTime(concurrent, scheduler, int.MaxValue, window, o => CreateScheduledObserver(o, scheduler));
        }

        public ReplayAsyncSubject(bool concurrent, int bufferSize, TimeSpan window)
        {
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            _impl = new ReplayTime(concurrent, ImmediateAsyncScheduler.Instance, bufferSize, window, CreateImmediateObserver);
        }

        public ReplayAsyncSubject(bool concurrent, int bufferSize, TimeSpan window, IAsyncScheduler scheduler)
        {
            if (window < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(window));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            _impl = new ReplayTime(concurrent, scheduler, bufferSize, window, o => CreateScheduledObserver(o, scheduler));
        }

        private static IScheduledAsyncObserver<T> CreateImmediateObserver(IAsyncObserver<T> observer) => new FastImmediateAsyncObserver<T>(observer);

        private static IScheduledAsyncObserver<T> CreateScheduledObserver(IAsyncObserver<T> observer, IAsyncScheduler scheduler) => new ScheduledAsyncObserver<T>(observer, scheduler);

        public Task OnCompletedAsync() => _impl.OnCompletedAsync();

        public Task OnErrorAsync(Exception error) => _impl.OnErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        public Task OnNextAsync(T value) => _impl.OnNextAsync(value);

        public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer) => _impl.SubscribeAsync(observer ?? throw new ArgumentNullException(nameof(observer)));

        private abstract class ReplayBase : IAsyncSubject<T>
        {
            private readonly bool _concurrent;
            private readonly AsyncLock _lock = new AsyncLock();
            private readonly List<IScheduledAsyncObserver<T>> _observers = new List<IScheduledAsyncObserver<T>>(); // TODO: immutable array
            private bool _done;
            private Exception _error;

            public ReplayBase(bool concurrent)
            {
                _concurrent = concurrent;
            }

            public async Task OnCompletedAsync()
            {
                var observers = default(IScheduledAsyncObserver<T>[]);

                using (await _lock.LockAsync().ConfigureAwait(false))
                {
                    if (!_done)
                    {
                        _done = true;
                        Trim();

                        observers = _observers.ToArray();

                        if (_concurrent)
                        {
                            await Task.WhenAll(observers.Select(o => o.OnCompletedAsync())).ConfigureAwait(false);
                        }
                        else
                        {
                            foreach (var observer in observers)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                }

                if (observers != null)
                {
                    await EnsureActive(observers).ConfigureAwait(false);
                }
            }

            public async Task OnErrorAsync(Exception error)
            {
                var observers = default(IScheduledAsyncObserver<T>[]);

                using (await _lock.LockAsync().ConfigureAwait(false))
                {
                    if (!_done)
                    {
                        _done = true;
                        _error = error;
                        Trim();

                        observers = _observers.ToArray();

                        if (_concurrent)
                        {
                            await Task.WhenAll(observers.Select(o => o.OnErrorAsync(error))).ConfigureAwait(false);
                        }
                        else
                        {
                            foreach (var observer in observers)
                            {
                                await observer.OnErrorAsync(error).ConfigureAwait(false);
                            }
                        }
                    }
                }

                if (observers != null)
                {
                    await EnsureActive(observers).ConfigureAwait(false);
                }
            }

            public async Task OnNextAsync(T value)
            {
                var observers = default(IScheduledAsyncObserver<T>[]);

                using (await _lock.LockAsync().ConfigureAwait(false))
                {
                    if (!_done)
                    {
                        await NextAsync(value);
                        Trim();

                        observers = _observers.ToArray();

                        if (_concurrent)
                        {
                            await Task.WhenAll(observers.Select(o => o.OnNextAsync(value))).ConfigureAwait(false);
                        }
                        else
                        {
                            foreach (var observer in observers)
                            {
                                await observer.OnNextAsync(value).ConfigureAwait(false);
                            }
                        }
                    }
                }

                if (observers != null)
                {
                    await EnsureActive(observers).ConfigureAwait(false);
                }
            }

            private async Task EnsureActive(IScheduledAsyncObserver<T>[] observers)
            {
                if (_concurrent)
                {
                    await Task.WhenAll(observers.Select(o => o.EnsureActive())).ConfigureAwait(false);
                }
                else
                {
                    foreach (var observer in observers)
                    {
                        await observer.EnsureActive().ConfigureAwait(false);
                    }
                }
            }

            public async Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
            {
                var res = AsyncDisposable.Nop;

                var scheduled = CreateScheduledObserver(observer);

                var count = 0;

                using (await _lock.LockAsync().ConfigureAwait(false))
                {
                    Trim();

                    count = await ReplayAsync(scheduled).ConfigureAwait(false);

                    if (_error != null)
                    {
                        count++;
                        await scheduled.OnErrorAsync(_error).ConfigureAwait(false);
                    }
                    else if (_done)
                    {
                        count++;
                        await scheduled.OnCompletedAsync().ConfigureAwait(false);
                    }

                    if (!_done)
                    {
                        _observers.Add(scheduled);

                        res = new Subscription(this, scheduled);
                    }
                }

                await scheduled.EnsureActive(count).ConfigureAwait(false);

                return res;
            }

            protected abstract IScheduledAsyncObserver<T> CreateScheduledObserver(IAsyncObserver<T> observer);

            protected abstract Task NextAsync(T value);

            protected abstract Task<int> ReplayAsync(IScheduledAsyncObserver<T> observer);

            protected abstract void Trim();

            private async Task UnsubscribeAsync(IScheduledAsyncObserver<T> observer)
            {
                using (await _lock.LockAsync().ConfigureAwait(false))
                {
                    _observers.Remove(observer);
                }
            }

            private sealed class Subscription : IAsyncDisposable
            {
                private readonly ReplayBase _parent;
                private readonly IScheduledAsyncObserver<T> _scheduled;

                public Subscription(ReplayBase parent, IScheduledAsyncObserver<T> scheduled)
                {
                    _parent = parent;
                    _scheduled = scheduled;
                }

                public Task DisposeAsync() => _parent.UnsubscribeAsync(_scheduled);
            }
        }

        private abstract class ReplayBufferBase : ReplayBase
        {
            private readonly Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> _createObserver;

            public ReplayBufferBase(bool concurrent, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver)
                : base(concurrent)
            {
                _createObserver = createObserver;
            }

            protected override IScheduledAsyncObserver<T> CreateScheduledObserver(IAsyncObserver<T> observer) => _createObserver(observer);
        }

        private sealed class ReplayOne : ReplayBufferBase
        {
            private bool _hasValue;
            private T _value;

            public ReplayOne(bool concurrent, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver)
                : base(concurrent, createObserver)
            {
            }

            protected override Task NextAsync(T value)
            {
                _hasValue = true;
                _value = value;

                return Task.CompletedTask;
            }

            protected override async Task<int> ReplayAsync(IScheduledAsyncObserver<T> observer)
            {
                if (_hasValue)
                {
                    await observer.OnNextAsync(_value).ConfigureAwait(false);
                    return 1;
                }

                return 0;
            }

            protected override void Trim() { }
        }

        private abstract class ReplayManyBase : ReplayBufferBase
        {
            protected readonly Queue<T> _values = new Queue<T>();

            public ReplayManyBase(bool concurrent, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver)
                : base(concurrent, createObserver)
            {
            }

            protected override Task NextAsync(T value)
            {
                _values.Enqueue(value);

                return Task.CompletedTask;
            }

            protected override async Task<int> ReplayAsync(IScheduledAsyncObserver<T> observer)
            {
                var count = _values.Count;

                foreach (var value in _values)
                {
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                }

                return count;
            }
        }

        private sealed class ReplayMany : ReplayManyBase
        {
            private readonly int _bufferSize;

            public ReplayMany(bool concurrent, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver, int bufferSize)
                : base(concurrent, createObserver)
            {
                _bufferSize = bufferSize;
            }

            protected override void Trim()
            {
                while (_values.Count > _bufferSize)
                {
                    _values.Dequeue();
                }
            }
        }

        private sealed class ReplayAll : ReplayManyBase
        {
            public ReplayAll(bool concurrent, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver)
                : base(concurrent, createObserver)
            {
            }

            protected override void Trim() { }
        }

        private sealed class ReplayTime : ReplayBufferBase
        {
            private readonly IAsyncScheduler _scheduler;
            private readonly int _bufferSize;
            private readonly TimeSpan _window;
            private readonly Queue<Timestamped<T>> _values = new Queue<Timestamped<T>>();

            public ReplayTime(bool concurrent, IAsyncScheduler scheduler, int bufferSize, TimeSpan window, Func<IAsyncObserver<T>, IScheduledAsyncObserver<T>> createObserver)
                : base(concurrent, createObserver)
            {
                _scheduler = scheduler;
                _bufferSize = bufferSize;
                _window = window;
            }

            protected override Task NextAsync(T value)
            {
                _values.Enqueue(new Timestamped<T>(value, _scheduler.Now));

                return Task.CompletedTask;
            }

            protected override async Task<int> ReplayAsync(IScheduledAsyncObserver<T> observer)
            {
                var count = _values.Count;

                foreach (var value in _values)
                {
                    await observer.OnNextAsync(value.Value).ConfigureAwait(false);
                }

                return count;
            }

            protected override void Trim()
            {
                while (_values.Count > _bufferSize)
                {
                    _values.Dequeue();
                }

                var threshold = _scheduler.Now - _window;

                while (_values.Count > 0 && _values.Peek().Timestamp < threshold)
                {
                    _values.Dequeue();
                }
            }
        }
    }
}
