// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Sample<TSource, TSample> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TSample> _sampler;

        public Sample(IObservable<TSource> source, IObservable<TSample> sampler)
        {
            _source = source;
            _sampler = sampler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Sample<TSource, TSample> _parent;

            public _(Sample<TSource, TSample> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;

            private IDisposable _sourceSubscription;

            private bool _hasValue;
            private TSource _value;
            private bool _atEnd;

            public IDisposable Run()
            {
                _gate = new object();

                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                var samplerSubscription = _parent._sampler.SubscribeSafe(new σ(this));

                return new CompositeDisposable(_sourceSubscription, samplerSubscription);
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
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
                    _atEnd = true;
                    _sourceSubscription.Dispose();
                }
            }

            class σ : IObserver<TSample>
            {
                private readonly _ _parent;

                public σ(_ parent)
                {
                    _parent = parent;
                }

                public void OnNext(TSample value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._hasValue)
                        {
                            _parent._hasValue = false;
                            _parent._observer.OnNext(_parent._value);
                        }

                        if (_parent._atEnd)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }
                }

                public void OnError(Exception error)
                {
                    // BREAKING CHANGE v2 > v1.x - This error used to be swallowed
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
                        if (_parent._hasValue)
                        {
                            _parent._hasValue = false;
                            _parent._observer.OnNext(_parent._value);
                        }

                        if (_parent._atEnd)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }
                }
            }
        }
    }

    class Sample<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly TimeSpan _interval;
        private readonly IScheduler _scheduler;

        public Sample(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
        {
            _source = source;
            _interval = interval;
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
            private readonly Sample<TSource> _parent;

            public _(Sample<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;

            private IDisposable _sourceSubscription;

            private bool _hasValue;
            private TSource _value;
            private bool _atEnd;

            public IDisposable Run()
            {
                _gate = new object();

                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(
                    sourceSubscription,
                    _parent._scheduler.SchedulePeriodic(_parent._interval, Tick)
                );
            }

            private void Tick()
            {
                lock (_gate)
                {
                    if (_hasValue)
                    {
                        _hasValue = false;
                        base._observer.OnNext(_value);
                    }

                    if (_atEnd)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _hasValue = true;
                    _value = value;
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
                    _atEnd = true;
                    _sourceSubscription.Dispose();
                }
            }
        }
    }
}
#endif