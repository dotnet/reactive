// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Case<TValue, TResult> : Producer<TResult, Case<TValue, TResult>._>, IEvaluatableObservable<TResult>
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
            if (_sources.TryGetValue(_selector(), out var res))
                return res;

            return _defaultSource;
        }

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TResult>, IObserver<TResult>
        {
            public _(IObserver<TResult> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(Case<TValue, TResult> parent)
            {
                var result = default(IObservable<TResult>);
                try
                {
                    result = parent.Eval();
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
