// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class GroupByUntil<TSource, TKey, TElement, TDuration> : Producer<IGroupedObservable<TKey, TElement>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> _durationSelector;
        private readonly int? _capacity;
        private readonly IEqualityComparer<TKey> _comparer;

        public GroupByUntil(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int? capacity, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
            _durationSelector = durationSelector;
            _capacity = capacity;
            _comparer = comparer;
        }

        private CompositeDisposable _groupDisposable;
        private RefCountDisposable _refCountDisposable;

        protected override IDisposable Run(IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            _groupDisposable = new CompositeDisposable();
            _refCountDisposable = new RefCountDisposable(_groupDisposable);

            var sink = new _(this, observer, cancel);
            setSink(sink);
            _groupDisposable.Add(_source.SubscribeSafe(sink));

            return _refCountDisposable;
        }

        class _ : Sink<IGroupedObservable<TKey, TElement>>, IObserver<TSource>
        {
            private readonly GroupByUntil<TSource, TKey, TElement, TDuration> _parent;
            private readonly Map<TKey, ISubject<TElement>> _map;
            private ISubject<TElement> _null;
            private object _nullGate;

            public _(GroupByUntil<TSource, TKey, TElement, TDuration> parent, IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _map = new Map<TKey, ISubject<TElement>>(_parent._capacity, _parent._comparer);
                _nullGate = new object();
            }

            public void OnNext(TSource value)
            {
                var key = default(TKey);
                try
                {
                    key = _parent._keySelector(value);
                }
                catch (Exception exception)
                {
                    Error(exception);
                    return;
                }

                var fireNewMapEntry = false;
                var writer = default(ISubject<TElement>);
                try
                {
                    //
                    // Note: The box instruction in the IL will be erased by the JIT in case T is
                    //       a value type. In fact, the whole if block will go away and we'll end
                    //       up with nothing but the GetOrAdd call below.
                    //
                    //       See GroupBy for more information and confirmation of this fact using
                    //       the SOS debugger extension.
                    //
                    if (key == null)
                    {
                        lock (_nullGate)
                        {
                            if (_null == null)
                            {
                                _null = new Subject<TElement>();
                                fireNewMapEntry = true;
                            }

                            writer = _null;
                        }
                    }
                    else
                    {
                        writer = _map.GetOrAdd(key, () => new Subject<TElement>(), out fireNewMapEntry);
                    }
                }
                catch (Exception exception)
                {
                    Error(exception);
                    return;
                }

                if (fireNewMapEntry)
                {
                    var group = new GroupedObservable<TKey, TElement>(key, writer, _parent._refCountDisposable);

                    var duration = default(IObservable<TDuration>);

                    var durationGroup = new GroupedObservable<TKey, TElement>(key, writer);
                    try
                    {
                        duration = _parent._durationSelector(durationGroup);
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                        return;
                    }

                    lock (base._observer)
                        base._observer.OnNext(group);

                    var md = new SingleAssignmentDisposable();
                    _parent._groupDisposable.Add(md);
                    md.Disposable = duration.SubscribeSafe(new δ(this, key, writer, md));
                }

                var element = default(TElement);
                try
                {
                    element = _parent._elementSelector(value);
                }
                catch (Exception exception)
                {
                    Error(exception);
                    return;
                }

                //
                // ISSUE: Rx v1.x shipped without proper handling of the case where the duration
                //        sequence fires concurrently with the OnNext code path here. In such a
                //        case, the subject can be completed before we get a chance to send out
                //        a new element. However, a resurrected group for the same key won't get
                //        to see the element either. To guard against this case, we'd have to
                //        check whether the OnNext call below lost the race, and resurrect a new
                //        group if needed. Unfortunately, this complicates matters when the
                //        duration selector triggers synchronously (e.g. Return or Empty), which
                //        causes the group to terminate immediately. We should not get stuck in
                //        this case, repeatedly trying to resurrect a group that always ends
                //        before we can send the element into it. Also, users may expect this
                //        base case to mean no elements will ever be produced, so sending the
                //        element into the group before starting the duration sequence may not
                //        be a good idea either. For the time being, we'll leave this as-is and
                //        revisit the behavior for vNext. Nonetheless, we'll add synchronization
                //        to ensure no concurrent calls to the subject are made.
                //
                lock (writer)
                    writer.OnNext(element);
            }

            class δ : IObserver<TDuration>
            {
                private readonly _ _parent;
                private readonly TKey _key;
                private readonly ISubject<TElement> _writer;
                private readonly IDisposable _self;

                public δ(_ parent, TKey key, ISubject<TElement> writer, IDisposable self)
                {
                    _parent = parent;
                    _key = key;
                    _writer = writer;
                    _self = self;
                }

                public void OnNext(TDuration value)
                {
                    OnCompleted();
                }

                public void OnError(Exception error)
                {
                    _parent.Error(error);
                    _self.Dispose();
                }

                public void OnCompleted()
                {
                    if (_key == null)
                    {
                        var @null = default(ISubject<TElement>);
                        lock (_parent._nullGate)
                        {
                            @null = _parent._null;
                            _parent._null = null;
                        }

                        lock (@null)
                            @null.OnCompleted();
                    }
                    else
                    {
                        if (_parent._map.Remove(_key))
                        {
                            lock (_writer)
                                _writer.OnCompleted();
                        }
                    }

                    _parent._parent._groupDisposable.Remove(_self);
                }
            }

            public void OnError(Exception error)
            {
                Error(error);
            }

            public void OnCompleted()
            {
                //
                // NOTE: A race with OnCompleted triggered by a duration selector is fine when
                //       using Subject<T>. It will transition into a terminal state, making one
                //       of the two calls a no-op by swapping in a DoneObserver<T>.
                //
                var @null = default(ISubject<TElement>);
                lock (_nullGate)
                    @null = _null;

                if (@null != null)
                    @null.OnCompleted();

                foreach (var w in _map.Values)
                    w.OnCompleted();

                lock (base._observer)
                    base._observer.OnCompleted();

                base.Dispose();
            }

            private void Error(Exception exception)
            {
                //
                // NOTE: A race with OnCompleted triggered by a duration selector is fine when
                //       using Subject<T>. It will transition into a terminal state, making one
                //       of the two calls a no-op by swapping in a DoneObserver<T>.
                //
                var @null = default(ISubject<TElement>);
                lock (_nullGate)
                    @null = _null;

                if (@null != null)
                    @null.OnError(exception);

                foreach (var w in _map.Values)
                    w.OnError(exception);

                lock (base._observer)
                    base._observer.OnError(exception);

                base.Dispose();
            }
        }
    }

