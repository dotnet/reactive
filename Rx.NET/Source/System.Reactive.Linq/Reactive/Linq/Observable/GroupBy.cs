// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class GroupBy<TSource, TKey, TElement> : Producer<IGroupedObservable<TKey, TElement>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly int? _capacity;
        private readonly IEqualityComparer<TKey> _comparer;

        public GroupBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int? capacity, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
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
            private readonly GroupBy<TSource, TKey, TElement> _parent;
            private readonly Dictionary<TKey, ISubject<TElement>> _map;
            private ISubject<TElement> _null;

            public _(GroupBy<TSource, TKey, TElement> parent, IObserver<IGroupedObservable<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;

                if (_parent._capacity.HasValue)
                {
                    _map = new Dictionary<TKey, ISubject<TElement>>(_parent._capacity.Value, _parent._comparer);
                }
                else
                {
                    _map = new Dictionary<TKey, ISubject<TElement>>(_parent._comparer);
                }
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
                    //       up with nothing but the TryGetValue check below.
                    //
                    //       // var fireNewMapEntry = false;
                    //       C:\Projects\Rx\Rx\Experimental\Main\Source\Rx\System.Reactive.Linq\Reactive\Linq\Observable\GroupBy.cs @ 67:
                    //       000007fb`6d544b80 48c7452800000000 mov     qword ptr [rbp+28h],0
                    //
                    //       // var writer = default(ISubject<TElement>);
                    //       C:\Projects\Rx\Rx\Experimental\Main\Source\Rx\System.Reactive.Linq\Reactive\Linq\Observable\GroupBy.cs @ 66:
                    //       000007fb`6d544b88 c6453400        mov     byte ptr [rbp+34h],0
                    //
                    //       // if (!_map.TryGetValue(key, out writer))
                    //       C:\Projects\Rx\Rx\Experimental\Main\Source\Rx\System.Reactive.Linq\Reactive\Linq\Observable\GroupBy.cs @ 86:
                    //       000007fb`6d544b8c 488b4560        mov     rax,qword ptr [rbp+60h]
                    //       ...
                    //
                    if (key == null)
                    {
                        if (_null == null)
                        {
                            _null = new Subject<TElement>();
                            fireNewMapEntry = true;
                        }

                        writer = _null;
                    }
                    else
                    {
                        if (!_map.TryGetValue(key, out writer))
                        {
                            writer = new Subject<TElement>();
                            _map.Add(key, writer);
                            fireNewMapEntry = true;
                        }
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
                if (_null != null)
                    _null.OnCompleted();

                foreach (var w in _map.Values)
                    w.OnCompleted();

                base._observer.OnCompleted();
                base.Dispose();
            }

            private void Error(Exception exception)
            {
                if (_null != null)
                    _null.OnError(exception);

                foreach (var w in _map.Values)
                    w.OnError(exception);

                base._observer.OnError(exception);
                base.Dispose();
            }
        }
    }
}
#endif