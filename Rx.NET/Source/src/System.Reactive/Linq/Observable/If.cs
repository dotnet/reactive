// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


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

        protected override _ CreateSink(IObserver<TResult> observer) => new _(this, observer);

        protected override void Run(_ sink) => sink.Run();

        internal sealed class _ : IdentitySink<TResult>
        {
            private readonly If<TResult> _parent;

            public _(If<TResult> parent, IObserver<TResult> observer)
                : base(observer)
            {
                _parent = parent;
            }

            public void Run()
            {
                IObservable<TResult> result;
                try
                {
                    result = _parent.Eval();
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                SetUpstream(result.SubscribeSafe(this));
            }
        }
    }
}
