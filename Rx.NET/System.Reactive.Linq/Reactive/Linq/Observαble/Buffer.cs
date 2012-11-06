// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class Buffer<TSource> : Producer<IList<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly int _skip;

        private readonly TimeSpan _timeSpan;
        private readonly TimeSpan _timeShift;
        private readonly IScheduler _scheduler;

        public Buffer(IObservable<TSource> source, int count, int skip)
        {
            _source = source;
            _count = count;
            _skip = skip;
        }

        public Buffer(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            _source = source;
            _timeSpan = timeSpan;
            _timeShift = timeShift;
            _scheduler = scheduler;
        }

        public Buffer(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            _source = source;
            _timeSpan = timeSpan;
            _count = count;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_scheduler == null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_count > 0)
            {
                var sink = new μ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                if (_timeSpan == _timeShift)
                {
                    var sink = new π(this, observer, cancel);
                    setSink(sink);
                    return sink.Run();
                }
                else
                {
                    var sink = new τ(this, observer, cancel);
                    setSink(sink);
                    return sink.Run();
                }
            }
        }

        class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource> _parent;

            public _(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private Queue<IList<TSource>> _queue;
            private int _n;

            public IDisposable Run()
            {
                _queue = new Queue<IList<TSource>>();
                _n = 0;

                CreateWindow();
                return _parent._source.SubscribeSafe(this);
            }

            private void CreateWindow()
            {
                var s = new List<TSource>();
                _queue.Enqueue(s);
            }

            public void OnNext(TSource value)
            {
                foreach (var s in _queue)
                    s.Add(value);

                var c = _n - _parent._count + 1;
                if (c >= 0 && c % _parent._skip == 0)
                {
                    var s = _queue.Dequeue();
                    if (s.Count > 0)
                        base._observer.OnNext(s);
                }

                _n++;
                if (_n % _parent._skip == 0)
                    CreateWindow();
            }

            public void OnError(Exception error)
            {
                while (_queue.Count > 0)
                    _queue.Dequeue().Clear();

                base._observer.OnError(error);
                base.Dispose();
            }

            public void OnCompleted()
            {
                while (_queue.Count > 0)
                {
                    var s = _queue.Dequeue();
                    if (s.Count > 0)
                        base._observer.OnNext(s);
                }

                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class τ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource> _parent;

            public τ(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private TimeSpan _totalTime;
            private TimeSpan _nextShift;
            private TimeSpan _nextSpan;

            private object _gate;
            private Queue<List<TSource>> _q;

            private SerialDisposable _timerD;

            public IDisposable Run()
            {
                _totalTime = TimeSpan.Zero;
                _nextShift = _parent._timeShift;
                _nextSpan = _parent._timeSpan;

                _gate = new object();
                _q = new Queue<List<TSource>>();

                _timerD = new SerialDisposable();

                CreateWindow();
                CreateTimer();

                var subscription = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable { _timerD, subscription };
            }

            private void CreateWindow()
            {
                var s = new List<TSource>();
                _q.Enqueue(s);
            }

            private void CreateTimer()
            {
                var m = new SingleAssignmentDisposable();
                _timerD.Disposable = m;

                var isSpan = false;
                var isShift = false;
                if (_nextSpan == _nextShift)
                {
                    isSpan = true;
                    isShift = true;
                }
                else if (_nextSpan < _nextShift)
                    isSpan = true;
                else
                    isShift = true;

                var newTotalTime = isSpan ? _nextSpan : _nextShift;
                var ts = newTotalTime - _totalTime;
                _totalTime = newTotalTime;

                if (isSpan)
                    _nextSpan += _parent._timeShift;
                if (isShift)
                    _nextShift += _parent._timeShift;

                m.Disposable = _parent._scheduler.Schedule(new State { isSpan = isSpan, isShift = isShift }, ts, Tick);
            }

            struct State
            {
                public bool isSpan;
                public bool isShift;
            }

            private IDisposable Tick(IScheduler self, State state)
            {
                lock (_gate)
                {
                    //
                    // Before v2, the two operations below were reversed. This doesn't have an observable
                    // difference for Buffer, but is done to keep code consistent with Window, where we
                    // took a breaking change in v2 to ensure consistency across overloads. For more info,
                    // see the comment in Tick for Window.
                    //
                    if (state.isSpan)
                    {
                        var s = _q.Dequeue();
                        base._observer.OnNext(s);
                    }

                    if (state.isShift)
                    {
                        CreateWindow();
                    }
                }

                CreateTimer();

                return Disposable.Empty;
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    foreach (var s in _q)
                        s.Add(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    while (_q.Count > 0)
                        _q.Dequeue().Clear();

                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    while (_q.Count > 0)
                        base._observer.OnNext(_q.Dequeue());

                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        class π : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource> _parent;

            public π(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private List<TSource> _list;

            public IDisposable Run()
            {
                _gate = new object();
                _list = new List<TSource>();

                var d = _parent._scheduler.SchedulePeriodic(_parent._timeSpan, Tick);
                var s = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable(d, s);
            }

            private void Tick()
            {
                lock (_gate)
                {
                    base._observer.OnNext(_list);
                    _list = new List<TSource>();
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _list.Add(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    _list.Clear();

                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnNext(_list);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        class μ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource> _parent;

            public μ(Buffer<TSource> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private IList<TSource> _s;
            private int _n;
            private int _windowId;

            private SerialDisposable _timerD;

            public IDisposable Run()
            {
                _gate = new object();
                _s = default(IList<TSource>);
                _n = 0;
                _windowId = 0;

                _timerD = new SerialDisposable();

                _s = new List<TSource>();
                CreateTimer(0);

                var subscription = _parent._source.SubscribeSafe(this);

                return new CompositeDisposable { _timerD, subscription };
            }

            private void CreateTimer(int id)
            {
                var m = new SingleAssignmentDisposable();
                _timerD.Disposable = m;

                m.Disposable = _parent._scheduler.Schedule(id, _parent._timeSpan, Tick);
            }

            private IDisposable Tick(IScheduler self, int id)
            {
                var d = Disposable.Empty;

                var newId = 0;
                lock (_gate)
                {
                    if (id != _windowId)
                        return d;

                    _n = 0;
                    newId = ++_windowId;

                    var res = _s;
                    _s = new List<TSource>();
                    base._observer.OnNext(res);
                }

                CreateTimer(newId);

                return d;
            }

            public void OnNext(TSource value)
            {
                var newWindow = false;
                var newId = 0;

                lock (_gate)
                {
                    _s.Add(value);

                    _n++;
                    if (_n == _parent._count)
                    {
                        newWindow = true;
                        _n = 0;
                        newId = ++_windowId;

                        var res = _s;
                        _s = new List<TSource>();
                        base._observer.OnNext(res);
                    }
                }

                if (newWindow)
                    CreateTimer(newId);
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    _s.Clear();
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnNext(_s);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }

    class Buffer<TSource, TBufferClosing> : Producer<IList<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<IObservable<TBufferClosing>> _bufferClosingSelector;
        private readonly IObservable<TBufferClosing> _bufferBoundaries;

        public Buffer(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
        {
            _source = source;
            _bufferClosingSelector = bufferClosingSelector;
        }

        public Buffer(IObservable<TSource> source, IObservable<TBufferClosing> bufferBoundaries)
        {
            _source = source;
            _bufferBoundaries = bufferBoundaries;
        }

        protected override IDisposable Run(IObserver<IList<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_bufferClosingSelector != null)
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                var sink = new β(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class _ : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource, TBufferClosing> _parent;

            public _(Buffer<TSource, TBufferClosing> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IList<TSource> _buffer;
            private object _gate;
            private AsyncLock _bufferGate;

            private SerialDisposable _m;

            public IDisposable Run()
            {
                _buffer = new List<TSource>();
                _gate = new object();
                _bufferGate = new AsyncLock();

                _m = new SerialDisposable();
                var groupDisposable = new CompositeDisposable(2) { _m };

                groupDisposable.Add(_parent._source.SubscribeSafe(this));

                _bufferGate.Wait(CreateBufferClose);

                return groupDisposable;
            }

            private void CreateBufferClose()
            {
                var bufferClose = default(IObservable<TBufferClosing>);
                try
                {
                    bufferClose = _parent._bufferClosingSelector();
                }
                catch (Exception exception)
                {
                    lock (_gate)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                    }
                    return;
                }

                var closingSubscription = new SingleAssignmentDisposable();
                _m.Disposable = closingSubscription;
                closingSubscription.Disposable = bufferClose.SubscribeSafe(new ω(this, closingSubscription));
            }

            private void CloseBuffer(IDisposable closingSubscription)
            {
                closingSubscription.Dispose();

                lock (_gate)
                {
                    var res = _buffer;
                    _buffer = new List<TSource>();
                    base._observer.OnNext(res);
                }

                _bufferGate.Wait(CreateBufferClose);
            }

            class ω : IObserver<TBufferClosing>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public ω(_ parent, IDisposable self)
                {
                    _parent = parent;
                    _self = self;
                }

                public void OnNext(TBufferClosing value)
                {
                    _parent.CloseBuffer(_self);
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnCompleted()
                {
                    _parent.CloseBuffer(_self);
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _buffer.Add(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    _buffer.Clear();
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnNext(_buffer);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        class β : Sink<IList<TSource>>, IObserver<TSource>
        {
            private readonly Buffer<TSource, TBufferClosing> _parent;

            public β(Buffer<TSource, TBufferClosing> parent, IObserver<IList<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private IList<TSource> _buffer;
            private object _gate;

            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _buffer = new List<TSource>();
                _gate = new object();

                var d = new CompositeDisposable(2);
                _refCountDisposable = new RefCountDisposable(d);

                d.Add(_parent._source.SubscribeSafe(this));
                d.Add(_parent._bufferBoundaries.SubscribeSafe(new ω(this)));

                return _refCountDisposable;
            }

            class ω : IObserver<TBufferClosing>
            {
                private readonly β _parent;

                public ω(β parent)
                {
                    _parent = parent;
                }

                public void OnNext(TBufferClosing value)
                {
                    lock (_parent._gate)
                    {
                        var res = _parent._buffer;
                        _parent._buffer = new List<TSource>();
                        _parent._observer.OnNext(res);
                    }
                }

                public void OnError(Exception error)
                {
                    _parent.OnError(error);
                }

                public void OnCompleted()
                {
                    _parent.OnCompleted();
                }
            }

            public void OnNext(TSource value)
            {
                lock (_gate)
                {
                    _buffer.Add(value);
                }
            }

            public void OnError(Exception error)
            {
                lock (_gate)
                {
                    _buffer.Clear();
                    base._observer.OnError(error);
                    base.Dispose();
                }
            }

            public void OnCompleted()
            {
                lock (_gate)
                {
                    base._observer.OnNext(_buffer);
                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }
    }
}
#endif