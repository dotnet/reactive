// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Buffer<TSource>
    {
        internal sealed class Count : Producer<IList<TSource>, Count._>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;
            private readonly int _skip;

            public Count(IObservable<TSource> source, int count, int skip)
            {
                _source = source;
                _count = count;
                _skip = skip;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly Queue<IList<TSource>> _queue = new Queue<IList<TSource>>();

                private readonly int _count;
                private readonly int _skip;

                public _(Count parent, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _count = parent._count;
                    _skip = parent._skip;
                }

                private int _n;

                public override void Run(IObservable<TSource> source)
                {
                    _n = 0;

                    CreateWindow();
                    base.Run(source);
                }

                private void CreateWindow()
                {
                    var s = new List<TSource>();
                    _queue.Enqueue(s);
                }

                public override void OnNext(TSource value)
                {
                    foreach (var s in _queue)
                        s.Add(value);

                    var c = _n - _count + 1;
                    if (c >= 0 && c % _skip == 0)
                    {
                        var s = _queue.Dequeue();
                        if (s.Count > 0)
                            ForwardOnNext(s);
                    }

                    _n++;
                    if (_n % _skip == 0)
                        CreateWindow();
                }

                public override void OnError(Exception error)
                {
                    while (_queue.Count > 0)
                        _queue.Dequeue().Clear();

                    ForwardOnError(error);
                }

                public override void OnCompleted()
                {
                    while (_queue.Count > 0)
                    {
                        var s = _queue.Dequeue();
                        if (s.Count > 0)
                            ForwardOnNext(s);
                    }

                    ForwardOnCompleted();
                }
            }
        }

        internal sealed class TimeSliding : Producer<IList<TSource>, TimeSliding._>
        {
            private readonly IObservable<TSource> _source;

            private readonly TimeSpan _timeSpan;
            private readonly TimeSpan _timeShift;
            private readonly IScheduler _scheduler;

            public TimeSliding(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
            {
                _source = source;
                _timeSpan = timeSpan;
                _timeShift = timeShift;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly TimeSpan _timeShift;
                private readonly IScheduler _scheduler;

                private readonly object _gate = new object();
                private readonly Queue<List<TSource>> _q = new Queue<List<TSource>>();
                
                private IDisposable _timerSerial;

                public _(TimeSliding parent, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _timeShift = parent._timeShift;
                    _scheduler = parent._scheduler;
                }

                private TimeSpan _totalTime;
                private TimeSpan _nextShift;
                private TimeSpan _nextSpan;

                public void Run(TimeSliding parent)
                {
                    _totalTime = TimeSpan.Zero;
                    _nextShift = parent._timeShift;
                    _nextSpan = parent._timeSpan;

                    CreateWindow();
                    CreateTimer();

                    SetUpstream(parent._source.SubscribeSafe(this));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _timerSerial);
                    }
                    base.Dispose(disposing);
                }

                private void CreateWindow()
                {
                    var s = new List<TSource>();
                    _q.Enqueue(s);
                }

                private void CreateTimer()
                {
                    var m = new SingleAssignmentDisposable();

                    Disposable.TrySetSerial(ref _timerSerial, m);

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
                        _nextSpan += _timeShift;
                    if (isShift)
                        _nextShift += _timeShift;

                    m.Disposable = _scheduler.Schedule(new State { isSpan = isSpan, isShift = isShift }, ts, Tick);
                }

                private struct State
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
                            ForwardOnNext(s);
                        }

                        if (state.isShift)
                        {
                            CreateWindow();
                        }
                    }

                    CreateTimer();

                    return Disposable.Empty;
                }

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        foreach (var s in _q)
                            s.Add(value);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        while (_q.Count > 0)
                            _q.Dequeue().Clear();

                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        while (_q.Count > 0)
                            ForwardOnNext(_q.Dequeue());

                        ForwardOnCompleted();
                    }
                }
            }
        }

        internal sealed class TimeHopping : Producer<IList<TSource>, TimeHopping._>
        {
            private readonly IObservable<TSource> _source;

            private readonly TimeSpan _timeSpan;
            private readonly IScheduler _scheduler;

            public TimeHopping(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
            {
                _source = source;
                _timeSpan = timeSpan;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly object _gate = new object();

                public _(IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                }

                private List<TSource> _list;

                private IDisposable _periodicDisposable;

                public void Run(TimeHopping parent)
                {
                    _list = new List<TSource>();

                    Disposable.SetSingle(ref _periodicDisposable, parent._scheduler.SchedulePeriodic(parent._timeSpan, Tick));
                    SetUpstream(parent._source.SubscribeSafe(this));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _periodicDisposable);
                    }
                    base.Dispose(disposing);
                }

                private void Tick()
                {
                    lock (_gate)
                    {
                        ForwardOnNext(_list);
                        _list = new List<TSource>();
                    }
                }

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        _list.Add(value);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _list.Clear();

                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        ForwardOnNext(_list);
                        ForwardOnCompleted();
                    }
                }
            }
        }

        internal sealed class Ferry : Producer<IList<TSource>, Ferry._>
        {
            private readonly IObservable<TSource> _source;
            private readonly int _count;
            private readonly TimeSpan _timeSpan;
            private readonly IScheduler _scheduler;

            public Ferry(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
            {
                _source = source;
                _timeSpan = timeSpan;
                _count = count;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly Ferry _parent;

                private readonly object _gate = new object();
                private IDisposable _timerSerial;

                public _(Ferry parent, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _parent = parent;
                }

                private IList<TSource> _s;
                private int _n;
                private int _windowId;

                public void Run()
                {
                    _s = new List<TSource>();
                    _n = 0;
                    _windowId = 0;

                    CreateTimer(0);

                    SetUpstream(_parent._source.SubscribeSafe(this));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _timerSerial);
                    }
                    base.Dispose(disposing);
                }

                private void CreateTimer(int id)
                {
                    var m = new SingleAssignmentDisposable();
                    Disposable.TrySetSerial(ref _timerSerial, m);

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
                        ForwardOnNext(res);

                        CreateTimer(newId);
                    }

                    return d;
                }

                public override void OnNext(TSource value)
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
                            ForwardOnNext(res);
                        }

                        if (newWindow)
                            CreateTimer(newId);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _s.Clear();
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        ForwardOnNext(_s);
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }

    internal static class Buffer<TSource, TBufferClosing>
    {
        internal sealed class Selector : Producer<IList<TSource>, Selector._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<IObservable<TBufferClosing>> _bufferClosingSelector;

            public Selector(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
            {
                _source = source;
                _bufferClosingSelector = bufferClosingSelector;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly object _gate = new object();
                private readonly AsyncLock _bufferGate = new AsyncLock();
                private IDisposable _bufferClosingSerialDisposable;

                private readonly Func<IObservable<TBufferClosing>> _bufferClosingSelector;

                public _(Selector parent, IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                    _bufferClosingSelector = parent._bufferClosingSelector;
                }

                private IList<TSource> _buffer;

                public override void Run(IObservable<TSource> source)
                {
                    _buffer = new List<TSource>();

                    SetUpstream(source.SubscribeSafe(this));

                    _bufferGate.Wait(CreateBufferClose);
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _bufferClosingSerialDisposable);
                    }
                    base.Dispose(disposing);
                }

                private void CreateBufferClose()
                {
                    var bufferClose = default(IObservable<TBufferClosing>);
                    try
                    {
                        bufferClose = _bufferClosingSelector();
                    }
                    catch (Exception exception)
                    {
                        lock (_gate)
                        {
                            ForwardOnError(exception);
                        }
                        return;
                    }

                    var closingSubscription = new SingleAssignmentDisposable();
                    Disposable.TrySetSerial(ref _bufferClosingSerialDisposable, closingSubscription);
                    closingSubscription.Disposable = bufferClose.SubscribeSafe(new BufferClosingObserver(this, closingSubscription));
                }

                private void CloseBuffer(IDisposable closingSubscription)
                {
                    closingSubscription.Dispose();

                    lock (_gate)
                    {
                        var res = _buffer;
                        _buffer = new List<TSource>();
                        ForwardOnNext(res);
                    }

                    _bufferGate.Wait(CreateBufferClose);
                }

                private sealed class BufferClosingObserver : IObserver<TBufferClosing>
                {
                    private readonly _ _parent;
                    private readonly IDisposable _self;

                    public BufferClosingObserver(_ parent, IDisposable self)
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

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        _buffer.Add(value);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _buffer.Clear();
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        ForwardOnNext(_buffer);
                        ForwardOnCompleted();
                    }
                }
            }
        }

        internal sealed class Boundaries : Producer<IList<TSource>, Boundaries._>
        {
            private readonly IObservable<TSource> _source;
            private readonly IObservable<TBufferClosing> _bufferBoundaries;

            public Boundaries(IObservable<TSource> source, IObservable<TBufferClosing> bufferBoundaries)
            {
                _source = source;
                _bufferBoundaries = bufferBoundaries;
            }

            protected override _ CreateSink(IObserver<IList<TSource>> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<TSource, IList<TSource>> 
            {
                private readonly object _gate = new object();

                public _(IObserver<IList<TSource>> observer)
                    : base(observer)
                {
                }

                private IList<TSource> _buffer;

                private IDisposable _boundariesDisposable;

                public void Run(Boundaries parent)
                {
                    _buffer = new List<TSource>();

                    SetUpstream(parent._source.SubscribeSafe(this));
                    Disposable.SetSingle(ref _boundariesDisposable, parent._bufferBoundaries.SubscribeSafe(new BufferClosingObserver(this)));
                }

                protected override void Dispose(bool disposing)
                {
                    if (disposing)
                    {
                        Disposable.TryDispose(ref _boundariesDisposable);
                    }
                    base.Dispose(disposing);
                }

                private sealed class BufferClosingObserver : IObserver<TBufferClosing>
                {
                    private readonly _ _parent;

                    public BufferClosingObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public void OnNext(TBufferClosing value)
                    {
                        lock (_parent._gate)
                        {
                            var res = _parent._buffer;
                            _parent._buffer = new List<TSource>();
                            _parent.ForwardOnNext(res);
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

                public override void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        _buffer.Add(value);
                    }
                }

                public override void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _buffer.Clear();
                        ForwardOnError(error);
                    }
                }

                public override void OnCompleted()
                {
                    lock (_gate)
                    {
                        ForwardOnNext(_buffer);
                        ForwardOnCompleted();
                    }
                }
            }
        }
    }
}
