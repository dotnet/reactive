// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class SequenceEqual<TSource>
    {
        internal sealed class Observable : Producer<bool, Observable._>
        {
            private readonly IObservable<TSource> _first;
            private readonly IObservable<TSource> _second;
            private readonly IEqualityComparer<TSource> _comparer;

            public Observable(IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer)
            {
                _first = first;
                _second = second;
                _comparer = comparer;
            }

            protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(_comparer, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<bool>
            {
                private readonly IEqualityComparer<TSource> _comparer;

                public _(IEqualityComparer<TSource> comparer, IObserver<bool> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _comparer = comparer;
                }

                private object _gate;
                private bool _donel;
                private bool _doner;
                private Queue<TSource> _ql;
                private Queue<TSource> _qr;

                public IDisposable Run(Observable parent)
                {
                    _gate = new object();
                    _donel = false;
                    _doner = false;
                    _ql = new Queue<TSource>();
                    _qr = new Queue<TSource>();

                    return StableCompositeDisposable.Create
                    (
                        parent._first.SubscribeSafe(new FirstObserver(this)),
                        parent._second.SubscribeSafe(new SecondObserver(this))
                    );
                }

                private sealed class FirstObserver : IObserver<TSource>
                {
                    private readonly _ _parent;

                    public FirstObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public void OnNext(TSource value)
                    {
                        lock (_parent._gate)
                        {
                            if (_parent._qr.Count > 0)
                            {
                                var equal = false;
                                var v = _parent._qr.Dequeue();
                                try
                                {
                                    equal = _parent._comparer.Equals(value, v);
                                }
                                catch (Exception exception)
                                {
                                    _parent._observer.OnError(exception);
                                    _parent.Dispose();
                                    return;
                                }
                                if (!equal)
                                {
                                    _parent._observer.OnNext(false);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                            }
                            else if (_parent._doner)
                            {
                                _parent._observer.OnNext(false);
                                _parent._observer.OnCompleted();
                                _parent.Dispose();
                            }
                            else
                                _parent._ql.Enqueue(value);
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
                            _parent._donel = true;
                            if (_parent._ql.Count == 0)
                            {
                                if (_parent._qr.Count > 0)
                                {
                                    _parent._observer.OnNext(false);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                                else if (_parent._doner)
                                {
                                    _parent._observer.OnNext(true);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                            }
                        }
                    }
                }

                private sealed class SecondObserver : IObserver<TSource>
                {
                    private readonly _ _parent;

                    public SecondObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public void OnNext(TSource value)
                    {
                        lock (_parent._gate)
                        {
                            if (_parent._ql.Count > 0)
                            {
                                var equal = false;
                                var v = _parent._ql.Dequeue();
                                try
                                {
                                    equal = _parent._comparer.Equals(v, value);
                                }
                                catch (Exception exception)
                                {
                                    _parent._observer.OnError(exception);
                                    _parent.Dispose();
                                    return;
                                }
                                if (!equal)
                                {
                                    _parent._observer.OnNext(false);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                            }
                            else if (_parent._donel)
                            {
                                _parent._observer.OnNext(false);
                                _parent._observer.OnCompleted();
                                _parent.Dispose();
                            }
                            else
                                _parent._qr.Enqueue(value);
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
                            _parent._doner = true;
                            if (_parent._qr.Count == 0)
                            {
                                if (_parent._ql.Count > 0)
                                {
                                    _parent._observer.OnNext(false);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                                else if (_parent._donel)
                                {
                                    _parent._observer.OnNext(true);
                                    _parent._observer.OnCompleted();
                                    _parent.Dispose();
                                }
                            }
                        }
                    }
                }
            }
        }

        internal sealed class Enumerable : Producer<bool, Enumerable._>
        {
            private readonly IObservable<TSource> _first;
            private readonly IEnumerable<TSource> _second;
            private readonly IEqualityComparer<TSource> _comparer;

            public Enumerable(IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
            {
                _first = first;
                _second = second;
                _comparer = comparer;
            }

            protected override _ CreateSink(IObserver<bool> observer, IDisposable cancel) => new _(_comparer, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<bool>, IObserver<TSource>
            {
                private readonly IEqualityComparer<TSource> _comparer;

                public _(IEqualityComparer<TSource> comparer, IObserver<bool> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _comparer = comparer;
                }

                private IEnumerator<TSource> _enumerator;

                public IDisposable Run(Enumerable parent)
                {
                    //
                    // Notice the evaluation order of obtaining the enumerator and subscribing to the
                    // observable sequence is reversed compared to the operator's signature. This is
                    // required to make sure the enumerator is available as soon as the observer can
                    // be called. Otherwise, we end up having a race for the initialization and use
                    // of the _rightEnumerator field.
                    //
                    try
                    {
                        _enumerator = parent._second.GetEnumerator();
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return Disposable.Empty;
                    }

                    return StableCompositeDisposable.Create(
                        parent._first.SubscribeSafe(this),
                        _enumerator
                    );
                }

                public void OnNext(TSource value)
                {
                    var equal = false;

                    try
                    {
                        if (_enumerator.MoveNext())
                        {
                            var current = _enumerator.Current;
                            equal = _comparer.Equals(value, current);
                        }
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return;
                    }

                    if (!equal)
                    {
                        base._observer.OnNext(false);
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }

                public void OnError(Exception error)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }

                public void OnCompleted()
                {
                    var hasNext = false;

                    try
                    {
                        hasNext = _enumerator.MoveNext();
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return;
                    }

                    base._observer.OnNext(!hasNext);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
