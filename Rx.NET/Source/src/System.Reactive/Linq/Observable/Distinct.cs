// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Distinct<TSource, TKey> : Producer<TSource, Distinct<TSource, TKey>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public Distinct(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly HashSet<TKey> _hashSet;

            public _(Distinct<TSource, TKey> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _keySelector = parent._keySelector;
                _hashSet = new HashSet<TKey>(parent._comparer);
            }

            public override void OnNext(TSource value)
            {
                TKey key;
                var hasAdded = false;
                try
                {
                    key = _keySelector(value);
                    hasAdded = _hashSet.Add(key);
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);
                    return;
                }

                if (hasAdded)
                {
                    ForwardOnNext(value);
                }
            }
        }
    }
}
