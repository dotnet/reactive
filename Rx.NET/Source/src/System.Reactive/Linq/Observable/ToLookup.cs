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

        protected override _ CreateSink(IObserver<ILookup<TKey, TElement>> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, ILookup<TKey, TElement>>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly Func<TSource, TElement> _elementSelector;
            private Lookup<TKey, TElement> _lookup;

            public _(ToLookup<TSource, TKey, TElement> parent, IObserver<ILookup<TKey, TElement>> observer)
                : base(observer)
            {
                _keySelector = parent._keySelector;
                _elementSelector = parent._elementSelector;
                _lookup = new Lookup<TKey, TElement>(parent._comparer);
            }

            public override void OnNext(TSource value)
            {
                try
                {
                    _lookup.Add(_keySelector(value), _elementSelector(value));
                }
                catch (Exception ex)
                {
                    _lookup = null;
                    ForwardOnError(ex);
                }
            }

            public override void OnError(Exception error)
            {
                _lookup = null;
                ForwardOnError(error);
            }

            public override void OnCompleted()
            {
                var lookup = _lookup;
                _lookup = null;
                ForwardOnNext(lookup);
                ForwardOnCompleted();
            }
        }
    }
}
