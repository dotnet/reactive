// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> : Producer<TResult, Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._>
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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TResult>
        {
            private readonly object _gate = new object();
            private readonly CompositeDisposable _group = new CompositeDisposable();
            private readonly SortedDictionary<int, TLeft> _leftMap = new SortedDictionary<int, TLeft>();
            private readonly SortedDictionary<int, TRight> _rightMap = new SortedDictionary<int, TRight>();

            private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
            private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
            private readonly Func<TLeft, TRight, TResult> _resultSelector;

            public _(Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _leftDurationSelector = parent._leftDurationSelector;
                _rightDurationSelector = parent._rightDurationSelector;
                _resultSelector = parent._resultSelector;
            }

            private bool _leftDone;
            private int _leftID;

            private bool _rightDone;
            private int _rightID;

            public IDisposable Run(Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent)
            {
                var leftSubscription = new SingleAssignmentDisposable();
                _group.Add(leftSubscription);
                _leftDone = false;
                _leftID = 0;

                var rightSubscription = new SingleAssignmentDisposable();
                _group.Add(rightSubscription);
                _rightDone = false;
                _rightID = 0;

                leftSubscription.Disposable = parent._left.SubscribeSafe(new LeftObserver(this, leftSubscription));
                rightSubscription.Disposable = parent._right.SubscribeSafe(new RightObserver(this, rightSubscription));

                return _group;
            }

            private sealed class LeftObserver : IObserver<TLeft>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public LeftObserver(_ parent, IDisposable self)
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
                    var rightID = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._leftID++;
                        rightID = _parent._rightID;
                        _parent._leftMap.Add(id, value);
                    }

                    var md = new SingleAssignmentDisposable();
                    _parent._group.Add(md);

                    var duration = default(IObservable<TLeftDuration>);
                    try
                    {
                        duration = _parent._leftDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        _parent._observer.OnError(exception);
                        _parent.Dispose();
                        return;
                    }

                    md.Disposable = duration.SubscribeSafe(new DurationObserver(this, id, md));

                    lock (_parent._gate)
                    {
                        foreach (var rightValue in _parent._rightMap)
                        {
                            if (rightValue.Key < rightID)
                            {
                                var result = default(TResult);
                                try
                                {
                                    result = _parent._resultSelector(value, rightValue.Value);
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
                }

                private sealed class DurationObserver : IObserver<TLeftDuration>
                {
                    private readonly LeftObserver _parent;
                    private readonly int _id;
                    private readonly IDisposable _self;

                    public DurationObserver(LeftObserver parent, int id, IDisposable self)
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

            private sealed class RightObserver : IObserver<TRight>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public RightObserver(_ parent, IDisposable self)
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
                    var leftID = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._rightID++;
                        leftID = _parent._leftID;
                        _parent._rightMap.Add(id, value);
                    }

                    var md = new SingleAssignmentDisposable();
                    _parent._group.Add(md);

                    var duration = default(IObservable<TRightDuration>);
                    try
                    {
                        duration = _parent._rightDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        _parent._observer.OnError(exception);
                        _parent.Dispose();
                        return;
                    }

                    md.Disposable = duration.SubscribeSafe(new DurationObserver(this, id, md));

                    lock (_parent._gate)
                    {
                        foreach (var leftValue in _parent._leftMap)
                        {
                            if (leftValue.Key < leftID)
                            {
                                var result = default(TResult);
                                try
                                {
                                    result = _parent._resultSelector(leftValue.Value, value);
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
                }

                private sealed class DurationObserver : IObserver<TRightDuration>
                {
                    private readonly RightObserver _parent;
                    private readonly int _id;
                    private readonly IDisposable _self;

                    public DurationObserver(RightObserver parent, int id, IDisposable self)
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
