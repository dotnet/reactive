// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class SequenceEqual<TSource> : Producer<bool>
    {
        private readonly IObservable<TSource> _first;
        private readonly IObservable<TSource> _second;
        private readonly IEnumerable<TSource> _secondE;
        private readonly IEqualityComparer<TSource> _comparer;

        public SequenceEqual(IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            _first = first;
            _second = second;
            _comparer = comparer;
        }

        public SequenceEqual(IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            _first = first;
            _secondE = second;
            _comparer = comparer;
        }

        protected override IDisposable Run(IObserver<bool> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_second != null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                var sink = new ε(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class _ : Sink<bool>
        {
            private readonly SequenceEqual<TSource> _parent;

            public _(SequenceEqual<TSource> parent, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool _donel;
            private bool _doner;
            private Queue<TSource> _ql;
            private Queue<TSource> _qr;

            public IDisposable Run()
            {
                _gate = new object();
                _donel = false;
                _doner = false;
                _ql = new Queue<TSource>();
                _qr = new Queue<TSource>();

                return new CompositeDisposable
                {
                    _parent._first.SubscribeSafe(new F(this)),
                    _parent._second.SubscribeSafe(new S(this))
                };
            }

            class F : IObserver<TSource>
            {
                private readonly _ _parent;

                public F(_ parent)
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
                                equal = _parent._parent._comparer.Equals(value, v);
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
                    _parent._observer.OnError(error);
                    _parent.Dispose();
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

            class S : IObserver<TSource>
            {
                private readonly _ _parent;

                public S(_ parent)
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
                                equal = _parent._parent._comparer.Equals(v, value);
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
                    _parent._observer.OnError(error);
                    _parent.Dispose();
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

        class ε : Sink<bool>, IObserver<TSource>
        {
            private readonly SequenceEqual<TSource> _parent;

            public ε(SequenceEqual<TSource> parent, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IEnumerator<TSource> _enumerator;

            public IDisposable Run()
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
                    _enumerator = _parent._secondE.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return Disposable.Empty;
                }

                return new CompositeDisposable(
                    _parent._first.SubscribeSafe(this),
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
                        equal = _parent._comparer.Equals(value, current);
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
#endif