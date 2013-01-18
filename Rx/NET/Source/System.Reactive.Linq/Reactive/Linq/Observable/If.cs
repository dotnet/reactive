// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class If<TResult> : Producer<TResult>, IEvaluatableObservable<TResult>
    {
        private readonly Func<bool> _condition;
        private readonly IObservable<TResult> _thenSource;
        private readonly IObservable<TResult> _elseSource;

        public If(Func<bool> condition, IObservable<TResult> thenSource, IObservable<TResult> elseSource)
        {
            _condition = condition;
            _thenSource = thenSource;
            _elseSource = elseSource;
        }

        public IObservable<TResult> Eval()
        {
            return _condition() ? _thenSource : _elseSource;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TResult>, IObserver<TResult>
        {
            private readonly If<TResult> _parent;

            public _(If<TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
