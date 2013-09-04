// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class DefaultIfEmpty<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly TSource _defaultValue;

        public DefaultIfEmpty(IObservable<TSource> source, TSource defaultValue)
        {
            _source = source;
            _defaultValue = defaultValue;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly DefaultIfEmpty<TSource> _parent;
            private bool _found;

            public _(DefaultIfEmpty<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _found = false;
            }

            public void OnNext(TSource value)
            {
                _found = true;
                base._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_found)
                    base._observer.OnNext(_parent._defaultValue);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif