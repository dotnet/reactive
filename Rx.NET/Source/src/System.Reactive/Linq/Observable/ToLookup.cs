// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ToLookup<TSource, TKey, TElement> : Producer<ILookup<TKey, TElement>, ToLookup<TSource, TKey, TElement>._>
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

        protected override _ CreateSink(IObserver<ILookup<TKey, TElement>> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<ILookup<TKey, TElement>>, IObserver<TSource>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private readonly Lookup<TKey, TElement> _lookup;

            public _(ToLookup<TSource, TKey, TElement> parent, IObserver<ILookup<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;
                _lookup = new Lookup<TKey, TElement>(parent._comparer);
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _lookup.Add(_keySelector(value), _elementSelector(value));
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
