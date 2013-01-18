// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.Observαble
{
    class RefCount<TSource> : Producer<TSource>
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

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly RefCount<TSource> _parent;

            public _(RefCount<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var subscription = _parent._source.SubscribeSafe(this);

                lock (_parent._gate)
                {
                    if (++_parent._count == 1)
                    {
                        _parent._connectableSubscription = _parent._source.Connect();
                    }
                }

                return Disposable.Create(() =>
                {
                    subscription.Dispose();

                    lock (_parent._gate)
                    {
                        if (--_parent._count == 0)
                        {
                            _parent._connectableSubscription.Dispose();
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
#endif
