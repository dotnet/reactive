﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class GroupBy<TSource, TKey, TElement> : Producer<IGroupedObservable<TKey, TElement>, GroupBy<TSource, TKey, TElement>._>
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

        protected override _ CreateSink(IObserver<IGroupedObservable<TKey, TElement>> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, IGroupedObservable<TKey, TElement>>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private readonly Grouping<TKey, TElement> _map;

            private RefCountDisposable? _refCountDisposable;
            private Subject<TElement>? _null;

            public _(GroupBy<TSource, TKey, TElement> parent, IObserver<IGroupedObservable<TKey, TElement>> observer)
                : base(observer)
            {
                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;

                if (parent._capacity.HasValue)
                {
                    _map = new Grouping<TKey, TElement>(parent._capacity.Value, parent._comparer);
                }
                else
                {
                    _map = new Grouping<TKey, TElement>(parent._comparer);
                }
            }

            public override void Run(IObservable<TSource> source)
            {
                var sourceSubscription = new SingleAssignmentDisposable();
                _refCountDisposable = new RefCountDisposable(sourceSubscription);
                sourceSubscription.Disposable = source.SubscribeSafe(this);

                SetUpstream(_refCountDisposable);
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
                Subject<TElement>? writer;
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
                    var group = new GroupedObservable<TKey, TElement>(key, writer, _refCountDisposable!); // NB: _refCountDisposable is set in Run.
                    ForwardOnNext(group);
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

                writer.OnNext(element);
            }

            public override void OnError(Exception error)
            {
                Error(error);
            }

            public override void OnCompleted()
            {
                _null?.OnCompleted();

                foreach (var w in _map.Values)
                {
                    w.OnCompleted();
                }

                ForwardOnCompleted();
            }

            private void Error(Exception exception)
            {
                _null?.OnError(exception);

                foreach (var w in _map.Values)
                {
                    w.OnError(exception);
                }

                ForwardOnError(exception);
            }
        }
    }
}
