// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class TimeInterval<TSource> : Producer<System.Reactive.TimeInterval<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly IScheduler _scheduler;

        public TimeInterval(IObservable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<System.Reactive.TimeInterval<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<System.Reactive.TimeInterval<TSource>>, IObserver<TSource>
        {
            private readonly TimeInterval<TSource> _parent;

            public _(TimeInterval<TSource> parent, IObserver<System.Reactive.TimeInterval<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IStopwatch _watch;
            private TimeSpan _last;

            public IDisposable Run()
            {
                _watch = _parent._scheduler.StartStopwatch();
                _last = TimeSpan.Zero;

                return _parent._source.Subscribe(this);
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
#endif