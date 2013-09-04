// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class TakeLast<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly TimeSpan _duration;
        private readonly IScheduler _scheduler;
        private readonly IScheduler _loopScheduler;

        public TakeLast(IObservable<TSource> source, int count, IScheduler loopScheduler)
        {
            _source = source;
            _count = count;
            _loopScheduler = loopScheduler;
        }

        public TakeLast(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler, IScheduler loopScheduler)
        {
            _source = source;
            _duration = duration;
            _scheduler = scheduler;
            _loopScheduler = loopScheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_scheduler == null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
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
            private readonly TakeLast<TSource> _parent;
            private Queue<TSource> _queue;

            public _(TakeLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _queue = new Queue<TSource>();
            }

            private SingleAssignmentDisposable _subscription;
            private SingleAssignmentDisposable _loop;

            public IDisposable Run()
            {
                _subscription = new SingleAssignmentDisposable();
                _loop = new SingleAssignmentDisposable();

                _subscription.Disposable = _parent._source.SubscribeSafe(this);
                
                return new CompositeDisposable(_subscription, _loop);
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
                _subscription.Dispose();

                var longRunning = _parent._loopScheduler.AsLongRunning();
                if (longRunning != null)
                    _loop.Disposable = longRunning.ScheduleLongRunning(Loop);
                else
                    _loop.Disposable = _parent._loopScheduler.Schedule(LoopRec);
            }

            private void LoopRec(Action recurse)
            {
                if (_queue.Count > 0)
                {
                    base._observer.OnNext(_queue.Dequeue());
                    recurse();
                }
                else
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            private void Loop(ICancelable cancel)
            {
                var n = _queue.Count;

                while (!cancel.IsDisposed)
                {
                    if (n == 0)
                    {
                        base._observer.OnCompleted();
                        break;
                    }
                    else
                        base._observer.OnNext(_queue.Dequeue());

                    n--;
                }

                base.Dispose();
            }
        }

        class τ : Sink<TSource>, IObserver<TSource>
        {
            private readonly TakeLast<TSource> _parent;
            private Queue<System.Reactive.TimeInterval<TSource>> _queue;

            public τ(TakeLast<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
            }

            private SingleAssignmentDisposable _subscription;
            private SingleAssignmentDisposable _loop;
            private IStopwatch _watch;

            public IDisposable Run()
            {
                _subscription = new SingleAssignmentDisposable();
                _loop = new SingleAssignmentDisposable();

                _watch = _parent._scheduler.StartStopwatch();
                _subscription.Disposable = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(_subscription, _loop);
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
                _subscription.Dispose();

                var now = _watch.Elapsed;
                Trim(now);

                var longRunning = _parent._loopScheduler.AsLongRunning();
                if (longRunning != null)
                    _loop.Disposable = longRunning.ScheduleLongRunning(Loop);
                else
                    _loop.Disposable = _parent._loopScheduler.Schedule(LoopRec);
            }

            private void LoopRec(Action recurse)
            {
                if (_queue.Count > 0)
                {
                    base._observer.OnNext(_queue.Dequeue().Value);
                    recurse();
                }
                else
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            private void Loop(ICancelable cancel)
            {
                var n = _queue.Count;

                while (!cancel.IsDisposed)
                {
                    if (n == 0)
                    {
                        base._observer.OnCompleted();
                        break;
                    }
                    else
                        base._observer.OnNext(_queue.Dequeue().Value);

                    n--;
                }

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