// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class MaxBy<TSource, TKey> : Producer<IList<TSource>, MaxBy<TSource, TKey>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IComparer<TKey> _comparer;

        public MaxBy(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
        }

        protected override _ CreateSink(IObserver<IList<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly MaxBy<TSource, TKey> _parent;
            private bool _hasValue;
            private TKey _lastKey;
            private List<TSource> _list;

            public _(MaxBy<TSource, TKey> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
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

                if (comparison > 0)
                {
                    _lastKey = key;
                    _list.Clear();
                }

                if (comparison >= 0)
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
