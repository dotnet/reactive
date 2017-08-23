// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Timeout<TSource>
    {
        internal sealed class Relative : Producer<TSource, Relative._>
        {
            private readonly IObservable<TSource> _source;
            private readonly TimeSpan _dueTime;
            private readonly IObservable<TSource> _other;
            private readonly IScheduler _scheduler;

            public Relative(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
            {
                _source = source;
                _dueTime = dueTime;
                _other = other;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly TimeSpan _dueTime;
                private readonly IObservable<TSource> _other;
                private readonly IScheduler _scheduler;

                private readonly object _gate = new object();
                private SerialDisposable _subscription = new SerialDisposable();
                private SerialDisposable _timer = new SerialDisposable();

                public _(Relative parent, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _dueTime = parent._dueTime;
                    _other = parent._other;
                    _scheduler = parent._scheduler;
                }

                private ulong _id;
                private bool _switched;

                public IDisposable Run(IObservable<TSource> source)
                {
                    var original = new SingleAssignmentDisposable();

                    _subscription.Disposable = original;

                    _id = 0UL;
                    _switched = false;

                    CreateTimer();

                    original.Disposable = source.SubscribeSafe(this);

                    return StableCompositeDisposable.Create(_subscription, _timer);
                }

                private void CreateTimer()
                {
                    _timer.Disposable = _scheduler.Schedule(_id, _dueTime, Timeout);
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
                        _subscription.Disposable = _other.SubscribeSafe(GetForwarder());

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

        internal sealed class Absolute : Producer<TSource, Absolute._>
        {
            private readonly IObservable<TSource> _source;
            private readonly DateTimeOffset _dueTime;
            private readonly IObservable<TSource> _other;
            private readonly IScheduler _scheduler;

            public Absolute(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
            {
                _source = source;
                _dueTime = dueTime;
                _other = other;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_other, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private readonly IObservable<TSource> _other;

                private readonly object _gate = new object();
                private readonly SerialDisposable _subscription = new SerialDisposable();

                public _(IObservable<TSource> other, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _other = other;
                }

                private bool _switched;

                public IDisposable Run(Absolute parent)
                {
                    var original = new SingleAssignmentDisposable();

                    _subscription.Disposable = original;

                    _switched = false;

                    var timer = parent._scheduler.Schedule(parent._dueTime, Timeout);

                    original.Disposable = parent._source.SubscribeSafe(this);

                    return StableCompositeDisposable.Create(_subscription, timer);
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
                        _subscription.Disposable = _other.SubscribeSafe(GetForwarder());
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
        }
    }

    internal sealed class Timeout<TSource, TTimeout> : Producer<TSource, Timeout<TSource, TTimeout>._>
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Func<TSource, IObservable<TTimeout>> _timeoutSelector;
            private readonly IObservable<TSource> _other;

            private readonly object _gate = new object();
            private readonly SerialDisposable _subscription = new SerialDisposable();
            private readonly SerialDisposable _timer = new SerialDisposable();

            public _(Timeout<TSource, TTimeout> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _timeoutSelector = parent._timeoutSelector;
                _other = parent._other;
            }

            private ulong _id;
            private bool _switched;

            public IDisposable Run(Timeout<TSource, TTimeout> parent)
            {
                var original = new SingleAssignmentDisposable();

                _subscription.Disposable = original;

                _id = 0UL;
                _switched = false;

                SetTimer(parent._firstTimeout);

                original.Disposable = parent._source.SubscribeSafe(this);

                return StableCompositeDisposable.Create(_subscription, _timer);
            }

            public void OnNext(TSource value)
            {
                if (ObserverWins())
                {
                    base._observer.OnNext(value);

                    var timeout = default(IObservable<TTimeout>);
                    try
                    {
                        timeout = _timeoutSelector(value);
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
                d.Disposable = timeout.SubscribeSafe(new TimeoutObserver(this, myid, d));
            }

            private sealed class TimeoutObserver : IObserver<TTimeout>
            {
                private readonly _ _parent;
                private readonly ulong _id;
                private readonly IDisposable _self;

                public TimeoutObserver(_ parent, ulong id, IDisposable self)
                {
                    _parent = parent;
                    _id = id;
                    _self = self;
                }

                public void OnNext(TTimeout value)
                {
                    if (TimerWins())
                        _parent._subscription.Disposable = _parent._other.SubscribeSafe(_parent.GetForwarder());

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
                        _parent._subscription.Disposable = _parent._other.SubscribeSafe(_parent.GetForwarder());
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
