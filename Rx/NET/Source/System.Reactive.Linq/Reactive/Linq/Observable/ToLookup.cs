// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq.Observαble
{
    class ToLookup<TSource, TKey, TElement> : Producer<ILookup<TKey, TElement>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public ToLookup(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
            _comparer = comparer;
        }

        protected override IDisposable Run(IObserver<ILookup<TKey, TElement>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<ILookup<TKey, TElement>>, IObserver<TSource>
        {
            private readonly ToLookup<TSource, TKey, TElement> _parent;
            private Lookup<TKey, TElement> _lookup;

            public _(ToLookup<TSource, TKey, TElement> parent, IObserver<ILookup<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _lookup = new Lookup<TKey, TElement>(_parent._comparer);
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _lookup.Add(_parent._keySelector(value), _parent._elementSelector(value));
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
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
                base._observer.OnNext(_lookup);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif