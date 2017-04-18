// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal abstract class DelaySubscription<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public DelaySubscription(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        internal sealed class Relative : DelaySubscription<TSource>
        {
            private readonly TimeSpan _dueTime;

            public Relative(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(observer, cancel);
                setSink(sink);
                return _scheduler.Schedule(sink, _dueTime, Subscribe);
            }
        }

        internal sealed class Absolute : DelaySubscription<TSource>
        {
            private readonly DateTimeOffset _dueTime;

            public Absolute(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
                : base(source, scheduler)
            {
                _dueTime = dueTime;
            }

            protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(observer, cancel);
                setSink(sink);
                return _scheduler.Schedule(sink, _dueTime, Subscribe);
            }
        }

        private IDisposable Subscribe(IScheduler _, _ sink)
        {
            return _source.SubscribeSafe(sink);
        }

        private sealed class _ : Sink<TSource>, IObserver<TSource>
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
