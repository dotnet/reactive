// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Latest<TSource> : PushToPullAdapter<TSource, TSource>
    {
        public Latest(IObservable<TSource> source)
            : base(source)
        {
        }

        protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription)
        {
            return new _(subscription);
        }

        private sealed class _ : PushToPullSink<TSource, TSource>
        {
            private readonly object _gate;
            private readonly SemaphoreSlim _semaphore;

            public _(IDisposable subscription)
                : base(subscription)
            {
                _gate = new object();
                _semaphore = new SemaphoreSlim(0, 1);
            }

            private bool _notificationAvailable;
            private NotificationKind _kind;
            private TSource _value;
            private Exception _error;

            public override void OnNext(TSource value)
            {
                var lackedValue = false;
                lock (_gate)
                {
                    lackedValue = !_notificationAvailable;
                    _notificationAvailable = true;
                    _kind = NotificationKind.OnNext;
                    _value = value;
                }

                if (lackedValue)
                    _semaphore.Release();
            }

            public override void OnError(Exception error)
            {
                base.Dispose();

                var lackedValue = false;
                lock (_gate)
                {
                    lackedValue = !_notificationAvailable;
                    _notificationAvailable = true;
                    _kind = NotificationKind.OnError;
                    _error = error;
                }

                if (lackedValue)
                    _semaphore.Release();
            }

            public override void OnCompleted()
            {
                base.Dispose();

                var lackedValue = false;
                lock (_gate)
                {
                    lackedValue = !_notificationAvailable;
                    _notificationAvailable = true;
                    _kind = NotificationKind.OnCompleted;
                }

                if (lackedValue)
                    _semaphore.Release();
            }

            public override bool TryMoveNext(out TSource current)
            {
                var kind = default(NotificationKind);
                var value = default(TSource);
                var error = default(Exception);

                _semaphore.Wait();

                lock (_gate)
                {
                    kind = _kind;

                    switch (kind)
                    {
                        case NotificationKind.OnNext:
                            value = _value;
                            break;
                        case NotificationKind.OnError:
                            error = _error;
                            break;
                    }

                    _notificationAvailable = false;
                }

                switch (kind)
                {
                    case NotificationKind.OnNext:
                        current = _value;
                        return true;
                    case NotificationKind.OnError:
                        error.Throw();
                        break;
                    case NotificationKind.OnCompleted:
                        break;
                }

                current = default(TSource);
                return false;
            }
        }
    }
}
