// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Finally<TSource> : Producer<TSource, Finally<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly Action _finallyAction;

        public Finally(IObservable<TSource> source, Action finallyAction)
        {
            _source = source;
            _finallyAction = finallyAction;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new _(_finallyAction, observer);

        protected override void Run(_ sink) => sink.Run(_source);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Action _finallyAction;

            public _(Action finallyAction, IObserver<TSource> observer)
                : base(observer)
            {
                _finallyAction = finallyAction;
            }

            public override void Run(IObservable<TSource> source)
            {
                var subscription = source.SubscribeSafe(this);

                SetUpstream(Disposable.Create(() =>
                {
                    try
                    {
                        subscription.Dispose();
                    }
                    finally
                    {
                        _finallyAction();
                    }
                }));
            }
        }
    }
}
