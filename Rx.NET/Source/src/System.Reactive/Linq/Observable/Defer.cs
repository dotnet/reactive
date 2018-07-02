// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 


namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Defer<TValue> : Producer<TValue, Defer<TValue>._>, IEvaluatableObservable<TValue>
    {
        private readonly Func<IObservable<TValue>> _observableFactory;

        public Defer(Func<IObservable<TValue>> observableFactory)
        {
            _observableFactory = observableFactory;
        }

        protected override _ CreateSink(IObserver<TValue> observer) => new _(_observableFactory, observer);

        protected override void Run(_ sink) => sink.Run();

        public IObservable<TValue> Eval() => _observableFactory();

        internal sealed class _ : IdentitySink<TValue>
        {
            private readonly Func<IObservable<TValue>> _observableFactory;

            public _(Func<IObservable<TValue>> observableFactory, IObserver<TValue> observer)
                : base(observer)
            {
                _observableFactory = observableFactory;
            }

            public void Run()
            {
                var result = default(IObservable<TValue>);
                try
                {
                    result = _observableFactory();
                }
                catch (Exception exception)
                {
                    ForwardOnError(exception);

                    return;
                }

                Run(result);
            }
        }
    }
}
