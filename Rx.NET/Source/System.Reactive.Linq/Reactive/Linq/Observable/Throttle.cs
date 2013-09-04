// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Throttle<TSource> : Producer<TSource>
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

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Throttle<TSource> _parent;

            public _(Throttle<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private TSource _value;
            private bool _hasValue;
            private SerialDisposable _cancelable;
            private ulong _id;

            public IDisposable Run()
            {
                _gate = new object();
                _value = default(TSource);
                _hasValue = false;
                _cancelable = new SerialDisposable();
                _id = 0UL;

                var subscription = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(subscription, _cancelable);
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
                d.Disposable = _parent._scheduler.Schedule(currentid, _parent._dueTime, Propagate);
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

    class Throttle<TSource, TThrottle> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, IObservable<TThrottle>> _throttleSelector;

        public Throttle(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleSelector)
        {
            _source = source;
            _throttleSelector = throttleSelector;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Throttle<TSource, TThrottle> _parent;

            public _(Throttle<TSource, TThrottle> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private TSource _value;
            private bool _hasValue;
            private SerialDisposable _cancelable;
            private ulong _id;

            public IDisposable Run()
            {
                _gate = new object();
                _value = default(TSource);
                _hasValue = false;
                _cancelable = new SerialDisposable();
                _id = 0UL;

                var subscription = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(subscription, _cancelable);
            }

            public void OnNext(TSource value)
            {
                var throttle = default(IObservable<TThrottle>);
                try
                {
                    throttle = _parent._throttleSelector(value);
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
                d.Disposable = throttle.SubscribeSafe(new δ(this, value, currentid, d));
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

            class δ : IObserver<TThrottle>
            {
                private readonly _ _parent;
                private readonly TSource _value;
                private readonly ulong _currentid;
                private readonly IDisposable _self;

                public δ(_ parent, TSource value, ulong currentid, IDisposable self)
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
#endif