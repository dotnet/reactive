// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Amb<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _left;
        private readonly IObservable<TSource> _right;

        public Amb(IObservable<TSource> left, IObservable<TSource> right)
        {
            _left = left;
            _right = right;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>
        {
            private readonly Amb<TSource> _parent;

            public _(Amb<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private AmbState _choice;

            public IDisposable Run()
            {
                var ls = new SingleAssignmentDisposable();
                var rs = new SingleAssignmentDisposable();
                var d = new CompositeDisposable(ls, rs);

                var gate = new object();

                var lo = new AmbObserver();
                lo._disposable = d;
                lo._target = new DecisionObserver(this, gate, AmbState.Left, ls, rs, lo);

                var ro = new AmbObserver();
                ro._disposable = d;
                ro._target = new DecisionObserver(this, gate, AmbState.Right, rs, ls, ro);

                _choice = AmbState.Neither;

                ls.Disposable = _parent._left.SubscribeSafe(lo);
                rs.Disposable = _parent._right.SubscribeSafe(ro);

                return d;
            }

            class DecisionObserver : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly AmbState _me;
                private readonly IDisposable _subscription;
                private readonly IDisposable _otherSubscription;
                private readonly object _gate;
                private readonly AmbObserver _observer;

                public DecisionObserver(_ parent, object gate, AmbState me, IDisposable subscription, IDisposable otherSubscription, AmbObserver observer)
                {
                    _parent = parent;
                    _gate = gate;
                    _me = me;
                    _subscription = subscription;
                    _otherSubscription = otherSubscription;
                    _observer = observer;
                }

                public void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        if (_parent._choice == AmbState.Neither)
                        {
                            _parent._choice = _me;
                            _otherSubscription.Dispose();
                            _observer._disposable = _subscription;
                            _observer._target = _parent._observer;
                        }

                        if (_parent._choice == _me)
                            _parent._observer.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        if (_parent._choice == AmbState.Neither)
                        {
                            _parent._choice = _me;
                            _otherSubscription.Dispose();
                            _observer._disposable = _subscription;
                            _observer._target = _parent._observer;
                        }

                        if (_parent._choice == _me)
                        {
                            _parent._observer.OnError(error);
                            _parent.Dispose();
                        }
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        if (_parent._choice == AmbState.Neither)
                        {
                            _parent._choice = _me;
                            _otherSubscription.Dispose();
                            _observer._disposable = _subscription;
                            _observer._target = _parent._observer;
                        }

                        if (_parent._choice == _me)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }
                }
            }

            class AmbObserver : IObserver<TSource>
            {
                public IObserver<TSource> _target;

                public IDisposable _disposable;

                public void OnNext(TSource value)
                {
                    _target.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    _target.OnError(error);
                    _disposable.Dispose();
                }

                public void OnCompleted()
                {
                    _target.OnCompleted();
                    _disposable.Dispose();
                }
            }

            enum AmbState
            {
                Left,
                Right,
                Neither
            }
        }
    }
}
#endif