// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Timeout<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly TimeSpan? _dueTimeR;
        private readonly DateTimeOffset? _dueTimeA;
        private readonly IObservable<TSource> _other;
        private readonly IScheduler _scheduler;

        public Timeout(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            _source = source;
            _dueTimeR = dueTime;
            _other = other;
            _scheduler = scheduler;
        }

        public Timeout(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            _source = source;
            _dueTimeA = dueTime;
            _other = other;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_dueTimeA.HasValue)
            {
                var sink = new α(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                var sink = new ρ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class α : Sink<TSource>, IObserver<TSource>
        {
            private readonly Timeout<TSource> _parent;

            public α(Timeout<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private SerialDisposable _subscription;
            private object _gate;
            private bool _switched;

            public IDisposable Run()
            {
                _subscription = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                _subscription.Disposable = original;

                _gate = new object();
                _switched = false;

                var timer = _parent._scheduler.Schedule(_parent._dueTimeA.Value, Timeout);

                original.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_subscription, timer);
            }

            private void Timeout()
            {
                var timerWins = false;

                lock (_gate)
                {
                    timerWins = !_switched;
                    _switched = true;
                }

                if (timerWins)
                    _subscription.Disposable = _parent._other.SubscribeSafe(this.GetForwarder());
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    if (!_switched)
                        base._observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                var onErrorWins = false;

                lock (_gate)
                {
                    onErrorWins = !_switched;
                    _switched = true;
                }

                if (onErrorWins)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                var onCompletedWins = false;

                lock (_gate)
                {
                    onCompletedWins = !_switched;
                    _switched = true;
                }

                if (onCompletedWins)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        class ρ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Timeout<TSource> _parent;

            public ρ(Timeout<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private SerialDisposable _subscription;
            private SerialDisposable _timer;

            private object _gate;
            private ulong _id;
            private bool _switched;

            public IDisposable Run()
            {
                _subscription = new SerialDisposable();
                _timer = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                _subscription.Disposable = original;

                _gate = new object();
                _id = 0UL;
                _switched = false;

                CreateTimer();

                original.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_subscription, _timer);
            }

            private void CreateTimer()
            {
                _timer.Disposable = _parent._scheduler.Schedule(_id, _parent._dueTimeR.Value, Timeout);
            }

            private IDisposable Timeout(IScheduler _, ulong myid)
            {
                var timerWins = false;

                lock (_gate)
                {
                    _switched = (_id == myid);
                    timerWins = _switched;
                }

                if (timerWins)
                    _subscription.Disposable = _parent._other.SubscribeSafe(this.GetForwarder());

                return Disposable.Empty;
            }

            public void OnNext(TSource value)
            {
                var onNextWins = false;

                lock (_gate)
                {
                    onNextWins = !_switched;
                    if (onNextWins)
                    {
                        _id = unchecked(_id + 1);
                    }
                }

                if (onNextWins)
                {
                    base._observer.OnNext(value);
                    CreateTimer();
                }
            }

            public void OnError(Exception error)
            {
                var onErrorWins = false;

                lock (_gate)
                {
                    onErrorWins = !_switched;
                    if (onErrorWins)
                    {
                        _id = unchecked(_id + 1);
                    }
                }

                if (onErrorWins)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                var onCompletedWins = false;

                lock (_gate)
                {
                    onCompletedWins = !_switched;
                    if (onCompletedWins)
                    {
                        _id = unchecked(_id + 1);
                    }
                }

                if (onCompletedWins)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }

    class Timeout<TSource, TTimeout> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TTimeout> _firstTimeout;
        private readonly Func<TSource, IObservable<TTimeout>> _timeoutSelector;
        private readonly IObservable<TSource> _other;

        public Timeout(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutSelector, IObservable<TSource> other)
        {
            _source = source;
            _firstTimeout = firstTimeout;
            _timeoutSelector = timeoutSelector;
            _other = other;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Timeout<TSource, TTimeout> _parent;

            public _(Timeout<TSource, TTimeout> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private SerialDisposable _subscription;
            private SerialDisposable _timer;
            private object _gate;
            private ulong _id;
            private bool _switched;

            public IDisposable Run()
            {
                _subscription = new SerialDisposable();
                _timer = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                _subscription.Disposable = original;

                _gate = new object();
                _id = 0UL;
                _switched = false;

                SetTimer(_parent._firstTimeout);

                original.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_subscription, _timer);
            }

            public void OnNext(TSource value)
            {
                if (ObserverWins())
                {
                    base._observer.OnNext(value);

                    var timeout = default(IObservable<TTimeout>);
                    try
                    {
                        timeout = _parent._timeoutSelector(value);
                    }
                    catch (Exception error)
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                        return;
                    }

                    SetTimer(timeout);
                }
            }

            public void OnError(Exception error)
            {
                if (ObserverWins())
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                if (ObserverWins())
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            private void SetTimer(IObservable<TTimeout> timeout)
            {
                var myid = _id;

                var d = new SingleAssignmentDisposable();
                _timer.Disposable = d;
                d.Disposable = timeout.SubscribeSafe(new τ(this, myid, d));
            }

            class τ : IObserver<TTimeout>
            {
                private readonly _ _parent;
                private readonly ulong _id;
                private readonly IDisposable _self;

                public τ(_ parent, ulong id, IDisposable self)
                {
                    _parent = parent;
                    _id = id;
                    _self = self;
                }

                public void OnNext(TTimeout value)
                {
                    if (TimerWins())
                        _parent._subscription.Disposable = _parent._parent._other.SubscribeSafe(_parent.GetForwarder());

                    _self.Dispose();
                }

                public void OnError(Exception error)
                {
                    if (TimerWins())
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    if (TimerWins())
                        _parent._subscription.Disposable = _parent._parent._other.SubscribeSafe(_parent.GetForwarder());
                }

                private bool TimerWins()
                {
                    var res = false;

                    lock (_parent._gate)
                    {
                        _parent._switched = (_parent._id == _id);
                        res = _parent._switched;
                    }

                    return res;
                }
            }

            private bool ObserverWins()
            {
                var res = false;

                lock (_gate)
                {
                    res = !_switched;
                    if (res)
                    {
                        _id = unchecked(_id + 1);
                    }
                }

                return res;
            }
        }
    }
}
#endif