// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class TakeUntil<TSource, TOther> : Producer<TSource, TakeUntil<TSource, TOther>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TOther> _other;

        public TakeUntil(IObservable<TSource> source, IObservable<TOther> other)
        {
            _source = source;
            _other = other;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(TakeUntil<TSource, TOther> parent)
            {
                var sourceObserver = new SourceObserver(this);
                var otherObserver = new OtherObserver(this, sourceObserver);

                // COMPAT - Order of Subscribe calls per v1.0.10621
                var otherSubscription = parent._other.SubscribeSafe(otherObserver);
                otherObserver.Disposable = otherSubscription;

                var sourceSubscription = parent._source.SubscribeSafe(sourceObserver);

                return StableCompositeDisposable.Create(
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
             * because the "other" channel still needs to send an OnCompleted message, which could happen
             * concurrently with another message when the "source" channel has already read from the
             * _parent._observer field between making the On* call.
             * 
             * Fixing this issue requires an ownership transfer mechanism for channels to get exclusive
             * access to the outgoing observer while dispatching a message. Doing this more fine-grained
             * than using locks turns out to be tricky and doesn't reduce cost.
             */
            private sealed class SourceObserver : IObserver<TSource>
            {
                private readonly _ _parent;
                public volatile bool _open;

                public SourceObserver(_ parent)
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

            private sealed class OtherObserver : IObserver<TOther>
            {
                private readonly _ _parent;
                private readonly SourceObserver _sourceObserver;
                private readonly SingleAssignmentDisposable _subscription;

                public OtherObserver(_ parent, SourceObserver sourceObserver)
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

    internal sealed class TakeUntil<TSource> : Producer<TSource, TakeUntil<TSource>._>
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

        public IObservable<TSource> Combine(DateTimeOffset endTime)
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly object _gate = new object();

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(TakeUntil<TSource> parent)
            {
                var t = parent._scheduler.Schedule(parent._endTime, Tick);
                var d = parent._source.SubscribeSafe(this);
                return StableCompositeDisposable.Create(t, d);
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
