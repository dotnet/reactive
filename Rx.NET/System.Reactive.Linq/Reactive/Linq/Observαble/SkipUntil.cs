// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class SkipUntil<TSource, TOther> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TOther> _other;

        public SkipUntil(IObservable<TSource> source, IObservable<TOther> other)
        {
            _source = source;
            _other = other;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>
        {
            private readonly SkipUntil<TSource, TOther> _parent;

            public _(SkipUntil<TSource, TOther> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var sourceObserver = new T(this);
                var otherObserver = new O(this, sourceObserver);

                var sourceSubscription = _parent._source.SubscribeSafe(sourceObserver);
                var otherSubscription = _parent._other.SubscribeSafe(otherObserver);

                sourceObserver.Disposable = sourceSubscription;
                otherObserver.Disposable = otherSubscription;

                return new CompositeDisposable(
                    sourceSubscription,
                    otherSubscription
                );
            }

            class T : IObserver<TSource>
            {
                private readonly _ _parent;
                public volatile IObserver<TSource> _observer;
                private readonly SingleAssignmentDisposable _subscription;

                public T(_ parent)
                {
                    _parent = parent;
                    _observer = NopObserver<TSource>.Instance;
                    _subscription = new SingleAssignmentDisposable();
                }

                public IDisposable Disposable
                {
                    set { _subscription.Disposable = value; }
                }

                public void OnNext(TSource value)
                {
                    _observer.OnNext(value);
                }

                public void OnError(Exception error)
                {
                    _parent._observer.OnError(error);
                    _parent.Dispose();
                }

                public void OnCompleted()
                {
                    _observer.OnCompleted();
                    _subscription.Dispose(); // We can't cancel the other stream yet, it may be on its way to dispatch an OnError message and we don't want to have a race.
                }
            }

            class O : IObserver<TOther>
            {
                private readonly _ _parent;
                private readonly T _sourceObserver;
                private readonly SingleAssignmentDisposable _subscription;

                public O(_ parent, T sourceObserver)
                {
                    _parent = parent;
                    _sourceObserver = sourceObserver;
                    _subscription = new SingleAssignmentDisposable();
                }

                public IDisposable Disposable
                {
                    set { _subscription.Disposable = value; }
                }

                public void OnNext(TOther value)
                {
                    _sourceObserver._observer = _parent._observer;
                    _subscription.Dispose();
                }

                public void OnError(Exception error)
                {
                    _parent._observer.OnError(error);
                    _parent.Dispose();
                }

                public void OnCompleted()
                {
                    _subscription.Dispose();
                }
            }
        }
    }

    class SkipUntil<TSource> : Producer<TSource>
    {
        private readonly IObservable<TSource> _source;
        private readonly DateTimeOffset _startTime;
        internal readonly IScheduler _scheduler;

        public SkipUntil(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
        {
            _source = source;
            _startTime = startTime;
            _scheduler = scheduler;
        }

        public IObservable<TSource> Ω(DateTimeOffset startTime)
        {
            //
            // Maximum semantics:
            //
            //   t                     0--1--2--3--4--5--6--7->   t                     0--1--2--3--4--5--6--7->
            //
            //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
            //   xs.SU(5AM)            xxxxxxxxxxxxxxxx-o--|      xs.SU(3AM)            xxxxxxxxxx-o--o--o--|
            //   xs.SU(5AM).SU(3AM)    xxxxxxxxx--------o--|      xs.SU(3AM).SU(5AM)    xxxxxxxxxxxxxxxx-o--|
            //
            if (startTime <= _startTime)
                return this;
            else
                return new SkipUntil<TSource>(_source, startTime, _scheduler);
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>, IObserver<TSource>
        {
            private readonly SkipUntil<TSource> _parent;
            private volatile bool _open;

            public _(SkipUntil<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var t = _parent._scheduler.Schedule(_parent._startTime, Tick);
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