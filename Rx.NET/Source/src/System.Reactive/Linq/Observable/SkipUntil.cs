// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

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
            IDisposable _mainDisposable;
            IDisposable _otherDisposable;
            volatile bool _forward;
            int _halfSerializer;
            Exception _error;

            public _(IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public IDisposable Run(SkipUntil<TSource, TOther> parent)
            {
                Disposable.TrySetSingle(ref _otherDisposable, parent._other.Subscribe(new OtherObserver(this)));

                Disposable.TrySetSingle(ref _mainDisposable, parent._source.Subscribe(this));

                return this;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    DisposeMain();
                    if (!Disposable.GetIsDisposed(ref _otherDisposable))
                    {
                        Disposable.TryDispose(ref _otherDisposable);
                    }
                }

                base.Dispose(disposing);
            }

            void DisposeMain()
            {
                if (!Disposable.GetIsDisposed(ref _mainDisposable))
                {
                    Disposable.TryDispose(ref _mainDisposable);
                }
            }

            public override void OnNext(TSource value)
            {
                if (_forward)
                {
                    if (Interlocked.CompareExchange(ref _halfSerializer, 1, 0) == 0)
                    {
                        ForwardOnNext(value);
                        if (Interlocked.Decrement(ref _halfSerializer) != 0)
                        {
                            var ex = _error;
                            _error = SkipUntilTerminalException.Instance;
                            ForwardOnError(ex);
                        }
                    }
                }
            }

            public override void OnError(Exception ex)
            {
                if (Interlocked.CompareExchange(ref _error, ex, null) == null)
                {
                    if (Interlocked.Increment(ref _halfSerializer) == 1)
                    {
                        _error = SkipUntilTerminalException.Instance;
                        ForwardOnError(ex);
                    }
                }
            }

            public override void OnCompleted()
            {
                if (_forward)
                {
                    if (Interlocked.CompareExchange(ref _error, SkipUntilTerminalException.Instance, null) == null)
                    {
                        if (Interlocked.Increment(ref _halfSerializer) == 1)
                        {
                            ForwardOnCompleted();
                        }
                    }
                }
                else
                {
                    DisposeMain();
                }
            }

            void OtherComplete()
            {
                _forward = true;
            }

            sealed class OtherObserver : IObserver<TOther>, IDisposable
            {
                readonly _ _parent;

                public OtherObserver(_ parent)
                {
                    _parent = parent;
                }

                public void Dispose()
                {
                    if (!Disposable.GetIsDisposed(ref _parent._otherDisposable))
                    {
                        Disposable.TryDispose(ref _parent._otherDisposable);
                    }
                }

                public void OnCompleted()
                {
                    Dispose();
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnNext(TOther value)
                {
                    _parent.OtherComplete();
                    Dispose();
                }
            }
        }
    }

    internal static class SkipUntilTerminalException
    {
        internal static readonly Exception Instance = new Exception("No further exceptions");
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
                var t = parent._scheduler.Schedule(this, parent._startTime, (_, state) => state.Tick());
                var d = parent._source.SubscribeSafe(this);
                return StableCompositeDisposable.Create(t, d);
            }

            private IDisposable Tick()
            {
                _open = true;
                return Disposable.Empty;
            }

            public override void OnNext(TSource value)
            {
                if (_open)
                    ForwardOnNext(value);
            }
        }
    }
}
