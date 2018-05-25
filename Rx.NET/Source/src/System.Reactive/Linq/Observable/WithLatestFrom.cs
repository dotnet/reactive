// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class WithLatestFrom<TFirst, TSecond, TResult> : Producer<TResult, WithLatestFrom<TFirst, TSecond, TResult>._>
    {
        private readonly IObservable<TFirst> _first;
        private readonly IObservable<TSecond> _second;
        private readonly Func<TFirst, TSecond, TResult> _resultSelector;

        public WithLatestFrom(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            _first = first;
            _second = second;
            _resultSelector = resultSelector;
        }

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(_resultSelector, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_first, _second);

        internal sealed class _ : Sink<TResult>
        {
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;
            private volatile bool _hasLatest;
            private TSecond _latest;

            private object _latestGate;

            public IDisposable Run(IObservable<TFirst> first, IObservable<TSecond> second)
            {
                _gate = new object();
                _latestGate = new object();

                var sndSubscription = new SingleAssignmentDisposable();

                var fstO = new FirstObserver(this);
                var sndO = new SecondObserver(this, sndSubscription);

                sndSubscription.Disposable = second.SubscribeSafe(sndO);
                var fstSubscription = first.SubscribeSafe(fstO);

                return StableCompositeDisposable.Create(fstSubscription, sndSubscription);
            }

            private sealed class FirstObserver : IObserver<TFirst>
            {
                private readonly _ _parent;

                public FirstObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnCompleted()
                {
                    lock (_parent._gate)
                    {
                        _parent._observer.OnCompleted();
                        _parent.Dispose();
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

                public void OnNext(TFirst value)
                {
                    if (_parent._hasLatest) // Volatile read
                    {

                        TSecond latest;

                        lock (_parent._latestGate)
                        {
                            latest = _parent._latest;
                        }

                        var res = default(TResult);

                        try
                        {
                            res = _parent._resultSelector(value, latest);
                        }
                        catch (Exception ex)
                        {
                            lock (_parent._gate)
                            {
                                _parent._observer.OnError(ex);
                                _parent.Dispose();
                            }

                            return;
                        }

                        lock (_parent._gate)
                        {
                            _parent._observer.OnNext(res);
                        }
                    }
                }
            }

            private sealed class SecondObserver : IObserver<TSecond>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public SecondObserver(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnCompleted()
                {
                    _self.Dispose();
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent._observer.OnError(error);
                        _parent.Dispose();
                    }
                }

                public void OnNext(TSecond value)
                {
                    lock (_parent._latestGate)
                    {
                        _parent._latest = value;
                    }

                    if (!_parent._hasLatest)
                    {
                        _parent._hasLatest = true;
                    }
                }
            }
        }
    }
}
