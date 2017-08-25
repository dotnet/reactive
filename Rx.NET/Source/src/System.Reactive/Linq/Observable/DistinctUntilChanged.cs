// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class DistinctUntilChanged<TSource, TKey> : Producer<TSource, DistinctUntilChanged<TSource, TKey>._>
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Func<TSource, TKey> _keySelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private TKey _currentKey;
            private bool _hasCurrentKey;

            public _(DistinctUntilChanged<TSource, TKey> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _keySelector = parent._keySelector;
                _comparer = parent._comparer;

                _currentKey = default(TKey);
                _hasCurrentKey = false;
            }

            public void OnNext(TSource value)
            {
                var key = default(TKey);
                try
                {
                    key = _keySelector(value);
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
                        comparerEquals = _comparer.Equals(_currentKey, key);
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
