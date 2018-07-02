// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Take<TSource>
    {
        internal sealed class Count : Producer<TSource, Count._>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;

            public Count(IObservable<TSource> source, int count)
            {
                _source = source;
                _count = count;
            }

            public IObservable<TSource> Combine(int count)
            {
                //
                // Minimum semantics:
                //
                //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
                //   xs.Take(5)            --o--o--o--o--o|           xs.Take(3)            --o--o--o|
                //   xs.Take(5).Take(3)    --o--o--o|                 xs.Take(3).Take(5)    --o--o--o|
                //
                if (_count <= count)
                {
                    return this;
                }

                return new Count(_source, count);
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(_count, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private int _remaining;

                public _(int count, IObserver<TSource> observer)
                    : base(observer)
                {
                    _remaining = count;
                }

                public override void OnNext(TSource value)
                {
                    if (_remaining > 0)
                    {
                        --_remaining;
                        ForwardOnNext(value);

                        if (_remaining == 0)
                        {
                            ForwardOnCompleted();
                        }
                    }
                }
            }
        }

        internal sealed class Time : Producer<TSource, Time._>
        {
            private readonly IObservable<TSource> _source;
            private readonly TimeSpan _duration;
            internal readonly IScheduler _scheduler;

            public Time(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
            {
                _source = source;
                _duration = duration;
                _scheduler = scheduler;
            }

            public IObservable<TSource> Combine(TimeSpan duration)
            {
                //
                // Minimum semantics:
                //
                //   t                     0--1--2--3--4--5--6--7->   t                     0--1--2--3--4--5--6--7->
                //                                                    
                //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
                //   xs.Take(5s)           --o--o--o--o--o|           xs.Take(3s)           --o--o--o|
                //   xs.Take(5s).Take(3s)  --o--o--o|                 xs.Take(3s).Take(5s)  --o--o--o|
                //
                if (_duration <= duration)
                {
                    return this;
                }

                return new Time(_source, duration, _scheduler);
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                private object _gate;

                private IDisposable _task;

                public void Run(Time parent)
                {
                    _gate = new object();

                    Disposable.SetSingle(ref _task, parent._scheduler.ScheduleAction(this, parent._duration, state => state.Tick()));
                    Run(parent._source);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _task);
                    }
                    base.Dispose(disposing);
                }

                private void Tick()
                {
                    lock (_gate)
                    {
                        ForwardOnCompleted();
                    }
                }

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        ForwardOnNext(value);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }
}
