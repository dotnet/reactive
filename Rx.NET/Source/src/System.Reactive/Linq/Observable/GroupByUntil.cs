// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class GroupByUntil<TSource, TKey, TElement, TDuration> : Producer<IGroupedObservable<TKey, TElement>, GroupByUntil<TSource, TKey, TElement, TDuration>._>
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

        protected override _ CreateSink(IObserver<IGroupedObservable<TKey, TElement>> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, IGroupedObservable<TKey, TElement>>
        {
            private readonly object _gate = new object();
            private readonly object _nullGate = new object();
            private readonly CompositeDisposable _groupDisposable = new CompositeDisposable();
            private readonly RefCountDisposable _refCountDisposable;
            private readonly Map<TKey, ISubject<TElement>> _map;

            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private readonly Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> _durationSelector;

            private ISubject<TElement> _null;

            public _(GroupByUntil<TSource, TKey, TElement, TDuration> parent, IObserver<IGroupedObservable<TKey, TElement>> observer)
                : base(observer)
            {
                _refCountDisposable = new RefCountDisposable(_groupDisposable);
                _map = new Map<TKey, ISubject<TElement>>(parent._capacity, parent._comparer);

                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;
                _durationSelector = parent._durationSelector;
            }

            public override void Run(IObservable<TSource> source)
            {
                _groupDisposable.Add(source.SubscribeSafe(this));

                SetUpstream(_refCountDisposable);
            }

            private ISubject<TElement> NewSubject()
            {
                var sub = new Subject<TElement>();

                return Subject.Create<TElement>(new AsyncLockObserver<TElement>(sub, new Concurrency.AsyncLock()), sub);
            }

            public override void OnNext(TSource value)
            {
                TKey key;
                try
                {
                    key = _keySelector(value);
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
                                _null = NewSubject();
                                fireNewMapEntry = true;
                            }

                            writer = _null;
                        }
                    }
                    else
                    {
                        writer = _map.GetOrAdd(key, NewSubject, out fireNewMapEntry);
                    }
                }
                catch (Exception exception)
                {
                    Error(exception);
                    return;
                }

                if (fireNewMapEntry)
                {
                    var group = new GroupedObservable<TKey, TElement>(key, writer, _refCountDisposable);

                    var duration = default(IObservable<TDuration>);

                    var durationGroup = new GroupedObservable<TKey, TElement>(key, writer);
                    try
                    {
                        duration = _durationSelector(durationGroup);
                    }
                    catch (Exception exception)
                    {
                        Error(exception);
                        return;
                    }

                    lock (_gate)
                    {
                        ForwardOnNext(group);
                    }

                    var durationObserver = new DurationObserver(this, key, writer);
                    _groupDisposable.Add(durationObserver);
                    durationObserver.SetResource(duration.SubscribeSafe(durationObserver));
                }

                TElement element;
                try
                {
                    element = _elementSelector(value);
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
                writer.OnNext(element);
            }

            private sealed class DurationObserver : SafeObserver<TDuration>
            {
                private readonly _ _parent;
                private readonly TKey _key;
                private readonly ISubject<TElement> _writer;

                public DurationObserver(_ parent, TKey key, ISubject<TElement> writer)
                {
                    _parent = parent;
                    _key = key;
                    _writer = writer;
                }

                public override void OnNext(TDuration value)
                {
                    OnCompleted();
                }

                public override void OnError(Exception error)
                {
                    _parent.Error(error);
                    Dispose();
                }

                public override void OnCompleted()
                {
                    if (_key == null)
                    {
                        var @null = default(ISubject<TElement>);
                        lock (_parent._nullGate)
                        {
                            @null = _parent._null;
                            _parent._null = null;
                        }

                        @null.OnCompleted();
                    }
                    else
                    {
                        if (_parent._map.Remove(_key))
                        {
                            _writer.OnCompleted();
                        }
                    }

                    _parent._groupDisposable.Remove(this);
                }
            }

            public override void OnError(Exception error)
            {
                Error(error);
            }

            public override void OnCompleted()
            {
                //
                // NOTE: A race with OnCompleted triggered by a duration selector is fine when
                //       using Subject<T>. It will transition into a terminal state, making one
                //       of the two calls a no-op by swapping in a DoneObserver<T>.
                //
                var @null = default(ISubject<TElement>);
                lock (_nullGate)
                {
                    @null = _null;
                }

                @null?.OnCompleted();

                foreach (var w in _map.Values)
                {
                    w.OnCompleted();
                }

                lock (_gate)
                {
                    ForwardOnCompleted();
                }
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
                {
                    @null = _null;
                }

                @null?.OnError(exception);

                foreach (var w in _map.Values)
                {
                    w.OnError(exception);
                }

                lock (_gate)
                {
                    ForwardOnError(exception);
                }
            }
        }
    }

    internal sealed class Map<TKey, TValue>
    {
        // Taken from ConcurrentDictionary in the BCL.

        // The default concurrency level is DEFAULT_CONCURRENCY_MULTIPLIER * #CPUs. The higher the
        // DEFAULT_CONCURRENCY_MULTIPLIER, the more concurrent writes can take place without interference
        // and blocking, but also the more expensive operations that require all locks become (e.g. table
        // resizing, ToArray, Count, etc). According to brief benchmarks that we ran, 4 seems like a good
        // compromise.
        private const int DefaultConcurrencyMultiplier = 4;

        private static int DefaultConcurrencyLevel => DefaultConcurrencyMultiplier * Environment.ProcessorCount;

        private readonly ConcurrentDictionary<TKey, TValue> _map;

        public Map(int? capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity.HasValue)
            {
                _map = new ConcurrentDictionary<TKey, TValue>(DefaultConcurrencyLevel, capacity.Value, comparer);
            }
            else
            {
                _map = new ConcurrentDictionary<TKey, TValue>(comparer);
            }
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
        {
            added = false;

            TValue value;
            var newValue = default(TValue);
            var hasNewValue = false;
            while (true)
            {
                if (_map.TryGetValue(key, out value))
                {
                    break;
                }

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

        public IEnumerable<TValue> Values => _map.Values.ToArray();

        public bool Remove(TKey key)
        {
            return _map.TryRemove(key, out _);
        }
    }
}
