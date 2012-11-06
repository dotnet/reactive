// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    abstract class Select<TResult> : Producer<TResult>
    {
        public abstract IObservable<TResult2> Ω<TResult2>(Func<TResult, TResult2> selector);
    }

    class Select<TSource, TResult> : Select<TResult>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<TSource, TResult> _selector;
        private readonly Func<TSource, int, TResult> _selectorI;

        public Select(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            _source = source;
            _selector = selector;
        }

        public Select(IObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
            _source = source;
            _selectorI = selector;
        }

        public override IObservable<TResult2> Ω<TResult2>(Func<TResult, TResult2> selector)
        {
            if (_selector != null)
                return new Select<TSource, TResult2>(_source, x => selector(_selector(x)));
            else
                return new Select<TResult, TResult2>(this, selector);
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_selector != null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new τ(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
        }

        class _ : Sink<TResult>, IObserver<TSource>
        {
            private readonly Select<TSource, TResult> _parent;

            public _(Select<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                var result = default(TResult);
                try
                {
                    result = _parent._selector(value);
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(result);
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

        class τ : Sink<TResult>, IObserver<TSource>
        {
            private readonly Select<TSource, TResult> _parent;
            private int _index;

            public τ(Select<TSource, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _index = 0;
            }

            public void OnNext(TSource value)
            {
                var result = default(TResult);
                try
                {
                    result = _parent._selectorI(value, checked(_index++));
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(result);
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