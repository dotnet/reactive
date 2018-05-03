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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_finallyAction, observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(_source);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Action _finallyAction;

            public _(Action finallyAction, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _finallyAction = finallyAction;
            }

            public IDisposable Run(IObservable<TSource> source)
            {
                var subscription = source.SubscribeSafe(this);

                return Disposable.Create(() =>
                {
                    try
                    {
                        subscription.Dispose();
                    }
                    finally
                    {
                        _finallyAction();
                    }
                });
            }

            public void OnNext(TSource value)
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
