// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Do<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Action<TSource> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public Do(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            _source = source;
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Do<TSource> _parent;

            public _(Do<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                try
                {
                    _parent._onNext(value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(value);
            }

            public void OnError(Exception error)
            {
                try
                {
                    _parent._onError(error);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                try
                {
                    _parent._onCompleted();
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif