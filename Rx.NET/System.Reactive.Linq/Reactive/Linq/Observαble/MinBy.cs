// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
    class MinBy<TSource, TKey> : Producer<IList<TSource>>
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

        protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly MinBy<TSource, TKey> _parent;
            private bool _hasValue;
            private TKey _lastKey;
            private List<TSource> _list;

            public _(MinBy<TSource, TKey> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;

                _hasValue = false;
                _lastKey = default(TKey);
                _list = new List<TSource>();
            }

            public void OnNext(TSource value)
            {
                var key = default(TKey);
                try
                {
                    key = _parent._keySelector(value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
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
                        base._observer.OnError(ex);
                        base.Dispose();
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

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(_list);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif