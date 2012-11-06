// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Threading;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class Next<TSource> : PushToPullAdapter<TSource, TSource>
    {
        public Next(IObservable<TSource> source)
            : base(source)
        {
        }

        protected override PushToPullSink<TSource, TSource> Run(IDisposable subscription)
        {
            return new _(subscription);
        }

        class _ : PushToPullSink<TSource, TSource>
        {
            private readonly object _gate;

#if !NO_CDS
            private readonly SemaphoreSlim _semaphore;
#else
            private readonly Semaphore _semaphore;
#endif

            public _(IDisposable subscription)
                : base(subscription)
            {
                _gate = new object();

#if !NO_CDS
                _semaphore = new SemaphoreSlim(0, 1);
#else
                _semaphore = new Semaphore(0, 1);
#endif
            }

            private bool _waiting;
            private NotificationKind _kind;
            private TSource _value;
            private Exception _error;

            public override void OnNext(TSource value)
            {
                lock (_gate)
                {
                    if (_waiting)
                    {
                        _value = value;
                        _kind = NotificationKind.OnNext;
                        _semaphore.Release();
                    }

                    _waiting = false;
                }
            }

            public override void OnError(Exception error)
            {
                base.Dispose();

                lock (_gate)
                {
                    //
                    // BREAKING CHANGE v2 > v1.x - Next doesn't block indefinitely when it reaches the end.
                    //
                    _error = error;
                    _kind = NotificationKind.OnError;

                    if (_waiting)
                        _semaphore.Release();

                    _waiting = false;
                }
            }

            public override void OnCompleted()
            {
                base.Dispose();

                lock (_gate)
                {
                    //
                    // BREAKING CHANGE v2 > v1.x - Next doesn't block indefinitely when it reaches the end.
                    //
                    _kind = NotificationKind.OnCompleted;

                    if (_waiting)
                        _semaphore.Release();

                    _waiting = false;
                }
            }

            public override bool TryMoveNext(out TSource current)
            {
                var done = false;

                lock (_gate)
                {
                    _waiting = true;

                    //
                    // BREAKING CHANGE v2 > v1.x - Next doesn't block indefinitely when it reaches the end.
                    //
                    done = _kind != NotificationKind.OnNext;
                }

                if (!done)
                {
#if !NO_CDS
                    _semaphore.Wait();
#else
                    _semaphore.WaitOne();
#endif
                }

                //
                // When we reach this point, we released the lock and got the next notification
                // from the observer. We assume no concurrent calls to the TryMoveNext method
                // are made (per general guidance on usage of IEnumerable<T>). If the observer
                // enters the lock again, it should have quit it first, causing _waiting to be
                // set to false, hence future accesses of the lock won't set the _kind, _value,
                // and _error fields, until TryMoveNext is entered again and _waiting is reset
                // to true. In conclusion, the fields are stable for read below.
                //
                // Notice we rely on memory barrier acquire/release behavior due to the use of
                // the semaphore, not the lock (we're still under the lock when we release the
                // semaphore in the On* methods!).
                //
                switch (_kind)
                {
                    case NotificationKind.OnNext:
                        current = _value;
                        return true;
                    case NotificationKind.OnError:
                        _error.Throw();
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
#endif
