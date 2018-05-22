// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Sample<TSource, TSample> : Producer<TSource, Sample<TSource, TSample>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TSample> _sampler;

        public Sample(IObservable<TSource> source, IObservable<TSample> sampler)
        {
            _source = source;
            _sampler = sampler;
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

            private IDisposable _sourceSubscription;
            private IDisposable _samplerSubscription;

            private bool _hasValue;
            private TSource _value;
            private bool _sourceAtEnd;
            private bool _samplerAtEnd;

            public IDisposable Run(Sample<TSource, TSample> parent)
            {
                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = parent._source.SubscribeSafe(this);

                var samplerSubscription = new SingleAssignmentDisposable();
                _samplerSubscription = samplerSubscription;
                samplerSubscription.Disposable = parent._sampler.SubscribeSafe(new SampleObserver(this));

                return StableCompositeDisposable.Create(_sourceSubscription, _samplerSubscription);
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
                    _sourceAtEnd = true;

                    if (_samplerAtEnd)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                    else
                        _sourceSubscription.Dispose();
                }
            }

            private sealed class SampleObserver : IObserver<TSample>
            {
                private readonly _ _parent;

                public SampleObserver(_ parent)
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

                        if (_parent._sourceAtEnd)
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
                        _parent._samplerAtEnd = true;

                        if (_parent._hasValue)
                        {
                            _parent._hasValue = false;
                            _parent._observer.OnNext(_parent._value);
                        }

                        if (_parent._sourceAtEnd)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                        else
                            _parent._samplerSubscription.Dispose();
                    }
                }
            }
        }
    }

    internal sealed class Sample<TSource> : Producer<TSource, Sample<TSource>._>
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private object _gate = new object();

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            private IDisposable _sourceSubscription;

            private bool _hasValue;
            private TSource _value;
            private bool _atEnd;

            public IDisposable Run(Sample<TSource> parent)
            {
                var sourceSubscription = new SingleAssignmentDisposable();
                _sourceSubscription = sourceSubscription;
                sourceSubscription.Disposable = parent._source.SubscribeSafe(this);

                return StableCompositeDisposable.Create(
                    sourceSubscription,
                    parent._scheduler.SchedulePeriodic(parent._interval, Tick)
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
