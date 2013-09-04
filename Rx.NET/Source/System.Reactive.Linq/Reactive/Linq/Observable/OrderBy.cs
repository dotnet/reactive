// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class OrderBy<TSource, TKey> : OrderedProducer<TSource>, IOrderedObservable<TSource>
    {
        private readonly Func<TSource, IObservable<TKey>> _timeSelector;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;

        public OrderBy(IObservable<TSource> source, Func<TSource, IObservable<TKey>> timeSelector, bool descending)
            : base(source, null)
        {
            _timeSelector = timeSelector;
            _descending = descending;
        }

        public OrderBy(IObservable<TSource> source, Func<TSource, IObservable<TKey>> timeSelector, bool descending, OrderedProducer<TSource> previous)
            : base(source, previous)
        {
            _timeSelector = timeSelector;
            _descending = descending;
        }

        public OrderBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            : base(source, null)
        {
            _keySelector = keySelector;
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
        }

        public OrderBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending, OrderedProducer<TSource> previous)
            : base(source, previous)
        {
            _keySelector = keySelector;
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
        }

        public IOrderedObservable<TSource> CreateOrderedObservable<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderBy<TSource, TKey>(base._source, keySelector, comparer, descending, previous: this);
        }

        public IOrderedObservable<TSource> CreateOrderedObservable<TOther>(Func<TSource, IObservable<TOther>> timeSelector, bool descending)
        {
            return new OrderBy<TSource, TOther>(base._source, timeSelector, descending, previous: this);
        }

        protected override SortSink Sort(IObserver<TSource> observer, IDisposable cancel)
        {
            if (_timeSelector != null)
            {
                if (_descending)
                {
                    return new Descending(this, observer, cancel);
                }
                else
                {
                    return new Ascending(this, observer, cancel);
                }
            }
            else
            {
                var sink = observer as ι;

                if (sink != null)
                {
                    /* This optimization exists for 2 reasons: 
                     * 
                     * 1. To avoid having to use multiple buffers in consecutive ordering operations.
                     * 2. To take advantage of Enumerable's optimizations for consecutive ordering operations.
                     */
                    sink.OrderBy(this);
                    return sink;
                }
                else
                {
                    if (_descending)
                    {
                        return new Descending_(this, observer, cancel);
                    }
                    else
                    {
                        return new Ascending_(this, observer, cancel);
                    }
                }
            }
        }

        class Ascending : ρ
        {
            public Ascending(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(parent, observer, cancel)
            {
            }

            protected override void Consume(TSource value)
            {
                base._observer.OnNext(value);
            }

            protected override void Complete()
            {
                base._observer.OnCompleted();
            }
        }

        class Descending : ρ
        {
            public Descending(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(parent, observer, cancel)
            {
            }

            private IList<TSource> _list;

            public override IDisposable Initialize()
            {
                _list = new List<TSource>();

                return base.Initialize();
            }

            protected override void Consume(TSource value)
            {
                _list.Add(value);
            }

            protected override void Complete()
            {
                foreach (var value in _list.Reverse())
                {
                    base._observer.OnNext(value);
                }
                base._observer.OnCompleted();
            }
        }

        class Ascending_ : ι
        {
            public Ascending_(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(parent, observer, cancel)
            {
            }

            protected override IOrderedEnumerable<TSource> OrderBy(IEnumerable<TSource> source)
            {
                return source.OrderBy(_parent._keySelector, _parent._comparer);
            }

            protected override IOrderedEnumerable<TSource> ThenBy(IOrderedEnumerable<TSource> source)
            {
                return source.ThenBy(_parent._keySelector, _parent._comparer);
            }
        }

        class Descending_ : ι
        {
            public Descending_(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(parent, observer, cancel)
            {
            }

            protected override IOrderedEnumerable<TSource> OrderBy(IEnumerable<TSource> source)
            {
                return source.OrderByDescending(base._parent._keySelector, base._parent._comparer);
            }

            protected override IOrderedEnumerable<TSource> ThenBy(IOrderedEnumerable<TSource> source)
            {
                return source.ThenByDescending(base._parent._keySelector, base._parent._comparer);
            }
        }

        /// <summary>
        /// Reactive sorting.  This code is based on the code from the SelectMany operator (8/11/2013).
        /// </summary>
        abstract class ρ : SortSink
        {
            protected readonly OrderBy<TSource, TKey> _parent;

            public ρ(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private bool _isStopped;
            private CompositeDisposable _group;
            private SingleAssignmentDisposable _sourceSubscription;

            public override IDisposable Initialize()
            {
                _gate = new object();
                _isStopped = false;
                _group = new CompositeDisposable();

                _sourceSubscription = new SingleAssignmentDisposable();
                _group.Add(_sourceSubscription);

                return _group;
            }

            public override void Run(IObservable<TSource> source)
            {
                _sourceSubscription.Disposable = source.SubscribeSafe(this);
            }

            public override void OnNext(TSource value)
            {
                var collection = default(IObservable<TKey>);

                try
                {
                    collection = _parent._timeSelector(value);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(ex);
                        base.Dispose();
                    }
                    return;
                }

                var innerSubscription = new SingleAssignmentDisposable();
                _group.Add(innerSubscription);
                innerSubscription.Disposable = collection.SubscribeSafe(new ι(this, value, innerSubscription));
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public override void OnCompleted()
            {
                _isStopped = true;
                if (_group.Count == 1)
                {
                    //
                    // Notice there can be a race between OnCompleted of the source and any
                    // of the inner sequences, where both see _group.Count == 1, and one is
                    // waiting for the lock. There won't be a double OnCompleted observation
                    // though, because the call to Dispose silences the observer by swapping
                    // in a NopObserver<T>.
                    //
                    lock (_gate)
                    {
                        Complete();
                        base.Dispose();
                    }
                }
                else
                {
                    _sourceSubscription.Dispose();
                }
            }

            protected abstract void Complete();

            protected abstract void Consume(TSource value);

            class ι : IObserver<TKey>
            {
                private readonly ρ _parent;
                private readonly TSource _value;
                private readonly IDisposable _self;

                public ι(ρ parent, TSource value, IDisposable self)
                {
                    _parent = parent;
                    _value = value;
                    _self = self;
                }

                public void OnNext(TKey value)
                {
                    OnCompleted();
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
                        _parent.Consume(_value);
                    }

                    _parent._group.Remove(_self);
                    if (_parent._isStopped && _parent._group.Count == 1)
                    {
                        //
                        // Notice there can be a race between OnCompleted of the source and any
                        // of the inner sequences, where both see _group.Count == 1, and one is
                        // waiting for the lock. There won't be a double OnCompleted observation
                        // though, because the call to Dispose silences the observer by swapping
                        // in a NopObserver<T>.
                        //
                        lock (_parent._gate)
                        {
                            _parent.Complete();
                            _parent.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aggregates before sorting.  This code is based on the code from the ToList operator (8/11/2013).
        /// </summary>
        abstract class ι : SortSink
        {
            protected readonly OrderBy<TSource, TKey> _parent;

            public ι(OrderBy<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private List<TSource> _list;
            private Stack<OrderBy<TSource, TKey>> _orderBy;
            private SingleAssignmentDisposable _sourceSubscription;

            public override IDisposable Initialize()
            {
                _list = new List<TSource>();
                _orderBy = new Stack<OrderBy<TSource, TKey>>();
                _sourceSubscription = new SingleAssignmentDisposable();

                return _sourceSubscription;
            }

            public override void Run(IObservable<TSource> source)
            {
                _sourceSubscription.Disposable = source.SubscribeSafe(this);
            }

            public override void OnNext(TSource value)
            {
                _list.Add(value);
            }

            public override void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public override void OnCompleted()
            {
                foreach (var value in OrderAll(_list))
                {
                    base._observer.OnNext(value);
                }
                base._observer.OnCompleted();
                base.Dispose();
            }

            protected abstract IOrderedEnumerable<TSource> OrderBy(IEnumerable<TSource> source);

            protected abstract IOrderedEnumerable<TSource> ThenBy(IOrderedEnumerable<TSource> source);

            internal void OrderBy(OrderBy<TSource, TKey> parent)
            {
                _orderBy.Push(parent);
            }

            private IEnumerable<TSource> OrderAll(IEnumerable<TSource> source)
            {
                IOrderedEnumerable<TSource> ordered = null;

                foreach (var parent in _orderBy)
                {
                    if (ordered == null)
                    {
                        ordered = parent._descending
                                ? source.OrderByDescending(parent._keySelector, parent._comparer)
                                : source.OrderBy(parent._keySelector, parent._comparer);
                    }
                    else
                    {
                        ordered = parent._descending
                                ? ordered.ThenByDescending(parent._keySelector, parent._comparer)
                                : ordered.ThenBy(parent._keySelector, parent._comparer);
                    }
                }

                return ordered == null ? OrderBy(source) : ThenBy(ordered);
            }
        }
    }
}