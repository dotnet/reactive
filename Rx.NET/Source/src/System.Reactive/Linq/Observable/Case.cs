// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Case<TValue, TResult> : BasicProducer<TResult>, IEvaluatableObservable<TResult>
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

        protected override IDisposable Run(IObserver<TResult> observer)
        {
            var result = default(IObservable<TResult>);
            try
            {
                result = Eval();
            }
            catch (Exception exception)
            {
                observer.OnError(exception);

                return Disposable.Empty;
            }

            return result.SubscribeSafe(observer);
        }
    }
}
