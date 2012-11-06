// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.Observαble
{
    class SkipLast<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly TimeSpan _duration;
        private readonly IScheduler _scheduler;

        public SkipLast(IObservable<TSource> source, int count)
        {
            _source = source;
            _count = count;
        }

        public SkipLast(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            _source = source;
            _duration = duration;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_scheduler == null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }
            else
            {
                var sink = new τ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly SkipLast<TSource> _parent;
            private Queue<TSource> _queue;

            public _(SkipLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _queue = new Queue<TSource>();
            }

            public void OnNext(TSource value)
            {
                _queue.Enqueue(value);
                if (_queue.Count > _parent._count)
                    base._observer.OnNext(_queue.Dequeue());
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

        class τ : Sink<TSource>, IObserver<TSource>
        {
            private readonly SkipLast<TSource> _parent;
            private Queue<System.Reactive.TimeInterval<TSource>> _queue;

            public τ(SkipLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
            }

            private IStopwatch _watch;

            public IDisposable Run()
            {
                _watch = _parent._scheduler.StartStopwatch();

                return _parent._source.SubscribeSafe(this);
            }

            public void OnNext(TSource value)
            {
                var now = _watch.Elapsed;
                _queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, now));
                while (_queue.Count > 0 && now - _queue.Peek().Interval >= _parent._duration)
                    base._observer.OnNext(_queue.Dequeue().Value);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                var now = _watch.Elapsed;
                while (_queue.Count > 0 && now - _queue.Peek().Interval >= _parent._duration)
                    base._observer.OnNext(_queue.Dequeue().Value);

                base._observer.OnCompleted();
                base.Dispose();
            }
        }
    }
}
#endif