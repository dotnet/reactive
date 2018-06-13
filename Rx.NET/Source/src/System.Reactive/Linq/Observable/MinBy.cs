// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class MinBy<TSource, TKey> : Producer<IList<TSource>, MinBy<TSource, TKey>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;

        public MinBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource, IList<TSource>> 
        {
            private readonly MinBy<TSource, TKey> _parent;
            private bool _hasValue;
            private TKey _lastKey;
            private List<TSource> _list;

            public _(MinBy<TSource, TKey> parent, IObserver<IList<TSource>> observer)
                : base(observer)
            {
                _parent = parent;

                _hasValue = false;
                _lastKey = default(TKey);
                _list = new List<TSource>();
            }

            public override void OnNext(TSource value)
            {
                var key = default(TKey);
                try
                {
                    key = _parent._keySelector(value);
                }
                catch (Exception ex)
                {
                    ForwardOnError(ex);
                    return;
                }

                var comparison = 0;

                if (!_hasValue)
                {
                    _hasValue = true;
                    _lastKey = key;
                }
                else
                {
                    try
                    {
                        comparison = _parent._comparer.Compare(key, _lastKey);
                    }
                    catch (Exception ex)
                    {
                        ForwardOnError(ex);
                        return;
                    }
                }

                if (comparison < 0)
                {
                    _lastKey = key;
                    _list.Clear();
                }

                if (comparison <= 0)
                {
                    _list.Add(value);
                }
            }

            public override void OnCompleted()
            {
                ForwardOnNext(_list);
                ForwardOnCompleted();
            }
        }
    }
}
