// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Take<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly TimeSpan _duration;
        internal readonly IScheduler _scheduler;

        public Take(IObservable<TSource> source, int count)
        {
            _source = source;
            _count = count;
        }

        public Take(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            _source = source;
            _duration = duration;
            _scheduler = scheduler;
        }

        public IObservable<TSource> Ω(int count)
        {
            //
            // Minimum semantics:
            //
            //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
            //   xs.Take(5)            --o--o--o--o--o|           xs.Take(3)            --o--o--o|
            //   xs.Take(5).Take(3)    --o--o--o|                 xs.Take(3).Take(5)    --o--o--o|
            //
            if (_count <= count)
                return this;
            else
                return new Take<TSource>(_source, count);
        }

        public IObservable<TSource> Ω(TimeSpan duration)
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
                return this;
            else
                return new Take<TSource>(_source, duration, _scheduler);
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
            private readonly Take<TSource> _parent;
            private int _remaining;

            public _(Take<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _remaining = _parent._count;
            }

            public void OnNext(TSource value)
            {
                if (_remaining > 0)
                {
                    --_remaining;
                    base._observer.OnNext(value);

                    if (_remaining == 0)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
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
            private readonly Take<TSource> _parent;

            public τ(Take<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;

            public IDisposable Run()
            {
                _gate = new object();

                var t = _parent._scheduler.Schedule(_parent._duration, Tick);
                var d = _parent._source.SubscribeSafe(this);
                return new CompositeDisposable(t, d);
            }

            private void Tick()
            {
                lock (_gate)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    base._observer.OnNext(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
#endif