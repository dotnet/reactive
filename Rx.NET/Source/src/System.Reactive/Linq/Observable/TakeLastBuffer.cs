// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class TakeLastBuffer<TSource>
    {
        internal sealed class Count : Producer<IList<TSource>, Count._>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;

            public Count(IObservable<TSource> source, int count)
            {
                _source = source;
                _count = count;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(_count, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource, IList<TSource>>
            {
                private readonly int _count;
                private readonly Queue<TSource> _queue;

                public _(int count, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _count = count;
                    _queue = new Queue<TSource>();
                }

                public override void OnNext(TSource value)
                {
                    _queue.Enqueue(value);
                    if (_queue.Count > _count)
                    {
                        _queue.Dequeue();
                    }
                }

                public override void OnCompleted()
                {
                    var res = new List<TSource>(_queue.Count);
                    while (_queue.Count > 0)
                    {
                        res.Add(_queue.Dequeue());
                    }

                    ForwardOnNext(res);
                    ForwardOnCompleted();
                }
            }
        }

        internal sealed class Time : Producer<IList<TSource>, Time._>
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

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(_duration, observer);

            protected override void Run(_ sink) => sink.Run(_source, _scheduler);

            internal sealed class _ : Sink<TSource, IList<TSource>>
            {
                private readonly TimeSpan _duration;
                private readonly Queue<Reactive.TimeInterval<TSource>> _queue;

                public _(TimeSpan duration, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _duration = duration;
                    _queue = new Queue<Reactive.TimeInterval<TSource>>();
                }

                private IStopwatch _watch;

                public void Run(IObservable<TSource> source, IScheduler scheduler)
                {
                    _watch = scheduler.StartStopwatch();

                    Run(source);
                }

                public override void OnNext(TSource value)
                {
                    var now = _watch.Elapsed;
                    _queue.Enqueue(new Reactive.TimeInterval<TSource>(value, now));
                    Trim(now);
                }

                public override void OnCompleted()
                {
                    var now = _watch.Elapsed;
                    Trim(now);

                    var res = new List<TSource>(_queue.Count);
                    while (_queue.Count > 0)
                    {
                        res.Add(_queue.Dequeue().Value);
                    }

                    ForwardOnNext(res);
                    ForwardOnCompleted();
                }

                private void Trim(TimeSpan now)
                {
                    while (_queue.Count > 0 && now - _queue.Peek().Interval >= _duration)
                    {
                        _queue.Dequeue();
                    }
                }
            }
        }
    }
}
