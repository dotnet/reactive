// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class Skip<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly TimeSpan _duration;
        internal readonly IScheduler _scheduler;

        public Skip(IObservable<TSource> source, int count)
        {
            _source = source;
            _count = count;
        }

        public Skip(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            _source = source;
            _duration = duration;
            _scheduler = scheduler;
        }

        public IObservable<TSource> Ω(int count)
        {
            //
            // Sum semantics:
            //
            //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
            //   xs.Skip(2)            --x--x--o--o--o--o--|      xs.Skip(3)            --x--x--x--o--o--o--|
            //   xs.Skip(2).Skip(3)    --------x--x--x--o--|      xs.Skip(3).Skip(2)    -----------x--x--o--|
            //
            return new Skip<TSource>(_source, _count + count);
        }

        public IObservable<TSource> Ω(TimeSpan duration)
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
                return new Skip<TSource>(_source, duration, _scheduler);
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
            private readonly Skip<TSource> _parent;
            private int _remaining;

            public _(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _remaining = _parent._count;
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

        class τ : Sink<TSource>, IObserver<TSource>
        {
            private readonly Skip<TSource> _parent;
            private volatile bool _open;

            public τ(Skip<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var t = _parent._scheduler.Schedule(_parent._duration, Tick);
                var d = _parent._source.SubscribeSafe(this);
                return new CompositeDisposable(t, d);
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
#endif