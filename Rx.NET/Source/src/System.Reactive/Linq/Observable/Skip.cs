// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Skip<TSource>
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
                // Sum semantics:
                //
                //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
                //   xs.Skip(2)            --x--x--o--o--o--o--|      xs.Skip(3)            --x--x--x--o--o--o--|
                //   xs.Skip(2).Skip(3)    --------x--x--x--o--|      xs.Skip(3).Skip(2)    -----------x--x--o--|
                //
                return new Count(_source, _count + count);
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(_count, observer, cancel);

            protected override IDisposable Run(_ sink) => _source.SubscribeSafe(sink);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private int _remaining;

                public _(int count, IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _remaining = count;
                }

                public void OnNext(TSource value)
                {
                    if (_remaining <= 0)
                        base._observer.OnNext(value);
                    else
                        _remaining--;
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
                // Maximum semantics:
                //
                //   t                     0--1--2--3--4--5--6--7->   t                     0--1--2--3--4--5--6--7->
                //                                                    
                //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
                //   xs.Skip(2s)           xxxxxxx-o--o--o--o--|      xs.Skip(3s)           xxxxxxxxxx-o--o--o--|
                //   xs.Skip(2s).Skip(3s)  xxxxxxxxxx-o--o--o--|      xs.Skip(3s).Skip(2s)  xxxxxxx----o--o--o--|
                //
                if (duration <= _duration)
                    return this;
                else
                    return new Time(_source, duration, _scheduler);
            }

            protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TSource>, IObserver<TSource>
            {
                private volatile bool _open;

                public _(IObserver<TSource> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                }

                public IDisposable Run(Time parent)
                {
                    var t = parent._scheduler.Schedule(parent._duration, Tick);
                    var d = parent._source.SubscribeSafe(this);
                    return StableCompositeDisposable.Create(t, d);
                }

                private void Tick()
                {
                    _open = true;
                }

                public void OnNext(TSource value)
                {
                    if (_open)
                        base._observer.OnNext(value);
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
        }
    }
}
