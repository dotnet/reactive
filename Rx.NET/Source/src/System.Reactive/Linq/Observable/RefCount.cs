// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class RefCount<TSource> : Producer<TSource, RefCount<TSource>._>
    {
        private readonly IConnectableObservable<TSource> _source;

        private readonly object _gate;
        private int _count;
        private IDisposable _connectableSubscription;

        public RefCount(IConnectableObservable<TSource> source)
        {
            _source = source;
            _gate = new object();
            _count = 0;
            _connectableSubscription = default(IDisposable);
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<TSource>, IObserver<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(RefCount<TSource> parent)
            {
                var subscription = parent._source.SubscribeSafe(this);

                lock (parent._gate)
                {
                    if (++parent._count == 1)
                    {
                        parent._connectableSubscription = parent._source.Connect();
                    }
                }

                return Disposable.Create(() =>
                {
                    subscription.Dispose();

                    lock (parent._gate)
                    {
                        if (--parent._count == 0)
                        {
                            parent._connectableSubscription.Dispose();
                        }
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
