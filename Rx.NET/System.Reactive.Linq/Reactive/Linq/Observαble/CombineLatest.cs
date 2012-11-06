// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    #region Binary

    class CombineLatest<TFirst, TSecond, TResult> : Producer<TResult>
    {
        private readonly IObservable<TFirst> _first;
        private readonly IObservable<TSecond> _second;
        private readonly Func<TFirst, TSecond, TResult> _resultSelector;

        public CombineLatest(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            _first = first;
            _second = second;
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
            private readonly CombineLatest<TFirst, TSecond, TResult> _parent;

            public _(CombineLatest<TFirst, TSecond, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;

            public IDisposable Run()
            {
                _gate = new object();

                var fstSubscription = new SingleAssignmentDisposable();
                var sndSubscription = new SingleAssignmentDisposable();

                var fstO = new F(this, fstSubscription);
                var sndO = new S(this, sndSubscription);

                fstO.Other = sndO;
                sndO.Other = fstO;

                fstSubscription.Disposable = _parent._first.SubscribeSafe(fstO);
                sndSubscription.Disposable = _parent._second.SubscribeSafe(sndO);

                return new CompositeDisposable(fstSubscription, sndSubscription);
            }

            class F : IObserver<TFirst>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;
                private S _other;

                public F(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public S Other { set { _other = value; } }

                public bool HasValue { get; private set; }
                public TFirst Value { get; private set; }
                public bool Done { get; private set; }

                public void OnNext(TFirst value)
                {
                    lock (_parent._gate)
                    {
                        HasValue = true;
                        Value = value;

                        if (_other.HasValue)
                        {
                            var res = default(TResult);
                            try
                            {
                                res = _parent._parent._resultSelector(value, _other.Value);
                            }
                            catch (Exception ex)
                            {
                                _parent._observer.OnError(ex);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
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
                        Done = true;

                        if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
                        else
                        {
                            _self.Dispose();
                        }
                    }
                }
            }

            class S : IObserver<TSecond>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;
                private F _other;

                public S(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public F Other { set { _other = value; } }

                public bool HasValue { get; private set; }
                public TSecond Value { get; private set; }
                public bool Done { get; private set; }

                public void OnNext(TSecond value)
                {
                    lock (_parent._gate)
                    {
                        HasValue = true;
                        Value = value;

                        if (_other.HasValue)
                        {
                            var res = default(TResult);
                            try
                            {
                                res = _parent._parent._resultSelector(_other.Value, value);
                            }
                            catch (Exception ex)
                            {
                                _parent._observer.OnError(ex);
                                _parent.Dispose();
                                return;
                            }

                            _parent._observer.OnNext(res);
                        }
                        else if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
                        }
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
                        Done = true;

                        if (_other.Done)
                        {
                            _parent._observer.OnCompleted();
                            _parent.Dispose();
                            return;
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

    #endregion

    #region [3,16]-ary

    /* The following code is generated by a tool checked in to $/.../Source/Tools/CodeGenerators. */

    #region CombineLatest auto-generated code (6/10/2012 7:22:14 PM)

    class CombineLatest<T1, T2, T3, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly Func<T1, T2, T3, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, Func<T1, T2, T3, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(3, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[3];
                for (int i = 0; i < 3; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly Func<T1, T2, T3, T4, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(4, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[4];
                for (int i = 0; i < 4; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value);
            }
        }
    }

#if !NO_LARGEARITY

    class CombineLatest<T1, T2, T3, T4, T5, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly Func<T1, T2, T3, T4, T5, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(5, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[5];
                for (int i = 0; i < 5; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly Func<T1, T2, T3, T4, T5, T6, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(6, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[6];
                for (int i = 0; i < 6; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(7, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[7];
                for (int i = 0; i < 7; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(8, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[8];
                for (int i = 0; i < 8; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(9, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[9];
                for (int i = 0; i < 9; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(10, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[10];
                for (int i = 0; i < 10; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(11, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[11];
                for (int i = 0; i < 11; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly IObservable<T12> _source12;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(12, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[12];
                for (int i = 0; i < 12; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);
                _observer12 = new CombineLatestObserver<T12>(_gate, this, 11, subscriptions[11]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);
                subscriptions[11].Disposable = _parent._source12.SubscribeSafe(_observer12);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value, _observer12.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly IObservable<T12> _source12;
        private readonly IObservable<T13> _source13;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(13, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[13];
                for (int i = 0; i < 13; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);
                _observer12 = new CombineLatestObserver<T12>(_gate, this, 11, subscriptions[11]);
                _observer13 = new CombineLatestObserver<T13>(_gate, this, 12, subscriptions[12]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);
                subscriptions[11].Disposable = _parent._source12.SubscribeSafe(_observer12);
                subscriptions[12].Disposable = _parent._source13.SubscribeSafe(_observer13);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value, _observer12.Value, _observer13.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly IObservable<T12> _source12;
        private readonly IObservable<T13> _source13;
        private readonly IObservable<T14> _source14;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(14, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[14];
                for (int i = 0; i < 14; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);
                _observer12 = new CombineLatestObserver<T12>(_gate, this, 11, subscriptions[11]);
                _observer13 = new CombineLatestObserver<T13>(_gate, this, 12, subscriptions[12]);
                _observer14 = new CombineLatestObserver<T14>(_gate, this, 13, subscriptions[13]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);
                subscriptions[11].Disposable = _parent._source12.SubscribeSafe(_observer12);
                subscriptions[12].Disposable = _parent._source13.SubscribeSafe(_observer13);
                subscriptions[13].Disposable = _parent._source14.SubscribeSafe(_observer14);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value, _observer12.Value, _observer13.Value, _observer14.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly IObservable<T12> _source12;
        private readonly IObservable<T13> _source13;
        private readonly IObservable<T14> _source14;
        private readonly IObservable<T15> _source15;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _source15 = source15;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(15, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;
            private CombineLatestObserver<T15> _observer15;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[15];
                for (int i = 0; i < 15; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);
                _observer12 = new CombineLatestObserver<T12>(_gate, this, 11, subscriptions[11]);
                _observer13 = new CombineLatestObserver<T13>(_gate, this, 12, subscriptions[12]);
                _observer14 = new CombineLatestObserver<T14>(_gate, this, 13, subscriptions[13]);
                _observer15 = new CombineLatestObserver<T15>(_gate, this, 14, subscriptions[14]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);
                subscriptions[11].Disposable = _parent._source12.SubscribeSafe(_observer12);
                subscriptions[12].Disposable = _parent._source13.SubscribeSafe(_observer13);
                subscriptions[13].Disposable = _parent._source14.SubscribeSafe(_observer14);
                subscriptions[14].Disposable = _parent._source15.SubscribeSafe(_observer15);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value, _observer12.Value, _observer13.Value, _observer14.Value, _observer15.Value);
            }
        }
    }

    class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : Producer<TResult>
    {
        private readonly IObservable<T1> _source1;
        private readonly IObservable<T2> _source2;
        private readonly IObservable<T3> _source3;
        private readonly IObservable<T4> _source4;
        private readonly IObservable<T5> _source5;
        private readonly IObservable<T6> _source6;
        private readonly IObservable<T7> _source7;
        private readonly IObservable<T8> _source8;
        private readonly IObservable<T9> _source9;
        private readonly IObservable<T10> _source10;
        private readonly IObservable<T11> _source11;
        private readonly IObservable<T12> _source12;
        private readonly IObservable<T13> _source13;
        private readonly IObservable<T14> _source14;
        private readonly IObservable<T15> _source15;
        private readonly IObservable<T16> _source16;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _resultSelector;

        public CombineLatest(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, IObservable<T16> source16, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> resultSelector)
        {
            _source1 = source1;
            _source2 = source2;
            _source3 = source3;
            _source4 = source4;
            _source5 = source5;
            _source6 = source6;
            _source7 = source7;
            _source8 = source8;
            _source9 = source9;
            _source10 = source10;
            _source11 = source11;
            _source12 = source12;
            _source13 = source13;
            _source14 = source14;
            _source15 = source15;
            _source16 = source16;
            _resultSelector = resultSelector;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : CombineLatestSink<TResult>
        {
            private readonly CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _parent;

            public _(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(16, observer, cancel)
            {
                _parent = parent;
            }

            private CombineLatestObserver<T1> _observer1;
            private CombineLatestObserver<T2> _observer2;
            private CombineLatestObserver<T3> _observer3;
            private CombineLatestObserver<T4> _observer4;
            private CombineLatestObserver<T5> _observer5;
            private CombineLatestObserver<T6> _observer6;
            private CombineLatestObserver<T7> _observer7;
            private CombineLatestObserver<T8> _observer8;
            private CombineLatestObserver<T9> _observer9;
            private CombineLatestObserver<T10> _observer10;
            private CombineLatestObserver<T11> _observer11;
            private CombineLatestObserver<T12> _observer12;
            private CombineLatestObserver<T13> _observer13;
            private CombineLatestObserver<T14> _observer14;
            private CombineLatestObserver<T15> _observer15;
            private CombineLatestObserver<T16> _observer16;

            public IDisposable Run()
            {
                var subscriptions = new SingleAssignmentDisposable[16];
                for (int i = 0; i < 16; i++)
                    subscriptions[i] = new SingleAssignmentDisposable();

                _observer1 = new CombineLatestObserver<T1>(_gate, this, 0, subscriptions[0]);
                _observer2 = new CombineLatestObserver<T2>(_gate, this, 1, subscriptions[1]);
                _observer3 = new CombineLatestObserver<T3>(_gate, this, 2, subscriptions[2]);
                _observer4 = new CombineLatestObserver<T4>(_gate, this, 3, subscriptions[3]);
                _observer5 = new CombineLatestObserver<T5>(_gate, this, 4, subscriptions[4]);
                _observer6 = new CombineLatestObserver<T6>(_gate, this, 5, subscriptions[5]);
                _observer7 = new CombineLatestObserver<T7>(_gate, this, 6, subscriptions[6]);
                _observer8 = new CombineLatestObserver<T8>(_gate, this, 7, subscriptions[7]);
                _observer9 = new CombineLatestObserver<T9>(_gate, this, 8, subscriptions[8]);
                _observer10 = new CombineLatestObserver<T10>(_gate, this, 9, subscriptions[9]);
                _observer11 = new CombineLatestObserver<T11>(_gate, this, 10, subscriptions[10]);
                _observer12 = new CombineLatestObserver<T12>(_gate, this, 11, subscriptions[11]);
                _observer13 = new CombineLatestObserver<T13>(_gate, this, 12, subscriptions[12]);
                _observer14 = new CombineLatestObserver<T14>(_gate, this, 13, subscriptions[13]);
                _observer15 = new CombineLatestObserver<T15>(_gate, this, 14, subscriptions[14]);
                _observer16 = new CombineLatestObserver<T16>(_gate, this, 15, subscriptions[15]);

                subscriptions[0].Disposable = _parent._source1.SubscribeSafe(_observer1);
                subscriptions[1].Disposable = _parent._source2.SubscribeSafe(_observer2);
                subscriptions[2].Disposable = _parent._source3.SubscribeSafe(_observer3);
                subscriptions[3].Disposable = _parent._source4.SubscribeSafe(_observer4);
                subscriptions[4].Disposable = _parent._source5.SubscribeSafe(_observer5);
                subscriptions[5].Disposable = _parent._source6.SubscribeSafe(_observer6);
                subscriptions[6].Disposable = _parent._source7.SubscribeSafe(_observer7);
                subscriptions[7].Disposable = _parent._source8.SubscribeSafe(_observer8);
                subscriptions[8].Disposable = _parent._source9.SubscribeSafe(_observer9);
                subscriptions[9].Disposable = _parent._source10.SubscribeSafe(_observer10);
                subscriptions[10].Disposable = _parent._source11.SubscribeSafe(_observer11);
                subscriptions[11].Disposable = _parent._source12.SubscribeSafe(_observer12);
                subscriptions[12].Disposable = _parent._source13.SubscribeSafe(_observer13);
                subscriptions[13].Disposable = _parent._source14.SubscribeSafe(_observer14);
                subscriptions[14].Disposable = _parent._source15.SubscribeSafe(_observer15);
                subscriptions[15].Disposable = _parent._source16.SubscribeSafe(_observer16);

                return new CompositeDisposable(subscriptions);
            }

            protected override TResult GetResult()
            {
                return _parent._resultSelector(_observer1.Value, _observer2.Value, _observer3.Value, _observer4.Value, _observer5.Value, _observer6.Value, _observer7.Value, _observer8.Value, _observer9.Value, _observer10.Value, _observer11.Value, _observer12.Value, _observer13.Value, _observer14.Value, _observer15.Value, _observer16.Value);
            }
        }
    }

#endif

    #endregion

    #region Helpers for n-ary overloads

    interface ICombineLatest
    {
        void Next(int index);
        void Fail(Exception error);
        void Done(int index);
    }

    abstract class CombineLatestSink<TResult> : Sink<TResult>, ICombineLatest
    {
        protected readonly object _gate;

        private bool _hasValueAll;
        private readonly bool[] _hasValue;
        private readonly bool[] _isDone;

        public CombineLatestSink(int arity, IObserver<TResult> observer, IDisposable cancel)
            : base(observer, cancel)
        {
            _gate = new object();

            _hasValue = new bool[arity];
            _isDone = new bool[arity];
        }

        public void Next(int index)
        {
            if (!_hasValueAll)
            {
                _hasValue[index] = true;

                var hasValueAll = true;
                foreach (var hasValue in _hasValue)
                {
                    if (!hasValue)
                    {
                        hasValueAll = false;
                        break;
                    }
                }

                _hasValueAll = hasValueAll;
            }

            if (_hasValueAll)
            {
                var res = default(TResult);
                try
                {
                    res = GetResult();
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(res);
            }
            else
            {
                var allOthersDone = true;
                for (int i = 0; i < _isDone.Length; i++)
                {
                    if (i != index && !_isDone[i])
                    {
                        allOthersDone = false;
                        break;
                    }
                }

                if (allOthersDone)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        protected abstract TResult GetResult();

        public void Fail(Exception error)
        {
            base._observer.OnError(error);
            base.Dispose();
        }

        public void Done(int index)
        {
            _isDone[index] = true;

            var allDone = true;
            foreach (var isDone in _isDone)
            {
                if (!isDone)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone)
            {
                base._observer.OnCompleted();
                base.Dispose();
                return;
            }
        }
    }

    class CombineLatestObserver<T> : IObserver<T>
    {
        private readonly object _gate;
        private readonly ICombineLatest _parent;
        private readonly int _index;
        private readonly IDisposable _self;
        private T _value;

        public CombineLatestObserver(object gate, ICombineLatest parent, int index, IDisposable self)
        {
            _gate = gate;
            _parent = parent;
            _index = index;
            _self = self;
        }

        public T Value
        {
            get { return _value; }
        }

        public void OnNext(T value)
        {
            lock (_gate)
            {
                _value = value;
                _parent.Next(_index);
            }
        }

        public void OnError(Exception error)
        {
            _self.Dispose();

            lock (_gate)
            {
                _parent.Fail(error);
            }
        }

        public void OnCompleted()
        {
            _self.Dispose();

            lock (_gate)
            {
                _parent.Done(_index);
            }
        }
    }

    #endregion

    #endregion

    #region N-ary

    class CombineLatest<TSource, TResult> : Producer<TResult>
    {
        private readonly IEnumerable<IObservable<TSource>> _sources;
        private readonly Func<IList<TSource>, TResult> _resultSelector;

        public CombineLatest(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            _sources = sources;
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
            private readonly CombineLatest<TSource, TResult> _parent;

            public _(CombineLatest<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool[] _hasValue;
            private bool _hasValueAll;
            private List<TSource> _values;
            private bool[] _isDone;
            private IDisposable[] _subscriptions;

            public IDisposable Run()
            {
                var srcs = _parent._sources.ToArray();

                var N = srcs.Length;

                _hasValue = new bool[N];
                _hasValueAll = false;

                _values = new List<TSource>(N);
                for (int i = 0; i < N; i++)
                    _values.Add(default(TSource));

                _isDone = new bool[N];

                _subscriptions = new IDisposable[N];

                _gate = new object();

                for (int i = 0; i < N; i++)
                {
                    var j = i;

                    var d = new SingleAssignmentDisposable();
                    _subscriptions[j] = d;

                    var o = new O(this, j);
                    d.Disposable = srcs[j].SubscribeSafe(o);
                }

                return new CompositeDisposable(_subscriptions);
            }

            private void OnNext(int index, TSource value)
            {
                lock (_gate)
                {
                    _values[index] = value;

                    _hasValue[index] = true;

                    if (_hasValueAll || (_hasValueAll = _hasValue.All(Stubs<bool>.I)))
                    {
                        var res = default(TResult);
                        try
                        {
                            res = _parent._resultSelector(new ReadOnlyCollection<TSource>(_values));
                        }
                        catch (Exception ex)
                        {
                            base._observer.OnError(ex);
                            base.Dispose();
                            return;
                        }

                        _observer.OnNext(res);
                    }
                    else if (_isDone.Where((x, i) => i != index).All(Stubs<bool>.I))
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return;
                    }
                }
            }

            private void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            private void OnCompleted(int index)
            {
                lock (_gate)
                {
                    _isDone[index] = true;

                    if (_isDone.All(Stubs<bool>.I))
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return;
                    }
                    else
                    {
                        _subscriptions[index].Dispose();
                    }
                }
            }

            class O : IObserver<TSource>
            {
                private readonly _ _parent;
                private readonly int _index;

                public O(_ parent, int index)
                {
                    _parent = parent;
                    _index = index;
                }

                public void OnNext(TSource value)
                {
                    _parent.OnNext(_index, value);
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnCompleted()
                {
                    _parent.OnCompleted(_index);
                }
            }
        }
    }

    #endregion
}
#endif