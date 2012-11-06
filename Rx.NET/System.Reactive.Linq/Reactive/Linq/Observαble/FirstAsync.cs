// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class FirstAsync<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;
        private readonly bool _throwOnEmpty;

        public FirstAsync(IObservable<TSource> source, Func<TSource, bool> predicate, bool throwOnEmpty)
        {
            _source = source;
            _predicate = predicate;
            _throwOnEmpty = throwOnEmpty;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_predicate != null)
            {
                var sink = new π(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly FirstAsync<TSource> _parent;

            public _(FirstAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(value);
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
                if (_parent._throwOnEmpty)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(default(TSource));
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }

        class π : Sink<TSource>, IObserver<TSource>
        {
            private readonly FirstAsync<TSource> _parent;

            public π(FirstAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var b = false;

                try
                {
                    b = _parent._predicate(value);
                }
                catch (Exception ex)
                {
                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (b)
                {
                    base._observer.OnNext(value);
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
                if (_parent._throwOnEmpty)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
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