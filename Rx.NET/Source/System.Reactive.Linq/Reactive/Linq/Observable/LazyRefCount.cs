// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    class LazyRefCount<TSource> : Producer<TSource>
    {
        private readonly object _gate;
        private readonly IScheduler _scheduler;
        private readonly TimeSpan _disconnectTime;
        private readonly IConnectableObservable<TSource> _source;
        private readonly SerialDisposable _serial = new SerialDisposable();

        private int _count;
        private IDisposable _connectableSubscription;

        public LazyRefCount(IConnectableObservable<TSource> source, TimeSpan disconnectTime, IScheduler scheduler)
        {
            _source = source;
            _gate = new object();
            _disconnectTime = disconnectTime;
            _scheduler = scheduler;
            _count = 0;
            _connectableSubscription = default(IDisposable);
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly LazyRefCount<TSource> _parent;

            public _(LazyRefCount<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var subscription = _parent._source.SubscribeSafe(this);

                lock (_parent._gate)
                {
                    if (++_parent._count == 1)
                    {
                        if (_parent._connectableSubscription == null)
                            _parent._connectableSubscription = _parent._source.Connect();

                        _parent._serial.Disposable = new SingleAssignmentDisposable();
                    }
                }

                return Disposable.Create(() =>
                {
                    subscription.Dispose();

                    lock (_parent._gate)
                    {
                        if (--_parent._count == 0)
                        {
                            var cancelable = (SingleAssignmentDisposable)_parent._serial.Disposable;

                            cancelable.Disposable = _parent._scheduler.Schedule(cancelable, _parent._disconnectTime, (self, state) =>
                            {
                                lock (_parent._gate)
                                {
                                    if (object.ReferenceEquals(_parent._serial.Disposable, state))
                                    {
                                        _parent._connectableSubscription.Dispose();
                                        _parent._connectableSubscription = null;
                                    }
                                }

                                return Disposable.Empty;
                            });
                        }
                    }
                });
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif
