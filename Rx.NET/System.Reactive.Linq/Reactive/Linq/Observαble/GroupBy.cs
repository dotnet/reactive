// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class GroupBy<TSource, TKey, TElement> : Producer<IGroupedObservable<TKey, TElement>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public GroupBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
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
            private readonly GroupBy<TSource, TKey, TElement> _parent;
            private readonly Dictionary<TKey, ISubject<TElement>> _map;

            public _(GroupBy<TSource, TKey, TElement> parent, IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _map = new Dictionary<TKey, ISubject<TElement>>(_parent._comparer);
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
                    if (!_map.TryGetValue(key, out writer))
                    {
                        writer = new Subject<TElement>();
                        _map.Add(key, writer);
                        fireNewMapEntry = true;
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
                    _observer.OnNext(group);
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

                writer.OnNext(element);
            }

            public void OnError(Exception error)
            {
                Error(error);
            }

            public void OnCompleted()
            {
                foreach (var w in _map.Values)
                    w.OnCompleted();

                base._observer.OnCompleted();
                base.Dispose();
            }

            private void Error(Exception exception)
            {
                foreach (var w in _map.Values)
                    w.OnError(exception);

                base._observer.OnError(exception);
                base.Dispose();
            }
        }
    }
}
#endif