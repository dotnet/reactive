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

        protected override _ CreateSink(IObserver<IDictionary<TKey, TElement>> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, IDictionary<TKey, TElement>>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private Dictionary<TKey, TElement> _dictionary;

            public _(ToDictionary<TSource, TKey, TElement> parent, IObserver<IDictionary<TKey, TElement>> observer)
                : base(observer)
            {
                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;
                _dictionary = new Dictionary<TKey, TElement>(parent._comparer);
            }

            public override void OnNext(TSource value)
            {
                try
                {
                    _dictionary.Add(_keySelector(value), _elementSelector(value));
                }
                catch (Exception ex)
                {
                    _dictionary = null;
                    ForwardOnError(ex);
                }
            }

            public override void OnError(Exception error)
            {
                _dictionary = null;
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                var dictionary = _dictionary;
                _dictionary = null;
                ForwardOnNext(dictionary);
                ForwardOnCompleted();
            }
        }
    }
}
