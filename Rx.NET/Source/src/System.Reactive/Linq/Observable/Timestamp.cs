// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class Timestamp<TSource> : Producer<Timestamped<TSource>, Timestamp<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public Timestamp(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<Timestamped<TSource>> observer, IDisposable cancel) => new _(_scheduler, observer, cancel);

        protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

        internal sealed class _ : Sink<Timestamped<TSource>>, IObserver<TSource>
        {
            private readonly IScheduler _scheduler;

            public _(IScheduler scheduler, IObserver<Timestamped<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _scheduler = scheduler;
            }

            public void OnNext(TSource value)
            {
                base._observer.OnNext(new Timestamped<TSource>(value, _scheduler.Now));
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
