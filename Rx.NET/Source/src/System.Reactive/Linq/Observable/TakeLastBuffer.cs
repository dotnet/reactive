// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class TakeLastBuffer<TSource>
    {
        internal sealed class Count : Producer<IList<TSource>>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;

            public Count(IObservable<TSource> source, int count)
            {
                _source = source;
                _count = count;
            }

            protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(_count, observer, cancel);
                setSink(sink);
                return _source.SubscribeSafe(sink);
            }

            private sealed class _ : Sink<IList<TSource>>, IObserver<TSource>
            {
                private readonly int _count;
                private Queue<TSource> _queue;

                public _(int count, IObserver<IList<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _count = count;
                    _queue = new Queue<TSource>();
                }

                public void OnNext(TSource value)
                {
                    _queue.Enqueue(value);
                    if (_queue.Count > _count)
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
        }

        internal sealed class Time : Producer<IList<TSource>>
        {
            private readonly IObservable<TSource> _source;
            private readonly TimeSpan _duration;
            private readonly IScheduler _scheduler;

            public Time(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
            {
                _source = source;
                _duration = duration;
                _scheduler = scheduler;
            }

            protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
            {
                var sink = new _(_duration, observer, cancel);
                setSink(sink);
                return sink.Run(this);
            }

            private sealed class _ : Sink<IList<TSource>>, IObserver<TSource>
            {
                private readonly TimeSpan _duration;
                private Queue<System.Reactive.TimeInterval<TSource>> _queue;

                public _(TimeSpan duration, IObserver<IList<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _duration = duration;
                    _queue = new Queue<System.Reactive.TimeInterval<TSource>>();
                }

                private IStopwatch _watch;

                public IDisposable Run(Time parent)
                {
                    _watch = parent._scheduler.StartStopwatch();

                    return parent._source.SubscribeSafe(this);
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
                    while (_queue.Count > 0 && now - _queue.Peek().Interval >= _duration)
                        _queue.Dequeue();
                }
            }
        }
    }
}
