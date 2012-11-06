// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class TakeUntil<TSource, TOther> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TOther> _other;

        public TakeUntil(IObservable<TSource> source, IObservable<TOther> other)
        {
            _source = source;
            _other = other;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>
        {
            private readonly TakeUntil<TSource, TOther> _parent;

            public _(TakeUntil<TSource, TOther> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var sourceObserver = new T(this);
                var otherObserver = new O(this, sourceObserver);

                // COMPAT - Order of Subscribe calls per v1.0.10621
                var otherSubscription = _parent._other.SubscribeSafe(otherObserver);
                otherObserver.Disposable = otherSubscription;

                var sourceSubscription = _parent._source.SubscribeSafe(sourceObserver);

                return new CompositeDisposable(
                    otherSubscription,
                    sourceSubscription
                );
            }

            /*
             * We tried a more fine-grained synchronization scheme to make TakeUntil more efficient, but
             * this requires several CAS instructions, which quickly add up to being non-beneficial.
             * 
             * Notice an approach where the "other" channel performs an Interlocked.Exchange operation on
             * the _parent._observer field to substitute it with a NopObserver<TSource> doesn't work,
             * because the "other" channel still need to send an OnCompleted message, which could happen
             * concurrently with another message when the "source" channel has already read from the
             * _parent._observer field between making the On* call.
             * 
             * Fixing this issue requires an ownership transfer mechanism for channels to get exclusive
             * access to the outgoing observer while dispatching a message. Doing this more fine-grained
             * than using locks turns out to be tricky and doesn't reduce cost.
             */
            class T : IObserver<TSource>
            {
                private readonly _ _parent;
                public volatile bool _open;

                public T(_ parent)
                {
                    _parent = parent;
                    _open = false;
                }

                public void OnNext(TSource value)
                {
                    if (_open)
                    {
                        _parent._observer.OnNext(value);
                    }
                    else
                    {
                        lock (_parent)
                        {
                            _parent._observer.OnNext(value);
                        }
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_parent)
                    {
                        _parent._observer.OnCompleted();
                        _parent.Dispose();
                    }
                }
            }

            class O : IObserver<TOther>
            {
                private readonly _ _parent;
                private readonly T _sourceObserver;
                private readonly SingleAssignmentDisposable _subscription;

                public O(_ parent, T sourceObserver)
                {
                    _parent = parent;
                    _sourceObserver = sourceObserver;
                    _subscription = new SingleAssignmentDisposable();
                }

                public IDisposable Disposable
                {
                    set { _subscription.Disposable = value; }
                }

                public void OnNext(TOther value)
                {
                    lock (_parent)
                    {
                        _parent._observer.OnCompleted();
                        _parent.Dispose();
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_parent)
                    {
                        _sourceObserver._open = true;
                        _subscription.Dispose();
                    }
                }
            }
        }
    }

    class TakeUntil<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly DateTimeOffset _endTime;
        internal readonly IScheduler _scheduler;

        public TakeUntil(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
        {
            _source = source;
            _endTime = endTime;
            _scheduler = scheduler;
        }

        public IObservable<TSource> Ω(DateTimeOffset endTime)
        {
            //
            // Minimum semantics:
            //
            //   t                     0--1--2--3--4--5--6--7->   t                     0--1--2--3--4--5--6--7->
            //
            //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
            //   xs.TU(5AM)            --o--o--o--o--o|           xs.TU(3AM)            --o--o--o|
            //   xs.TU(5AM).TU(3AM)    --o--o--o|                 xs.TU(3AM).TU(5AM)    --o--o--o|
            //
            if (_endTime <= endTime)
                return this;
            else
                return new TakeUntil<TSource>(_source, endTime, _scheduler);
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly TakeUntil<TSource> _parent;

            public _(TakeUntil<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;

            public IDisposable Run()
            {
                _gate = new object();

                var t = _parent._scheduler.Schedule(_parent._endTime, Tick);
                var d = _parent._source.SubscribeSafe(this);
                return new CompositeDisposable(t, d);
            }

            private void Tick()
            {
                lock (_gate)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    base._observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
#endif