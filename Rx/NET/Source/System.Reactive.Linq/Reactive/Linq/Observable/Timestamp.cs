// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
    class Timestamp<TSource> : Producer<Timestamped<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public Timestamp(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<Timestamped<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<Timestamped<TSource>>, IObserver<TSource>
        {
            private readonly Timestamp<TSource> _parent;

            public _(Timestamp<TSource> parent, IObserver<Timestamped<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(new Timestamped<TSource>(value, _parent._scheduler.Now));
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