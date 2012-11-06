// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Case<TValue, TResult> : Producer<TResult>, IEvaluatableObservable<TResult>
    {
        private readonly Func<TValue> _selector;
        private readonly IDictionary<TValue, IObservable<TResult>> _sources;
        private readonly IObservable<TResult> _defaultSource;

        public Case(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IObservable<TResult> defaultSource)
        {
            _selector = selector;
            _sources = sources;
            _defaultSource = defaultSource;
        }

        public IObservable<TResult> Eval()
        {
            var res = default(IObservable<TResult>);
            if (_sources.TryGetValue(_selector(), out res))
                return res;

            return _defaultSource;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TResult>, IObserver<TResult>
        {
            private readonly Case<TValue, TResult> _parent;

            public _(Case<TValue, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var result = default(IObservable<TResult>);
                try
                {
                    result = _parent.Eval();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return Disposable.Empty;
                }

                return result.SubscribeSafe(this);
            }

            public void OnNext(TResult value)
            {
                base._observer.OnNext(value);
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