#if !NO_CDS
    class Map<TKey, TValue>
    {
#if !NO_CDS_COLLECTIONS
        // Taken from Rx\NET\Source\System.Reactive.Core\Reactive\Internal\ConcurrentDictionary.cs

        // The default concurrency level is DEFAULT_CONCURRENCY_MULTIPLIER * #CPUs. The higher the
        // DEFAULT_CONCURRENCY_MULTIPLIER, the more concurrent writes can take place without interference
        // and blocking, but also the more expensive operations that require all locks become (e.g. table
        // resizing, ToArray, Count, etc). According to brief benchmarks that we ran, 4 seems like a good
        // compromise.
        private const int DEFAULT_CONCURRENCY_MULTIPLIER = 4;

        private static int DefaultConcurrencyLevel
        {
            get { return DEFAULT_CONCURRENCY_MULTIPLIER * Environment.ProcessorCount; }
        }
#endif

        private readonly System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> _map;

        public Map(int? capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity.HasValue)
            {
#if NO_CDS_COLLECTIONS
                _map = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>(capacity.Value, comparer);
#else
                _map = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>(DefaultConcurrencyLevel, capacity.Value, comparer);
#endif
            }
            else
            {
                _map = new System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>(comparer);
            }
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
        {
            added = false;

            var value = default(TValue);
            var newValue = default(TValue);
            var hasNewValue = false;
            while (true)
            {
                if (_map.TryGetValue(key, out value))
                    break;

                if (!hasNewValue)
                {
                    newValue = valueFactory();
                    hasNewValue = true;
                }

                if (_map.TryAdd(key, newValue))
                {
                    added = true;
                    value = newValue;
                    break;
                }
            }

            return value;
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                return _map.Values.ToArray();
            }
        }

        public bool Remove(TKey key)
        {
            var value = default(TValue);
            return _map.TryRemove(key, out value);
        }
    }
#else
    class Map<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _map;

        public Map(int? capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity.HasValue)
            {
                _map = new Dictionary<TKey, TValue>(capacity.Value, comparer);
            }
            else
            {
                _map = new Dictionary<TKey, TValue>(comparer);
            }
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
        {
            lock (_map)
            {
                added = false;

                var value = default(TValue);
                if (!_map.TryGetValue(key, out value))
                {
                    value = valueFactory();
                    _map.Add(key, value);
                    added = true;
                }

                return value;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                lock (_map)
                {
                    return _map.Values.ToArray();
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_map)
            {
                return _map.Remove(key);
            }
        }
    }
#endif
}
#endif