// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
    class DistinctUntilChanged<TSource, TKey> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _comparer;

        public DistinctUntilChanged(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly DistinctUntilChanged<TSource, TKey> _parent;
            private TKey _currentKey;
            private bool _hasCurrentKey;

            public _(DistinctUntilChanged<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _currentKey = default(TKey);
                _hasCurrentKey = false;
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
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                var comparerEquals = false;
                if (_hasCurrentKey)
                {
                    try
                    {
                        comparerEquals = _parent._comparer.Equals(_currentKey, key);
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return;
                    }
                }

                if (!_hasCurrentKey || !comparerEquals)
                {
                    _hasCurrentKey = true;
                    _currentKey = key;
                    base._observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif