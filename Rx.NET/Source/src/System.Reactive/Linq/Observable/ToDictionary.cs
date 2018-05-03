// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class ToDictionary<TSource, TKey, TElement> : Producer<IDictionary<TKey, TElement>, ToDictionary<TSource, TKey, TElement>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public ToDictionary(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<IDictionary<TKey, TElement>> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<IDictionary<TKey, TElement>>, IObserver<TSource>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private readonly Dictionary<TKey, TElement> _dictionary;

            public _(ToDictionary<TSource, TKey, TElement> parent, IObserver<IDictionary<TKey, TElement>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;
                _dictionary = new Dictionary<TKey, TElement>(parent._comparer);
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _dictionary.Add(_keySelector(value), _elementSelector(value));
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
                base._observer.OnNext(_dictionary);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
