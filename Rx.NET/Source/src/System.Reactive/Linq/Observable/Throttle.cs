// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly object _gate = new();
            private readonly TimeSpan _dueTime;
            private readonly IScheduler _scheduler;

            public _(Throttle<TSource> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _dueTime = parent._dueTime;
                _scheduler = parent._scheduler;
            }

            private TSource? _value;
            private bool _hasValue;
            private SerialDisposableValue _serialCancelable;
            private ulong _id;

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _serialCancelable.Dispose();
                }

                base.Dispose(disposing);
            }

            public override void OnNext(TSource value)
            {
                ulong currentid;

                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
                    _id = unchecked(_id + 1);
                    currentid = _id;
                }

                _serialCancelable.Disposable = null;
                _serialCancelable.Disposable = _scheduler.ScheduleAction((@this: this, currentid), _dueTime, static tuple => tuple.@this.Propagate(tuple.currentid));
            }

            private void Propagate(ulong currentid)
            {
                lock (_gate)
                {
                    if (_hasValue && _id == currentid)
                    {
                        ForwardOnNext(_value!);

                        _hasValue = false;
                    }
                }
            }

            public override void OnError(Exception error)
            {
                _serialCancelable.Dispose();

                lock (_gate)
                {
                    ForwardOnError(error);

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public override void OnCompleted()
            {
                _serialCancelable.Dispose();

                lock (_gate)
                {
                    if (_hasValue)
                    {
                        ForwardOnNext(_value!);
                    }

                    ForwardOnCompleted();

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

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly object _gate = new();
            private readonly Func<TSource, IObservable<TThrottle>> _throttleSelector;

            public _(Throttle<TSource, TThrottle> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _throttleSelector = parent._throttleSelector;
            }

            private TSource? _value;
            private bool _hasValue;
            private SerialDisposableValue _serialCancelable;
            private ulong _id;

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _serialCancelable.Dispose();
                }

                base.Dispose(disposing);
            }

            public override void OnNext(TSource value)
            {
                IObservable<TThrottle> throttle;
                try
                {
                    throttle = _throttleSelector(value);
                }
                catch (Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
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

                _serialCancelable.Disposable = null;

                var newInnerObserver = new ThrottleObserver(this, value, currentid);
                newInnerObserver.SetResource(throttle.SubscribeSafe(newInnerObserver));

                _serialCancelable.Disposable = newInnerObserver;
            }

            public override void OnError(Exception error)
            {
                _serialCancelable.Dispose();

                lock (_gate)
                {
                    ForwardOnError(error);

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            public override void OnCompleted()
            {
                _serialCancelable.Dispose();

                lock (_gate)
                {
                    if (_hasValue)
                    {
                        ForwardOnNext(_value!);
                    }

                    ForwardOnCompleted();

                    _hasValue = false;
                    _id = unchecked(_id + 1);
                }
            }

            private sealed class ThrottleObserver : SafeObserver<TThrottle>
            {
                private readonly _ _parent;
                private readonly TSource _value;
                private readonly ulong _currentid;

                public ThrottleObserver(_ parent, TSource value, ulong currentid)
                {
                    _parent = parent;
                    _value = value;
                    _currentid = currentid;
                }

                public override void OnNext(TThrottle value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentid)
                        {
                            _parent.ForwardOnNext(_value);
                        }

                        _parent._hasValue = false;
                        Dispose();
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent.ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue && _parent._id == _currentid)
                        {
                            _parent.ForwardOnNext(_value);
                        }

                        _parent._hasValue = false;
                        Dispose();
                    }
                }
            }
        }
    }
}
