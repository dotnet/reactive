// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Any<TSource> : Producer<bool>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;

        public Any(IObservable<TSource> source)
        {
            _source = source;
        }

        public Any(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        protected override IDisposable Run(IObserver<bool> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_predicate != null)
            {
                var sink = new π(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new _(observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<bool>, IObserver<TSource>
        {
            public _(IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(true);
                base._observer.OnCompleted();
                base.Dispose();
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

        class π : Sink<bool>, IObserver<TSource>
        {
            private readonly Any<TSource> _parent;

            public π(Any<TSource> parent, IObserver<bool> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var res = false;
                try
                {
                    res = _parent._predicate(value);
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