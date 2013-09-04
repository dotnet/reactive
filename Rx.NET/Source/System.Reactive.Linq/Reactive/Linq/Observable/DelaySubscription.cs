// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
    class DelaySubscription<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly DateTimeOffset? _dueTimeA;
        private readonly TimeSpan? _dueTimeR;
        private readonly IScheduler _scheduler;

        public DelaySubscription(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            _source = source;
            _dueTimeA = dueTime;
            _scheduler = scheduler;
        }

        public DelaySubscription(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            _source = source;
            _dueTimeR = dueTime;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(observer, cancel);
            setSink(sink);

            if (_dueTimeA.HasValue)
            {
                return _scheduler.Schedule(sink, _dueTimeA.Value, Subscribe);
            }
            else
            {
                return _scheduler.Schedule(sink, _dueTimeR.Value, Subscribe);
            }
        }

        private IDisposable Subscribe(IScheduler _, _ sink)
        {
            return _source.SubscribeSafe(sink);
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
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