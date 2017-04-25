// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Window<TSource>
    {
        internal sealed class Count : Producer<IObservable<TSource>, Count._>
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

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly Queue<ISubject<TSource>> _queue = new Queue<ISubject<TSource>>();
                private readonly SingleAssignmentDisposable _m = new SingleAssignmentDisposable();
                private readonly RefCountDisposable _refCountDisposable;

                private readonly int _count;
                private readonly int _skip;

                public _(Count parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _refCountDisposable = new RefCountDisposable(_m);

                    _count = parent._count;
                    _skip = parent._skip;
                }

                private int _n;

                public IDisposable Run(IObservable<TSource> source)
                {
                    _n = 0;

                    var firstWindow = CreateWindow();
                    base._observer.OnNext(firstWindow);

                    _m.Disposable = source.SubscribeSafe(this);

                    return _refCountDisposable;
                }

                private IObservable<TSource> CreateWindow()
                {
                    var s = new Subject<TSource>();
                    _queue.Enqueue(s);
                    return new WindowObservable<TSource>(s, _refCountDisposable);
                }

                public void OnNext(TSource value)
                {
                    foreach (var s in _queue)
                        s.OnNext(value);

                    var c = _n - _count + 1;
                    if (c >= 0 && c % _skip == 0)
                    {
                        var s = _queue.Dequeue();
                        s.OnCompleted();
                    }

                    _n++;
                    if (_n % _skip == 0)
                    {
                        var newWindow = CreateWindow();
                        base._observer.OnNext(newWindow);
                    }
                }

                public void OnError(Exception error)
                {
                    while (_queue.Count > 0)
                        _queue.Dequeue().OnError(error);

                    base._observer.OnError(error);
                    base.Dispose();
                }

                public void OnCompleted()
                {
                    while (_queue.Count > 0)
                        _queue.Dequeue().OnCompleted();

                    base._observer.OnCompleted();
                    base.Dispose();
                }
            }
        }

        internal sealed class TimeSliding : Producer<IObservable<TSource>, TimeSliding._>
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

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly object _gate = new object();
                private readonly Queue<ISubject<TSource>> _q = new Queue<ISubject<TSource>>();
                private readonly SerialDisposable _timerD = new SerialDisposable();

                private readonly IScheduler _scheduler;
                private readonly TimeSpan _timeShift;

                public _(TimeSliding parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _scheduler = parent._scheduler;
                    _timeShift = parent._timeShift;
                }

                private RefCountDisposable _refCountDisposable;
                private TimeSpan _totalTime;
                private TimeSpan _nextShift;
                private TimeSpan _nextSpan;

                public IDisposable Run(TimeSliding parent)
                {
                    _totalTime = TimeSpan.Zero;
                    _nextShift = parent._timeShift;
                    _nextSpan = parent._timeSpan;

                    var groupDisposable = new CompositeDisposable(2) { _timerD };
                    _refCountDisposable = new RefCountDisposable(groupDisposable);

                    CreateWindow();
                    CreateTimer();

                    groupDisposable.Add(parent._source.SubscribeSafe(this));

                    return _refCountDisposable;
                }

                private void CreateWindow()
                {
                    var s = new Subject<TSource>();
                    _q.Enqueue(s);
                    base._observer.OnNext(new WindowObservable<TSource>(s, _refCountDisposable));
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
                        _nextSpan += _timeShift;
                    if (isShift)
                        _nextShift += _timeShift;

                    m.Disposable = _scheduler.Schedule(new State { isSpan = isSpan, isShift = isShift }, ts, Tick);
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
                        // BREAKING CHANGE v2 > v1.x - Making behavior of sending OnCompleted to the window
                        //                             before sending out a new window consistent across all
                        //                             overloads of Window and Buffer. Before v2, the two
                        //                             operations below were reversed.
                        //
                        if (state.isSpan)
                        {
                            var s = _q.Dequeue();
                            s.OnCompleted();
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
                            s.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        foreach (var s in _q)
                            s.OnError(error);

                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        foreach (var s in _q)
                            s.OnCompleted();

                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }

        internal sealed class TimeHopping : Producer<IObservable<TSource>, TimeHopping._>
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

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly object _gate = new object();

                public _(IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                }

                private Subject<TSource> _subject;
                private RefCountDisposable _refCountDisposable;

                public IDisposable Run(TimeHopping parent)
                {
                    var groupDisposable = new CompositeDisposable(2);
                    _refCountDisposable = new RefCountDisposable(groupDisposable);

                    CreateWindow();

                    groupDisposable.Add(parent._scheduler.SchedulePeriodic(parent._timeSpan, Tick));
                    groupDisposable.Add(parent._source.SubscribeSafe(this));

                    return _refCountDisposable;
                }

                private void Tick()
                {
                    lock (_gate)
                    {
                        _subject.OnCompleted();
                        CreateWindow();
                    }
                }

                private void CreateWindow()
                {
                    _subject = new Subject<TSource>();
                    base._observer.OnNext(new WindowObservable<TSource>(_subject, _refCountDisposable));
                }

                public void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        _subject.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _subject.OnError(error);

                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        _subject.OnCompleted();

                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }

        internal sealed class Ferry : Producer<IObservable<TSource>, Ferry._>
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

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly object _gate = new object();
                private readonly SerialDisposable _timerD = new SerialDisposable();

                private readonly int _count;
                private readonly TimeSpan _timeSpan;
                private readonly IScheduler _scheduler;

                public _(Ferry parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _count = parent._count;
                    _timeSpan = parent._timeSpan;
                    _scheduler = parent._scheduler;
                }

                private Subject<TSource> _s;
                private int _n;

                private RefCountDisposable _refCountDisposable;

                public IDisposable Run(IObservable<TSource> source)
                {
                    _n = 0;

                    var groupDisposable = new CompositeDisposable(2) { _timerD };
                    _refCountDisposable = new RefCountDisposable(groupDisposable);

                    _s = new Subject<TSource>();
                    base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
                    CreateTimer(_s);

                    groupDisposable.Add(source.SubscribeSafe(this));

                    return _refCountDisposable;
                }

                private void CreateTimer(Subject<TSource> window)
                {
                    var m = new SingleAssignmentDisposable();
                    _timerD.Disposable = m;

                    m.Disposable = _scheduler.Schedule(window, _timeSpan, Tick);
                }

                private IDisposable Tick(IScheduler self, Subject<TSource> window)
                {
                    var d = Disposable.Empty;

                    var newWindow = default(Subject<TSource>);
                    lock (_gate)
                    {
                        if (window != _s)
                            return d;

                        _n = 0;
                        newWindow = new Subject<TSource>();

                        _s.OnCompleted();
                        _s = newWindow;
                        base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
                    }

                    CreateTimer(newWindow);

                    return d;
                }

                public void OnNext(TSource value)
                {
                    var newWindow = default(Subject<TSource>);

                    lock (_gate)
                    {
                        _s.OnNext(value);

                        _n++;
                        if (_n == _count)
                        {
                            _n = 0;
                            newWindow = new Subject<TSource>();

                            _s.OnCompleted();
                            _s = newWindow;
                            base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
                        }
                    }

                    if (newWindow != null)
                        CreateTimer(newWindow);
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _s.OnError(error);
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        _s.OnCompleted();
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }
    }

    internal static class Window<TSource, TWindowClosing>
    {
        internal sealed class Selector : Producer<IObservable<TSource>, Selector._>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<IObservable<TWindowClosing>> _windowClosingSelector;

            public Selector(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
            {
                _source = source;
                _windowClosingSelector = windowClosingSelector;
            }

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(_source);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly object _gate = new object();
                private readonly AsyncLock _windowGate = new AsyncLock();
                private readonly SerialDisposable _m = new SerialDisposable();

                private readonly Func<IObservable<TWindowClosing>> _windowClosingSelector;

                public _(Selector parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _windowClosingSelector = parent._windowClosingSelector;
                }

                private ISubject<TSource> _window;
                private RefCountDisposable _refCountDisposable;

                public IDisposable Run(IObservable<TSource> source)
                {
                    _window = new Subject<TSource>();

                    var groupDisposable = new CompositeDisposable(2) { _m };
                    _refCountDisposable = new RefCountDisposable(groupDisposable);

                    var window = new WindowObservable<TSource>(_window, _refCountDisposable);
                    base._observer.OnNext(window);

                    groupDisposable.Add(source.SubscribeSafe(this));

                    _windowGate.Wait(CreateWindowClose);

                    return _refCountDisposable;
                }

                private void CreateWindowClose()
                {
                    var windowClose = default(IObservable<TWindowClosing>);
                    try
                    {
                        windowClose = _windowClosingSelector();
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
                    closingSubscription.Disposable = windowClose.SubscribeSafe(new WindowClosingObserver(this, closingSubscription));
                }

                private void CloseWindow(IDisposable closingSubscription)
                {
                    closingSubscription.Dispose();

                    lock (_gate)
                    {
                        _window.OnCompleted();
                        _window = new Subject<TSource>();

                        var window = new WindowObservable<TSource>(_window, _refCountDisposable);
                        base._observer.OnNext(window);
                    }

                    _windowGate.Wait(CreateWindowClose);
                }

                private sealed class WindowClosingObserver : IObserver<TWindowClosing>
                {
                    private readonly _ _parent;
                    private readonly IDisposable _self;

                    public WindowClosingObserver(_ parent, IDisposable self)
                    {
                        _parent = parent;
                        _self = self;
                    }

                    public void OnNext(TWindowClosing value)
                    {
                        _parent.CloseWindow(_self);
                    }

                    public void OnError(Exception error)
                    {
                        _parent.OnError(error);
                    }

                    public void OnCompleted()
                    {
                        _parent.CloseWindow(_self);
                    }
                }

                public void OnNext(TSource value)
                {
                    lock (_gate)
                    {
                        _window.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _window.OnError(error);
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        _window.OnCompleted();
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }

        internal sealed class Boundaries : Producer<IObservable<TSource>, Boundaries._>
        {
            private readonly IObservable<TSource> _source;
            private readonly IObservable<TWindowClosing> _windowBoundaries;

            public Boundaries(IObservable<TSource> source, IObservable<TWindowClosing> windowBoundaries)
            {
                _source = source;
                _windowBoundaries = windowBoundaries;
            }

            protected override _ CreateSink(IObserver<IObservable<TSource>> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run(this);

            internal sealed class _ : Sink<IObservable<TSource>>, IObserver<TSource>
            {
                private readonly object _gate = new object();

                private readonly IObservable<TWindowClosing> _windowBoundaries;

                public _(Boundaries parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _windowBoundaries = parent._windowBoundaries;
                }

                private ISubject<TSource> _window;
                private RefCountDisposable _refCountDisposable;

                public IDisposable Run(Boundaries parent)
                {
                    _window = new Subject<TSource>();

                    var d = new CompositeDisposable(2);
                    _refCountDisposable = new RefCountDisposable(d);

                    var window = new WindowObservable<TSource>(_window, _refCountDisposable);
                    base._observer.OnNext(window);

                    d.Add(parent._source.SubscribeSafe(this));
                    d.Add(parent._windowBoundaries.SubscribeSafe(new WindowClosingObserver(this)));

                    return _refCountDisposable;
                }

                private sealed class WindowClosingObserver : IObserver<TWindowClosing>
                {
                    private readonly _ _parent;

                    public WindowClosingObserver(_ parent)
                    {
                        _parent = parent;
                    }

                    public void OnNext(TWindowClosing value)
                    {
                        lock (_parent._gate)
                        {
                            _parent._window.OnCompleted();
                            _parent._window = new Subject<TSource>();

                            var window = new WindowObservable<TSource>(_parent._window, _parent._refCountDisposable);
                            _parent._observer.OnNext(window);
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
                        _window.OnNext(value);
                    }
                }

                public void OnError(Exception error)
                {
                    lock (_gate)
                    {
                        _window.OnError(error);
                        base._observer.OnError(error);
                        base.Dispose();
                    }
                }

                public void OnCompleted()
                {
                    lock (_gate)
                    {
                        _window.OnCompleted();
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }
    }

    internal sealed class WindowObservable<TSource> : AddRef<TSource>
    {
        public WindowObservable(IObservable<TSource> source, RefCountDisposable refCount)
            : base(source, refCount)
        {
        }
    }
}
