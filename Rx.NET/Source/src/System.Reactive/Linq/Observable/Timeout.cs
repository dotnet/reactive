// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Timeout<TSource>
    {
        internal sealed class Relative : Producer<TSource, Relative._>
        {
            private readonly IObservable<TSource> _source;
            private readonly TimeSpan _dueTime;
            private readonly IObservable<TSource> _other;
            private readonly IScheduler _scheduler;

            public Relative(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
            {
                _source = source;
                _dueTime = dueTime;
                _other = other;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly TimeSpan _dueTime;
                private readonly IObservable<TSource> _other;
                private readonly IScheduler _scheduler;

                private long _index;
                private SingleAssignmentDisposableValue _mainDisposable;
                private SingleAssignmentDisposableValue _otherDisposable;
                private IDisposable? _timerDisposable;

                public _(Relative parent, IObserver<TSource> observer)
                    : base(observer)
                {
                    _dueTime = parent._dueTime;
                    _other = parent._other;
                    _scheduler = parent._scheduler;
                }

                public override void Run(IObservable<TSource> source)
                {
                    CreateTimer(0L);

                    _mainDisposable.Disposable = source.SubscribeSafe(this);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        _mainDisposable.Dispose();
                        _otherDisposable.Dispose();
                        Disposable.Dispose(ref _timerDisposable);
                    }

                    base.Dispose(disposing);
                }

                private void CreateTimer(long idx)
                {
                    if (Disposable.TrySetMultiple(ref _timerDisposable, null))
                    {
                        var d = _scheduler.ScheduleAction((idx, instance: this), _dueTime, static state => { state.instance.Timeout(state.idx); });

                        Disposable.TrySetMultiple(ref _timerDisposable, d);
                    }
                }

                private void Timeout(long idx)
                {
                    if (Volatile.Read(ref _index) == idx && Interlocked.CompareExchange(ref _index, long.MaxValue, idx) == idx)
                    {
                        _mainDisposable.Dispose();

                        var d = _other.Subscribe(GetForwarder());

                        _otherDisposable.Disposable = d;
                    }
                }

                public override void OnNext(TSource value)
                {
                    var idx = Volatile.Read(ref _index);
                    if (idx != long.MaxValue && Interlocked.CompareExchange(ref _index, idx + 1, idx) == idx)
                    {
                        // Do not swap in the BooleanDisposable.True here
                        // As we'll need _timerDisposable to store the next timer
                        // BD.True would cancel it immediately and break the operation
                        Volatile.Read(ref _timerDisposable)?.Dispose();

                        ForwardOnNext(value);

                        CreateTimer(idx + 1);
                    }
                }

                public override void OnError(Exception error)
                {
                    if (Interlocked.Exchange(ref _index, long.MaxValue) != long.MaxValue)
                    {
                        Disposable.Dispose(ref _timerDisposable);

                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    if (Interlocked.Exchange(ref _index, long.MaxValue) != long.MaxValue)
                    {
                        Disposable.Dispose(ref _timerDisposable);

                        ForwardOnCompleted();
                    }
                }
            }
        }

        internal sealed class Absolute : Producer<TSource, Absolute._>
        {
            private readonly IObservable<TSource> _source;
            private readonly DateTimeOffset _dueTime;
            private readonly IObservable<TSource> _other;
            private readonly IScheduler _scheduler;

            public Absolute(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
            {
                _source = source;
                _dueTime = dueTime;
                _other = other;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new(_other, observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly IObservable<TSource> _other;

                private SerialDisposableValue _serialDisposable;
                private int _wip;

                public _(IObservable<TSource> other, IObserver<TSource> observer)
                    : base(observer)
                {
                    _other = other;
                }

                public void Run(Absolute parent)
                {
                    SetUpstream(parent._scheduler.ScheduleAction(this, parent._dueTime, static @this => @this.Timeout()));

                    _serialDisposable.Disposable = parent._source.SubscribeSafe(this);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        _serialDisposable.Dispose();
                    }

                    base.Dispose(disposing);
                }

                private void Timeout()
                {
                    if (Interlocked.Increment(ref _wip) == 1)
                    {
                        _serialDisposable.Disposable = _other.SubscribeSafe(GetForwarder());
                    }
                }

                public override void OnNext(TSource value)
                {
                    if (Interlocked.CompareExchange(ref _wip, 1, 0) == 0)
                    {
                        ForwardOnNext(value);
                        if (Interlocked.Decrement(ref _wip) != 0)
                        {
                            _serialDisposable.Disposable = _other.SubscribeSafe(GetForwarder());
                        }
                    }
                }

                public override void OnError(Exception error)
                {
                    if (Interlocked.CompareExchange(ref _wip, 1, 0) == 0)
                    {
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    if (Interlocked.CompareExchange(ref _wip, 1, 0) == 0)
                    {
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }

    internal sealed class Timeout<TSource, TTimeout> : Producer<TSource, Timeout<TSource, TTimeout>._>
    {
        private readonly IObservable<TSource> _source;
        private readonly IObservable<TTimeout> _firstTimeout;
        private readonly Func<TSource, IObservable<TTimeout>> _timeoutSelector;
        private readonly IObservable<TSource> _other;

        public Timeout(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutSelector, IObservable<TSource> other)
        {
            _source = source;
            _firstTimeout = firstTimeout;
            _timeoutSelector = timeoutSelector;
            _other = other;
        }

        protected override _ CreateSink(IObserver<TSource> observer) => new(this, observer);

        protected override void Run(_ sink) => sink.Run(this);

        internal sealed class _ : IdentitySink<TSource>
        {
            private readonly Func<TSource, IObservable<TTimeout>> _timeoutSelector;
            private readonly IObservable<TSource> _other;

            private SerialDisposableValue _sourceDisposable;
            private IDisposable? _timerDisposable;
            private long _index;

            public _(Timeout<TSource, TTimeout> parent, IObserver<TSource> observer)
                : base(observer)
            {
                _timeoutSelector = parent._timeoutSelector;
                _other = parent._other;
            }

            public void Run(Timeout<TSource, TTimeout> parent)
            {
                SetTimer(parent._firstTimeout, 0L);

                _sourceDisposable.TrySetFirst(parent._source.SubscribeSafe(this));
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _sourceDisposable.Dispose();
                    Disposable.Dispose(ref _timerDisposable);
                }

                base.Dispose(disposing);
            }

            public override void OnNext(TSource value)
            {
                var idx = Volatile.Read(ref _index);
                if (idx != long.MaxValue)
                {
                    if (Interlocked.CompareExchange(ref _index, idx + 1, idx) == idx)
                    {
                        // Do not use Disposable.TryDispose here, we need the field
                        // for the next timer
                        Volatile.Read(ref _timerDisposable)?.Dispose();

                        ForwardOnNext(value);

                        IObservable<TTimeout> timeoutSource;
                        try
                        {
                            timeoutSource = _timeoutSelector(value);
                        }
                        catch (Exception ex)
                        {
                            ForwardOnError(ex);
                            return;
                        }

                        SetTimer(timeoutSource, idx + 1);
                    }
                }
            }

            public override void OnError(Exception error)
            {
                if (Interlocked.Exchange(ref _index, long.MaxValue) != long.MaxValue)
                {
                    ForwardOnError(error);
                }
            }

            public override void OnCompleted()
            {
                if (Interlocked.Exchange(ref _index, long.MaxValue) != long.MaxValue)
                {
                    ForwardOnCompleted();
                }
            }

            private void Timeout(long idx)
            {
                if (Volatile.Read(ref _index) == idx
                    && Interlocked.CompareExchange(ref _index, long.MaxValue, idx) == idx)
                {
                    _sourceDisposable.Disposable = _other.SubscribeSafe(GetForwarder());
                }
            }

            private bool TimeoutError(long idx, Exception error)
            {
                if (Volatile.Read(ref _index) == idx
                    && Interlocked.CompareExchange(ref _index, long.MaxValue, idx) == idx)
                {
                    ForwardOnError(error);
                    return true;
                }

                return false;
            }

            private void SetTimer(IObservable<TTimeout> timeout, long idx)
            {
                var timeoutObserver = new TimeoutObserver(this, idx);

                if (Disposable.TrySetSerial(ref _timerDisposable, timeoutObserver))
                {
                    var d = timeout.Subscribe(timeoutObserver);
                    timeoutObserver.SetResource(d);
                }
            }

            private sealed class TimeoutObserver : SafeObserver<TTimeout>
            {
                private readonly _ _parent;
                private readonly long _id;

                public TimeoutObserver(_ parent, long id)
                {
                    _parent = parent;
                    _id = id;
                }

                public override void OnNext(TTimeout value)
                {
                    OnCompleted();
                }

                public override void OnError(Exception error)
                {
                    if (!_parent.TimeoutError(_id, error))
                    {
                        Dispose();
                    }
                }

                public override void OnCompleted()
                {
                    _parent.Timeout(_id);

                    Dispose();
                }

            }
        }
    }
}
