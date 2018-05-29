// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal sealed class TakeUntil<TSource, TOther> : Producer<TSource, TakeUntil<TSource, TOther>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TOther> _other;

        public TakeUntil(IObservable<TSource> source, IObservable<TOther> other)
        {
            _source = source;
            _other = other;
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            readonly OtherObserver other;

            IDisposable mainDisposable;

            int halfSerializer;

            Exception error;

            static readonly Exception TerminalException = new Exception("No further exceptions");

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                other = new OtherObserver(this);
            }

            public IDisposable Run(TakeUntil<TSource, TOther> parent)
            {
                other.OnSubscribe(parent._other.Subscribe(other));

                Disposable.TrySetSingle(ref mainDisposable, parent._source.Subscribe(this));

                return this;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (!Disposable.GetIsDisposed(ref mainDisposable))
                {
                    Disposable.TryDispose(ref mainDisposable);
                }
                other.Dispose();
            }

            public override void OnNext(TSource value)
            {
                if (Interlocked.CompareExchange(ref halfSerializer, 1, 0) == 0)
                {
                    ForwardOnNext(value);
                    if (Interlocked.Decrement(ref halfSerializer) != 0)
                    {
                        var ex = error;
                        if (ex != TerminalException)
                        {
                            error = TerminalException;
                            ForwardOnError(ex);
                        }
                        else
                        {
                            ForwardOnCompleted();
                        }
                    }
                }
            }

            public override void OnError(Exception ex)
            {
                if (Interlocked.CompareExchange(ref error, ex, null) == null)
                {
                    if (Interlocked.Increment(ref halfSerializer) == 1)
                    {
                        error = TerminalException;
                        ForwardOnError(ex);
                    }
                }
            }

            public override void OnCompleted()
            {
                if (Interlocked.CompareExchange(ref error, TerminalException, null) == null)
                {
                    if (Interlocked.Increment(ref halfSerializer) == 1)
                    {
                        ForwardOnCompleted();
                    }
                }
            }

            sealed class OtherObserver : IObserver<TOther>, IDisposable
            {
                readonly _ parent;

                IDisposable upstream;

                public OtherObserver(_ parent)
                {
                    this.parent = parent;
                }

                public void Dispose()
                {
                    if (!Disposable.GetIsDisposed(ref upstream))
                    {
                        Disposable.TryDispose(ref upstream);
                    }
                }

                public void OnSubscribe(IDisposable d)
                {
                    Disposable.TrySetSingle(ref upstream, d);
                }

                public void OnCompleted()
                {
                    // Completion doesn't mean termination in Rx.NET for this operator
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    parent.OnError(error);
                }

                public void OnNext(TOther value)
                {
                    parent.OnCompleted();
                }
            }

        }
    }

    internal sealed class TakeUntil<TSource> : Producer<TSource, TakeUntil<TSource>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly DateTimeOffset _endTime;
        internal readonly IScheduler _scheduler;

        public TakeUntil(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
        {
            _source = source;
            _endTime = endTime;
            _scheduler = scheduler;
        }

        public IObservable<TSource> Combine(DateTimeOffset endTime)
        {
            //
            // Minimum semantics:
            //
            //   t                     0--1--2--3--4--5--6--7->   t                     0--1--2--3--4--5--6--7->
            //
            //   xs                    --o--o--o--o--o--o--|      xs                    --o--o--o--o--o--o--|
            //   xs.TU(5AM)            --o--o--o--o--o|           xs.TU(3AM)            --o--o--o|
            //   xs.TU(5AM).TU(3AM)    --o--o--o|                 xs.TU(3AM).TU(5AM)    --o--o--o|
            //
            if (_endTime <= endTime)
                return this;
            else
                return new TakeUntil<TSource>(_source, endTime, _scheduler);
        }

        protected override _ CreateSink(IObserver<TSource> observer, IDisposable cancel) => new _(observer, cancel);

        protected override IDisposable Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly object _gate = new object();

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(TakeUntil<TSource> parent)
            {
                var t = parent._scheduler.Schedule(parent._endTime, Tick);
                var d = parent._source.SubscribeSafe(this);
                return StableCompositeDisposable.Create(t, d);
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
