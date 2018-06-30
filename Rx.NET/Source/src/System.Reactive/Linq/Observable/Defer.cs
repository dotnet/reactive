// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Defer<TValue> : BasicProducer<TValue>, IEvaluatableObservable<TValue>
    {
        private readonly Func<IObservable<TValue>> _observableFactory;

        public Defer(Func<IObservable<TValue>> observableFactory)
        {
            _observableFactory = observableFactory;
        }

        public IObservable<TValue> Eval() => _observableFactory();

        protected override IDisposable Run(IObserver<TValue> observer)
        {
            var result = default(IObservable<TValue>);
            try
            {
                result = _observableFactory();
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
