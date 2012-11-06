// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;

namespace System.Reactive.Linq.Observαble
{
    class Materialize<TSource> : Producer<Notification<TSource>>
    {
        private readonly IObservable<TSource> _source;

        public Materialize(IObservable<TSource> source)
        {
            _source = source;
        }

        public IObservable<TSource> Dematerialize()
        {
            return _source.AsObservable();
        }

        protected override IDisposable Run(IObserver<Notification<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<Notification<TSource>>, IObserver<TSource>
        {
            public _(IObserver<Notification<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(Notification.CreateOnNext<TSource>(value));
            }

            public void OnError(Exception error)
            {
                base._observer.OnNext(Notification.CreateOnError<TSource>(error));
                base._observer.OnCompleted();
                base.Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnNext(Notification.CreateOnCompleted<TSource>());
                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif