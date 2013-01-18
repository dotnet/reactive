// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Switch<TSource> : Producer<TSource>
    {
        private readonly IObservable<IObservable<TSource>> _sources;

        public Switch(IObservable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<IObservable<TSource>>
        {
            private readonly Switch<TSource> _parent;

            public _(Switch<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private IDisposable _subscription;
            private SerialDisposable _innerSubscription;
            private bool _isStopped;
            private ulong _latest;
            private bool _hasLatest;

            public IDisposable Run()
            {
                _gate = new object();
                _innerSubscription = new SerialDisposable();
                _isStopped = false;
                _latest = 0UL;
                _hasLatest = false;

                var subscription = new SingleAssignmentDisposable();
                _subscription = subscription;
                subscription.Disposable = _parent._sources.SubscribeSafe(this);

                return new CompositeDisposable(_subscription, _innerSubscription);
            }

            public void OnNext(IObservable<TSource> value)
            {
                var id = default(ulong);
                lock (_gate)
                {
                    id = unchecked(++_latest);
                    _hasLatest = true;
                }

                var d = new SingleAssignmentDisposable();
                _innerSubscription.Disposable = d;
                d.Disposable = value.SubscribeSafe(new ι(this, id, d));
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                    base._observer.OnError(error);

                base.Dispose();
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    _subscription.Dispose();

                    _isStopped = true;
                    if (!_hasLatest)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }

            class ι : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly ulong _id;
                private readonly IDisposable _self;

                public ι(_ parent, ulong id, IDisposable self)
                {
                    _parent = parent;
                    _id = id;
                    _self = self;
                }

                public void OnNext(TSource value)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._latest == _id)
                            _parent._observer.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _self.Dispose();

                        if (_parent._latest == _id)
                        {
                            _parent._observer.OnError(error);
                            _parent.Dispose();
                        }
                    }
                }

                public void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        _self.Dispose();

                        if (_parent._latest == _id)
                        {
                            _parent._hasLatest = false;

                            if (_parent._isStopped)
                            {
                                _parent._observer.OnCompleted();
                                _parent.Dispose();
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif