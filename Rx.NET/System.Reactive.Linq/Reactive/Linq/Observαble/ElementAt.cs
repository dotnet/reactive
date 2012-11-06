// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class ElementAt<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _index;
        private readonly bool _throwOnEmpty;

        public ElementAt(IObservable<TSource> source, int index, bool throwOnEmpty)
        {
            _source = source;
            _index = index;
            _throwOnEmpty = throwOnEmpty;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly ElementAt<TSource> _parent;
            private int _i;

            public _(ElementAt<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _i = _parent._index;
            }

            public void OnNext(TSource value)
            {
                if (_i == 0)
                {
                    base._observer.OnNext(value);
                    base._observer.OnCompleted();
                    base.Dispose();
                }

                _i--;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (_parent._throwOnEmpty)
                {
                    base._observer.OnError(new ArgumentOutOfRangeException("index"));
                }
                else
                {
                    base._observer.OnNext(default(TSource));
                    base._observer.OnCompleted();
                }
                
                base.Dispose();
            }
        }
    }
}
#endif