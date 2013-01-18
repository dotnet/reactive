// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> : Producer<TResult>
    {
        private readonly IObservable<TLeft> _left;
        private readonly IObservable<TRight> _right;
        private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
        private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
        private readonly Func<TLeft, TRight, TResult> _resultSelector;

        public Join(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            _left = left;
            _right = right;
            _leftDurationSelector = leftDurationSelector;
            _rightDurationSelector = rightDurationSelector;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TResult>
        {
            private readonly Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> _parent;

            public _(Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private CompositeDisposable _group;

            private bool _leftDone;
            private int _leftID;
            private Dictionary<int, TLeft> _leftMap;
            
            private bool _rightDone;
            private int _rightID;
            private Dictionary<int, TRight> _rightMap;

            public IDisposable Run()
            {
                _gate = new object();
                _group = new CompositeDisposable();
                
                var leftSubscription = new SingleAssignmentDisposable();
                _group.Add(leftSubscription);
                _leftDone = false;
                _leftID = 0;
                _leftMap = new Dictionary<int, TLeft>();

                var rightSubscription = new SingleAssignmentDisposable();
                _group.Add(rightSubscription);
                _rightDone = false;
                _rightID = 0;
                _rightMap = new Dictionary<int, TRight>();

                leftSubscription.Disposable = _parent._left.SubscribeSafe(new λ(this, leftSubscription));
                rightSubscription.Disposable = _parent._right.SubscribeSafe(new ρ(this, rightSubscription));

                return _group;
            }

            class λ : IObserver<TLeft>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public λ(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                private void Expire(int id, IDisposable resource)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._leftMap.Remove(id) && _parent._leftMap.Count == 0 && _parent._leftDone)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }

                    _parent._group.Remove(resource);
                }

                public void OnNext(TLeft value)
                {
                    var id = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._leftID++;
                        _parent._leftMap.Add(id, value);
                    }

                    var md = new SingleAssignmentDisposable();
                    _parent._group.Add(md);

                    var duration = default(IObservable<TLeftDuration>);
                    try
                    {
                        duration = _parent._parent._leftDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        _parent._observer.OnError(exception);
                        _parent.Dispose();
                        return;
                    }

                    md.Disposable = duration.SubscribeSafe(new δ(this, id, md));

                    lock (_parent._gate)
                    {
                        foreach (var rightValue in _parent._rightMap.Values)
                        {
                            var result = default(TResult);
                            try
                            {
                                result = _parent._parent._resultSelector(value, rightValue);
                            }
                            catch (Exception exception)
                            {
                                _parent._observer.OnError(exception);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(result);
                        }
                    }
                }

                class δ : IObserver<TLeftDuration>
                {
                    private readonly λ _parent;
                    private readonly int _id;
                    private readonly IDisposable _self;

                    public δ(λ parent, int id, IDisposable self)
                    {
                        _parent = parent;
                        _id = id;
                        _self = self;
                    }

                    public void OnNext(TLeftDuration value)
                    {
                        _parent.Expire(_id, _self);
                    }

                    public void OnError(Exception error)
                    {
                        _parent.OnError(error);
                    }

                    public void OnCompleted()
                    {
                        _parent.Expire(_id, _self);
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
                        _parent._leftDone = true;
                        if (_parent._rightDone || _parent._leftMap.Count == 0)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                        else
                        {
                            _self.Dispose();
                        }
                    }
                }
            }

            class ρ : IObserver<TRight>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public ρ(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                private void Expire(int id, IDisposable resource)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._rightMap.Remove(id) && _parent._rightMap.Count == 0 && _parent._rightDone)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                    }

                    _parent._group.Remove(resource);
                }

                public void OnNext(TRight value)
                {
                    var id = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._rightID++;
                        _parent._rightMap.Add(id, value);
                    }

                    var md = new SingleAssignmentDisposable();
                    _parent._group.Add(md);

                    var duration = default(IObservable<TRightDuration>);
                    try
                    {
                        duration = _parent._parent._rightDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        _parent._observer.OnError(exception);
                        _parent.Dispose();
                        return;
                    }

                    md.Disposable = duration.SubscribeSafe(new δ(this, id, md));

                    lock (_parent._gate)
                    {
                        foreach (var leftValue in _parent._leftMap.Values)
                        {
                            var result = default(TResult);
                            try
                            {
                                result = _parent._parent._resultSelector(leftValue, value);
                            }
                            catch (Exception exception)
                            {
                                _parent._observer.OnError(exception);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(result);
                        }
                    }
                }

                class δ : IObserver<TRightDuration>
                {
                    private readonly ρ _parent;
                    private readonly int _id;
                    private readonly IDisposable _self;

                    public δ(ρ parent, int id, IDisposable self)
                    {
                        _parent = parent;
                        _id = id;
                        _self = self;
                    }

                    public void OnNext(TRightDuration value)
                    {
                        _parent.Expire(_id, _self);
                    }

                    public void OnError(Exception error)
                    {
                        _parent.OnError(error);
                    }

                    public void OnCompleted()
                    {
                        _parent.Expire(_id, _self);
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
                        _parent._rightDone = true;
                        if (_parent._leftDone || _parent._rightMap.Count == 0)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                        }
                        else
                        {
                            _self.Dispose();
                        }
                    }
                }
            }
        }
    }
}
#endif