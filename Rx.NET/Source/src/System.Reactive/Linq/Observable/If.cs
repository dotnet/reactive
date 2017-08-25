// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class If<TResult> : Producer<TResult, If<TResult>._>, IEvaluatableObservable<TResult>
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

        protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run();

        internal sealed class _ : Sink<TResult>, IObserver<TResult>
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
