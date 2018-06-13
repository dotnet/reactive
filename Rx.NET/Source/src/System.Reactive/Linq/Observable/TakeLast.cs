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

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : IdentitySink<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Count _parent;
                private Queue<TSource> _queue;

                public _(Count parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _parent = parent;
                    _queue = new Queue<TSource>();
                }

                private IDisposable _sourceDisposable;
                private IDisposable _loopDisposable;

                public void Run()
                {
                    Disposable.SetSingle(ref _sourceDisposable, _parent._source.SubscribeSafe(this));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _loopDisposable);
                        Disposable.TryDispose(ref _sourceDisposable);
                    }
                    base.Dispose(disposing);
                }

                public override void OnNext(TSource value)
                {
                    _queue.Enqueue(value);
                    if (_queue.Count > _parent._count)
                        _queue.Dequeue();
                }

                public override void OnCompleted()
                {
                    Disposable.TryDispose(ref _sourceDisposable);

                    var longRunning = _parent._loopScheduler.AsLongRunning();
                    if (longRunning != null)
                        Disposable.SetSingle(ref _loopDisposable, longRunning.ScheduleLongRunning(this, (@this, c) => @this.Loop(c)));
                    else
                        Disposable.SetSingle(ref _loopDisposable, _parent._loopScheduler.Schedule(this, (@this, a) => @this.LoopRec(a)));
                }

                private void LoopRec(Action<_> recurse)
                {
                    if (_queue.Count > 0)
                    {
                        ForwardOnNext(_queue.Dequeue());
                        recurse(this);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }

                private void Loop(ICancelable cancel)
                {
                    var n = _queue.Count;

                    while (!cancel.IsDisposed)
                    {
                        if (n == 0)
                        {
                            ForwardOnCompleted();
                            break;
                        }
                        else
                            ForwardOnNext(_queue.Dequeue());

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

            protected override _ CreateSink(IObserver<TSource> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : IdentitySink<TSource>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Time _parent;
                private Queue<System.Reactive.TimeInterval<TSource>> _queue;

                public _(Time parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _parent = parent;
                    _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                }

                private IDisposable _sourceDisposable;
                private IDisposable _loopDisposable;
                private IStopwatch _watch;

                public void Run()
                {
                    _watch = _parent._scheduler.StartStopwatch();
                    Disposable.SetSingle(ref _sourceDisposable, _parent._source.SubscribeSafe(this));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _sourceDisposable);
                    }
                    base.Dispose(disposing);
                }

                public override void OnNext(TSource value)
                {
                    var now = _watch.Elapsed;
                    _queue.Enqueue(new System.Reactive.TimeInterval<TSource>(value, now));
                    Trim(now);
                }

                public override void OnCompleted()
                {
                    Disposable.TryDispose(ref _sourceDisposable);

                    var now = _watch.Elapsed;
                    Trim(now);

                    var longRunning = _parent._loopScheduler.AsLongRunning();
                    if (longRunning != null)
                        Disposable.SetSingle(ref _loopDisposable, longRunning.ScheduleLongRunning(this, (@this, c) => @this.Loop(c)));
                    else
                        Disposable.SetSingle(ref _loopDisposable, _parent._loopScheduler.Schedule(this, (@this, a) => @this.LoopRec(a)));
                }

                private void LoopRec(Action<_> recurse)
                {
                    if (_queue.Count > 0)
                    {
                        ForwardOnNext(_queue.Dequeue().Value);
                        recurse(this);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                }

                private void Loop(ICancelable cancel)
                {
                    var n = _queue.Count;

                    while (!cancel.IsDisposed)
                    {
                        if (n == 0)
                        {
                            ForwardOnCompleted();
                            break;
                        }
                        else
                            ForwardOnNext(_queue.Dequeue().Value);

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
