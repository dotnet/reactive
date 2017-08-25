// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class TakeLast<TSource>
    {
        internal sealed class Count : Producer<TSource, Count._>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;
            private readonly IScheduler _loopScheduler;

            public Count(IObservable<TSource> source, int count, IScheduler loopScheduler)
            {
                _source = source;
                _count = count;
                _loopScheduler = loopScheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Count _parent;
                private Queue<TSource> _queue;

                public _(Count parent, IObserver<TSource> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_subscription, _loop);
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
        }

        internal sealed class Time : Producer<TSource, Time._>
        {
            private readonly IObservable<TSource> _source;
            private readonly TimeSpan _duration;
            private readonly IScheduler _scheduler;
            private readonly IScheduler _loopScheduler;

            public Time(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler, IScheduler loopScheduler)
            {
                _source = source;
                _duration = duration;
                _scheduler = scheduler;
                _loopScheduler = loopScheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Time _parent;
                private Queue<System.Reactive.TimeInterval<TSource>> _queue;

                public _(Time parent, IObserver<TSource> observer, IDisposable cancel)
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

                    return StableCompositeDisposable.Create(_subscription, _loop);
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
}
