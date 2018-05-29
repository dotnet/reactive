// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class SkipUntil<TSource, TOther> : Producer<TSource, SkipUntil<TSource, TOther>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TOther> _other;

        public SkipUntil(IObservable<TSource> source, IObservable<TOther> other)
        {
            _source = source;
            _other = other;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(SkipUntil<TSource, TOther> parent)
            {
                var sourceObserver = new SourceObserver(this);
                var otherObserver = new OtherObserver(this, sourceObserver);

                var otherSubscription = parent._other.SubscribeSafe(otherObserver);
                var sourceSubscription = parent._source.SubscribeSafe(sourceObserver);
                
                sourceObserver.Disposable = sourceSubscription;
                otherObserver.Disposable = otherSubscription;

                return StableCompositeDisposable.Create(
                    sourceSubscription,
                    otherSubscription
                );
            }

            private sealed class SourceObserver : IObserver<TSource>
            {
                private readonly _ _parent;
                public volatile bool _forward;
                private readonly SingleAssignmentDisposable _subscription;

                public SourceObserver(_ parent)
                {
                    _parent = parent;
                    _subscription = new SingleAssignmentDisposable();
                }

                public IDisposable Disposable
                {
                    set { _subscription.Disposable = value; }
                }

                public void OnNext(TSource value)
                {
                    if (_forward)
                        _parent.ForwardOnNext(value);
                }

                public void OnError(Exception error)
                {
                    _parent.ForwardOnError(error);
                }

                public void OnCompleted()
                {
                    if (_forward)
                        _parent.ForwardOnCompleted();

                    _subscription.Dispose(); // We can't cancel the other stream yet, it may be on its way to dispatch an OnError message and we don't want to have a race.
                }
            }

            private sealed class OtherObserver : IObserver<TOther>
            {
                private readonly _ _parent;
                private readonly SourceObserver _sourceObserver;
                private readonly SingleAssignmentDisposable _subscription;

                public OtherObserver(_ parent, SourceObserver sourceObserver)
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
                    _sourceObserver._forward = true;
                    _subscription.Dispose();
                }

                public void OnError(Exception error)
                {
                    _parent.ForwardOnError(error);
                }

                public void OnCompleted()
                {
                    _subscription.Dispose();
                }
            }
        }
    }

    internal sealed class SkipUntil<TSource> : Producer<TSource, SkipUntil<TSource>._>
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

        public IObservable<TSource> Combine(DateTimeOffset startTime)
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

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            private volatile bool _open;

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(SkipUntil<TSource> parent)
            {
                var t = parent._scheduler.Schedule(parent._startTime, Tick);
                var d = parent._source.SubscribeSafe(this);
                return StableCompositeDisposable.Create(t, d);
            }

            private void Tick()
            {
                _open = true;
            }

            public override void OnNext(TSource value)
            {
                if (_open)
                    ForwardOnNext(value);
            }
        }
    }
}
