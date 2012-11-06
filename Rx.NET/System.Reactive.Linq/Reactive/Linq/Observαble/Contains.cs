// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;

namespace System.Reactive.Linq.Observαble
{
    class Contains<TSource> : Producer<bool>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _value;
        private readonly IEqualityComparer<TSource> _comparer;

        public Contains(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            _source = source;
            _value = value;
            _comparer = comparer;
        }

        protected override IDisposable Run(IObserver<bool> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<bool>, IObserver<TSource>
        {
            private readonly Contains<TSource> _parent;

            public _(Contains<TSource> parent, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var res = false;
                try
                {
                    res = _parent._comparer.Equals(value, _parent._value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (res)
                {
                    base._observer.OnNext(true);
                    base._observer.OnCompleted();
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
                base._observer.OnNext(false);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif