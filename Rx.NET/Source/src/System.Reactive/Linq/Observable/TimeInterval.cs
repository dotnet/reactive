// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class TimeInterval<TSource> : Producer<System.Reactive.TimeInterval<TSource>, TimeInterval<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public TimeInterval(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override _ CreateSink(IObserver<System.Reactive.TimeInterval<TSource>> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : Sink<System.Reactive.TimeInterval<TSource>>, IObserver<TSource>
        {
            public _(IObserver<System.Reactive.TimeInterval<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            private IStopwatch _watch;
            private TimeSpan _last;

            public IDisposable Run(TimeInterval<TSource> parent)
            {
                _watch = parent._scheduler.StartStopwatch();
                _last = TimeSpan.Zero;

                return parent._source.Subscribe(this);
            }

            public void OnNext(TSource value)
            {
                var now = _watch.Elapsed;
                var span = now.Subtract(_last);
                _last = now;
                base._observer.OnNext(new System.Reactive.TimeInterval<TSource>(value, span));
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                Dispose();
            }

            public void OnCompleted()
            {
                base._observer.OnCompleted();
                Dispose();
            }
        }
    }
}
