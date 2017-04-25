// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Switch<TSource> : Producer<TSource, Switch<TSource>._>
    {
        private readonly IObservable<IObservable<TSource>> _sources;

        public Switch(IObservable<IObservable<TSource>> sources)
        {
            _sources = sources;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<IObservable<TSource>>
        {
            private readonly object _gate = new object();

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            private IDisposable _subscription;
            private SerialDisposable _innerSubscription;
            private bool _isStopped;
            private ulong _latest;
            private bool _hasLatest;

            public IDisposable Run(Switch<TSource> parent)
            {
                _innerSubscription = new SerialDisposable();
                _isStopped = false;
                _latest = 0UL;
                _hasLatest = false;

                var subscription = new SingleAssignmentDisposable();
                _subscription = subscription;
                subscription.Disposable = parent._sources.SubscribeSafe(this);

                return StableCompositeDisposable.Create(_subscription, _innerSubscription);
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
                d.Disposable = value.SubscribeSafe(new InnerObserver(this, id, d));
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

            private sealed class InnerObserver : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly ulong _id;
                private readonly IDisposable _self;

                public InnerObserver(_ parent, ulong id, IDisposable self)
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
                        {
                            _parent._observer.OnNext(value);
                        }
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
