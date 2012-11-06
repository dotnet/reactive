// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class LastAsync<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, bool> _predicate;
        private readonly bool _throwOnEmpty;

        public LastAsync(IObservable<TSource> source, Func<TSource, bool> predicate, bool throwOnEmpty)
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
            private readonly LastAsync<TSource> _parent;
            private TSource _value;
            private bool _seenValue;

            public _(LastAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;

                _value = default(TSource);
                _seenValue = false;
            }

            public void OnNext(TSource value)
            {
                _value = value;
                _seenValue = true;
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_seenValue && _parent._throwOnEmpty)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_value);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }

        class π : Sink<TSource>, IObserver<TSource>
        {
            private readonly LastAsync<TSource> _parent;
            private TSource _value;
            private bool _seenValue;

            public π(LastAsync<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;

                _value = default(TSource);
                _seenValue = false;
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
                    _value = value;
                    _seenValue = true;
                }
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                if (!_seenValue && _parent._throwOnEmpty)
                {
                    base._observer.OnError(new InvalidOperationException(Strings_Linq.NO_MATCHING_ELEMENTS));
                }
                else
                {
                    base._observer.OnNext(_value);
                    base._observer.OnCompleted();
                }

                base.Dispose();
            }
        }
    }
}
#endif