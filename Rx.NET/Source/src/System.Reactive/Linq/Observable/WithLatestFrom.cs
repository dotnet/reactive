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

        protected override _ CreateSink(IObserver<TResult> observer) => new _(_resultSelector, observer);

        protected override void Run(_ sink) => sink.Run(_first, _second);

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly Func<TFirst, TSecond, TResult> _resultSelector;

            public _(Func<TFirst, TSecond, TResult> resultSelector, IObserver<TResult> observer)
                : base(observer)
            {
                _resultSelector = resultSelector;
            }

            private object _gate;
            private volatile bool _hasLatest;
            private TSecond _latest;

            private object _latestGate;

            private IDisposable _secondDisposable;

            public void Run(IObservable<TFirst> first, IObservable<TSecond> second)
            {
                _gate = new object();
                _latestGate = new object();

                var fstO = new FirstObserver(this);
                var sndO = new SecondObserver(this);

                Disposable.SetSingle(ref _secondDisposable, second.SubscribeSafe(sndO));
                SetUpstream(first.SubscribeSafe(fstO));
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Disposable.TryDispose(ref _secondDisposable);
                }
                base.Dispose(disposing);
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
                        _parent.ForwardOnCompleted();
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent.ForwardOnError(error);
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

                        TResult res;

                        try
                        {
                            res = _parent._resultSelector(value, latest);
                        }
                        catch (Exception ex)
                        {
                            lock (_parent._gate)
                            {
                                _parent.ForwardOnError(ex);
                            }

                            return;
                        }

                        lock (_parent._gate)
                        {
                            _parent.ForwardOnNext(res);
                        }
                    }
                }
            }

            private sealed class SecondObserver : IObserver<TSecond>
            {
                private readonly _ _parent;

                public SecondObserver(_ parent)
                {
                    _parent = parent;
                }

                public void OnCompleted()
                {
                    Disposable.TryDispose(ref _parent._secondDisposable);
                }

                public void OnError(Exception error)
                {
                    lock (_parent._gate)
                    {
                        _parent.ForwardOnError(error);
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
