// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult> : Producer<TResult, GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>._>
    {
        private readonly IObservable<TLeft> _left;
        private readonly IObservable<TRight> _right;
        private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
        private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
        private readonly Func<TLeft, IObservable<TRight>, TResult> _resultSelector;

        public GroupJoin(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
            _left = left;
            _right = right;
            _leftDurationSelector = leftDurationSelector;
            _rightDurationSelector = rightDurationSelector;
            _resultSelector = resultSelector;
        }

        protected override _ CreateSink(IObserver<TResult> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly object _gate = new object();
            private readonly CompositeDisposable _group = new CompositeDisposable();
            private readonly RefCountDisposable _refCount;
            private readonly SortedDictionary<int, IObserver<TRight>> _leftMap;
            private readonly SortedDictionary<int, TRight> _rightMap;

            private readonly Func<TLeft, IObservable<TLeftDuration>> _leftDurationSelector;
            private readonly Func<TRight, IObservable<TRightDuration>> _rightDurationSelector;
            private readonly Func<TLeft, IObservable<TRight>, TResult> _resultSelector;

            public _(GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent, IObserver<TResult> observer)
                : base(observer)
            {
                _refCount = new RefCountDisposable(_group);
                _leftMap = new SortedDictionary<int, IObserver<TRight>>();
                _rightMap = new SortedDictionary<int, TRight>();

                _leftDurationSelector = parent._leftDurationSelector;
                _rightDurationSelector = parent._rightDurationSelector;
                _resultSelector = parent._resultSelector;
            }

            private int _leftID;
            private int _rightID;

            public void Run(GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult> parent)
            {
                var leftObserver = new LeftObserver(this);
                _group.Add(leftObserver);

                var rightObserver = new RightObserver(this);
                _group.Add(rightObserver);

                leftObserver.SetResource(parent._left.SubscribeSafe(leftObserver));
                rightObserver.SetResource(parent._right.SubscribeSafe(rightObserver));

                SetUpstream(_refCount);
            }

            private sealed class LeftObserver : SafeObserver<TLeft>
            {
                private readonly _ _parent;

                public LeftObserver(_ parent)
                {
                    _parent = parent;
                }

                private void Expire(int id, IObserver<TRight> group, IDisposable resource)
                {
                    lock (_parent._gate)
                    {
                        if (_parent._leftMap.Remove(id))
                        {
                            group.OnCompleted();
                        }
                    }

                    _parent._group.Remove(resource);
                }

                public override void OnNext(TLeft value)
                {
                    var s = new Subject<TRight>();
                    var id = 0;
                    var rightID = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._leftID++;
                        rightID = _parent._rightID;
                        _parent._leftMap.Add(id, s);
                    }

                    var window = new WindowObservable<TRight>(s, _parent._refCount);

                    // BREAKING CHANGE v2 > v1.x - Order of evaluation or the _leftDurationSelector and _resultSelector now consistent with Join.

                    var duration = default(IObservable<TLeftDuration>);
                    try
                    {
                        duration = _parent._leftDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        OnError(exception);
                        return;
                    }

                    var durationObserver = new DurationObserver(this, id, s);
                    _parent._group.Add(durationObserver);
                    // BREAKING CHANGE v2 > v1.x - The duration sequence is subscribed to before the result sequence is evaluated.
                    durationObserver.SetResource(duration.SubscribeSafe(durationObserver));

                    TResult result;
                    try
                    {
                        result = _parent._resultSelector(value, window);
                    }
                    catch (Exception exception)
                    {
                        OnError(exception);
                        return;
                    }

                    lock (_parent._gate)
                    {
                        _parent.ForwardOnNext(result);

                        foreach (var rightValue in _parent._rightMap)
                        {
                            if (rightValue.Key < rightID)
                            {
                                s.OnNext(rightValue.Value);
                            }
                        }
                    }
                }

                private sealed class DurationObserver : SafeObserver<TLeftDuration>
                {
                    private readonly LeftObserver _parent;
                    private readonly int _id;
                    private readonly IObserver<TRight> _group;

                    public DurationObserver(LeftObserver parent, int id, IObserver<TRight> group)
                    {
                        _parent = parent;
                        _id = id;
                        _group = group;
                    }

                    public override void OnNext(TLeftDuration value)
                    {
                        _parent.Expire(_id, _group, this);
                    }

                    public override void OnError(Exception error)
                    {
                        _parent.OnError(error);
                    }

                    public override void OnCompleted()
                    {
                        _parent.Expire(_id, _group, this);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        foreach (var o in _parent._leftMap)
                        {
                            o.Value.OnError(error);
                        }

                        _parent.ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        _parent.ForwardOnCompleted();
                    }

                    Dispose();
                }
            }

            private sealed class RightObserver : SafeObserver<TRight>
            {
                private readonly _ _parent;

                public RightObserver(_ parent)
                {
                    _parent = parent;
                }

                private void Expire(int id, IDisposable resource)
                {
                    lock (_parent._gate)
                    {
                        _parent._rightMap.Remove(id);
                    }

                    _parent._group.Remove(resource);
                }

                public override void OnNext(TRight value)
                {
                    var id = 0;
                    var leftID = 0;
                    lock (_parent._gate)
                    {
                        id = _parent._rightID++;
                        leftID = _parent._leftID;
                        _parent._rightMap.Add(id, value);
                    }

                    var duration = default(IObservable<TRightDuration>);
                    try
                    {
                        duration = _parent._rightDurationSelector(value);
                    }
                    catch (Exception exception)
                    {
                        OnError(exception);
                        return;
                    }

                    var durationObserver = new DurationObserver(this, id);
                    _parent._group.Add(durationObserver);
                    durationObserver.SetResource(duration.SubscribeSafe(durationObserver));

                    lock (_parent._gate)
                    {
                        foreach (var o in _parent._leftMap)
                        {
                            if (o.Key < leftID)
                            {
                                o.Value.OnNext(value);
                            }
                        }
                    }
                }

                private sealed class DurationObserver : SafeObserver<TRightDuration>
                {
                    private readonly RightObserver _parent;
                    private readonly int _id;

                    public DurationObserver(RightObserver parent, int id)
                    {
                        _parent = parent;
                        _id = id;
                    }

                    public override void OnNext(TRightDuration value)
                    {
                        _parent.Expire(_id, this);
                    }

                    public override void OnError(Exception error)
                    {
                        _parent.OnError(error);
                    }

                    public override void OnCompleted()
                    {
                        _parent.Expire(_id, this);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        foreach (var o in _parent._leftMap)
                        {
                            o.Value.OnError(error);
                        }

                        _parent.ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    Dispose();
                }
            }
        }
    }
}
