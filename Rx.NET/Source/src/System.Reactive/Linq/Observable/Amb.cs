// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Amb<TSource> : Producer<TSource, Amb<TSource>._>
    {
        private readonly IObservable<TSource> _left;
        private readonly IObservable<TSource> _right;

        public Amb(IObservable<TSource> left, IObservable<TSource> right)
        {
            _left = left;
            _right = right;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            private AmbState _choice;

            public IDisposable Run(Amb<TSource> parent)
            {
                var ls = new SingleAssignmentDisposable();
                var rs = new SingleAssignmentDisposable();
                var d = StableCompositeDisposable.Create(ls, rs);

                var gate = new object();

                var lo = new AmbObserver();
                lo._disposable = d;
                lo._target = new DecisionObserver(this, gate, AmbState.Left, ls, rs, lo);

                var ro = new AmbObserver();
                ro._disposable = d;
                ro._target = new DecisionObserver(this, gate, AmbState.Right, rs, ls, ro);

                _choice = AmbState.Neither;

                ls.Disposable = parent._left.SubscribeSafe(lo);
                rs.Disposable = parent._right.SubscribeSafe(ro);

                return d;
            }

            private sealed class DecisionObserver : IObserver<TSource>
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
                        {
                            _parent._observer.OnNext(value);
                        }
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

            private sealed class AmbObserver : IObserver<TSource>
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

            private enum AmbState
            {
                Left,
                Right,
                Neither,
            }
        }
    }
}
