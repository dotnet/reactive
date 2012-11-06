// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class TakeLastBuffer<TSource> : Producer<IList<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly TimeSpan _duration;
        private readonly IScheduler _scheduler;

        public TakeLastBuffer(IObservable<TSource> source, int count)
        {
            _source = source;
            _count = count;
        }

        public TakeLastBuffer(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            _source = source;
            _duration = duration;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
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

        class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly TakeLastBuffer<TSource> _parent;
            private Queue<TSource> _queue;

            public _(TakeLastBuffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _queue = new Queue<TSource>();
            }

            public void OnNext(TSource value)
            {
                _queue.Enqueue(value);
                if (_queue.Count > _parent._count)
                    _queue.Dequeue();
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                var res = new List<TSource>(_queue.Count);
                while (_queue.Count > 0)
                    res.Add(_queue.Dequeue());

                base._observer.OnNext(res);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class τ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly TakeLastBuffer<TSource> _parent;
            private Queue<System.Reactive.TimeInterval<TSource>> _queue;

            public τ(TakeLastBuffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
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
                Trim(now);
            }

            public void OnError(Exception error)
            {
                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                var now = _watch.Elapsed;
                Trim(now);

                var res = new List<TSource>(_queue.Count);
                while (_queue.Count > 0)
                    res.Add(_queue.Dequeue().Value);

                base._observer.OnNext(res);
                base._observer.OnCompleted();
                base.Dispose();
            }

            private void Trim(TimeSpan now)
            {
                while (_queue.Count > 0 && now - _queue.Peek().Interval >= _parent._duration)
                    _queue.Dequeue();
            }
        }
    }
}
#endif