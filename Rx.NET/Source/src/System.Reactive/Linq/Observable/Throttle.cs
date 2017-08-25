// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Throttle<TSource> : Producer<TSource, Throttle<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly TimeSpan _dueTime;
        private readonly IScheduler _scheduler;

        public Throttle(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            _source = source;
            _dueTime = dueTime;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly TimeSpan _dueTime;
            private readonly IScheduler _scheduler;

            public _(Throttle<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _dueTime = parent._dueTime;
                _scheduler = parent._scheduler;
            }

            private object _gate;
            private TSource _value;
            private bool _hasValue;
            private SerialDisposable _cancelable;
            private ulong _id;

            public IDisposable Run(IObservable<TSource> source)
            {
                _gate = new object();
                _value = default(TSource);
                _hasValue = false;
                _cancelable = new SerialDisposable();
                _id = 0UL;

                var subscription = source.SubscribeSafe(this);

                return StableCompositeDisposable.Create(subscription, _cancelable);
            }

            public void OnNext(TSource value)
            {
                var currentid = default(ulong);
                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
                    _id = unchecked(_id + 1);
                    currentid = _id;
                }
                var d = new SingleAssignmentDisposable();
                _cancelable.Disposable = d;
                d.Disposable = _scheduler.Schedule(currentid, _dueTime, Propagate);
            }

            private IDisposable Propagate(IScheduler self, ulong currentid)
            {
                lock (_gate)
                {
                    if (_hasValue && _id == currentid)
                        base._observer.OnNext(_value);
                    _hasValue = false;
                }

                return Disposable.Empty;
            }

            public void OnError(Exception error)
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public void OnCompleted()
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    if (_hasValue)
                        base._observer.OnNext(_value);

                    base._observer.OnCompleted();
                    base.Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }
        }
    }

    internal sealed class Throttle<TSource, TThrottle> : Producer<TSource, Throttle<TSource, TThrottle>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, IObservable<TThrottle>> _throttleSelector;

        public Throttle(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleSelector)
        {
            _source = source;
            _throttleSelector = throttleSelector;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Func<TSource, IObservable<TThrottle>> _throttleSelector;

            public _(Throttle<TSource, TThrottle> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _throttleSelector = parent._throttleSelector;
            }

            private object _gate;
            private TSource _value;
            private bool _hasValue;
            private SerialDisposable _cancelable;
            private ulong _id;

            public IDisposable Run(Throttle<TSource, TThrottle> parent)
            {
                _gate = new object();
                _value = default(TSource);
                _hasValue = false;
                _cancelable = new SerialDisposable();
                _id = 0UL;

                var subscription = parent._source.SubscribeSafe(this);

                return StableCompositeDisposable.Create(subscription, _cancelable);
            }

            public void OnNext(TSource value)
            {
                var throttle = default(IObservable<TThrottle>);
                try
                {
                    throttle = _throttleSelector(value);
                }
                catch (Exception error)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(error);
                        base.Dispose();
                    }

                    return;
                }

                ulong currentid;
                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
                    _id = unchecked(_id + 1);
                    currentid = _id;
                }

                var d = new SingleAssignmentDisposable();
                _cancelable.Disposable = d;
                d.Disposable = throttle.SubscribeSafe(new ThrottleObserver(this, value, currentid, d));
            }

            public void OnError(Exception error)
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public void OnCompleted()
            {
                _cancelable.Dispose();

                lock (_gate)
                {
                    if (_hasValue)
                        base._observer.OnNext(_value);

                    base._observer.OnCompleted();
                    base.Dispose();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            private sealed class ThrottleObserver : IObserver<TThrottle>
            {
                private readonly _ _parent;
                private readonly TSource _value;
                private readonly ulong _currentid;
                private readonly IDisposable _self;

                public ThrottleObserver(_ parent, TSource value, ulong currentid, IDisposable self)
                {
                    _parent = parent;
                    _value = value;
                    _currentid = currentid;
                    _self = self;
                }

                public void OnNext(TThrottle value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentid)
                            _parent._observer.OnNext(_value);

                        _parent._hasValue = false;
                        _self.Dispose();
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentid)
                            _parent._observer.OnNext(_value);

                        _parent._hasValue = false;
                        _self.Dispose();
                    }
                }
            }
        }
    }
}
