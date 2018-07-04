// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class If<TResult> : BasicProducer<TResult>, IEvaluatableObservable<TResult>
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

        public IObservable<TResult> Eval() => _condition() ? _thenSource : _elseSource;

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
