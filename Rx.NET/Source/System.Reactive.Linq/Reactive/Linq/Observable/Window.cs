// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class Window<TSource> : Producer<IObservable<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly int _count;
        private readonly int _skip;

        private readonly TimeSpan _timeSpan;
        private readonly TimeSpan _timeShift;
        private readonly IScheduler _scheduler;

        public Window(IObservable<TSource> source, int count, int skip)
        {
            _source = source;
            _count = count;
            _skip = skip;
        }

        public Window(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            _source = source;
            _timeSpan = timeSpan;
            _timeShift = timeShift;
            _scheduler = scheduler;
        }

        public Window(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            _source = source;
            _timeSpan = timeSpan;
            _count = count;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<IObservable<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
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

        class _ : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource> _parent;

            public _(Window<TSource> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private Queue<ISubject<TSource>> _queue;
            private int _n;
            private SingleAssignmentDisposable _m;
            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _queue = new Queue<ISubject<TSource>>();
                _n = 0;
                _m = new SingleAssignmentDisposable();
                _refCountDisposable = new RefCountDisposable(_m);

                var firstWindow = CreateWindow();
                base._observer.OnNext(firstWindow);

                _m.Disposable = _parent._source.SubscribeSafe(this);

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

                var c = _n - _parent._count + 1;
                if (c >= 0 && c % _parent._skip == 0)
                {
                    var s = _queue.Dequeue();
                    s.OnCompleted();
                }

                _n++;
                if (_n % _parent._skip == 0)
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

        class τ : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource> _parent;

            public τ(Window<TSource> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private TimeSpan _totalTime;
            private TimeSpan _nextShift;
            private TimeSpan _nextSpan;

            private object _gate;
            private Queue<ISubject<TSource>> _q;

            private SerialDisposable _timerD;
            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _totalTime = TimeSpan.Zero;
                _nextShift = _parent._timeShift;
                _nextSpan = _parent._timeSpan;

                _gate = new object();
                _q = new Queue<ISubject<TSource>>();

                _timerD = new SerialDisposable();

                var groupDisposable = new CompositeDisposable(2) { _timerD };
                _refCountDisposable = new RefCountDisposable(groupDisposable);

                CreateWindow();
                CreateTimer();

                groupDisposable.Add(_parent._source.SubscribeSafe(this));

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

        class π : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource> _parent;

            public π(Window<TSource> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private Subject<TSource> _subject;
            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _gate = new object();

                var groupDisposable = new CompositeDisposable(2);
                _refCountDisposable = new RefCountDisposable(groupDisposable);

                CreateWindow();

                groupDisposable.Add(_parent._scheduler.SchedulePeriodic(_parent._timeSpan, Tick));
                groupDisposable.Add(_parent._source.SubscribeSafe(this));

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

        class μ : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource> _parent;

            public μ(Window<TSource> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private object _gate;
            private ISubject<TSource> _s;
            private int _n;
            private int _windowId;

            private SerialDisposable _timerD;
            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _gate = new object();
                _s = default(ISubject<TSource>);
                _n = 0;
                _windowId = 0;

                _timerD = new SerialDisposable();
                var groupDisposable = new CompositeDisposable(2) { _timerD };
                _refCountDisposable = new RefCountDisposable(groupDisposable);

                _s = new Subject<TSource>();
                base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
                CreateTimer(0);

                groupDisposable.Add(_parent._source.SubscribeSafe(this));

                return _refCountDisposable;
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

                    _s.OnCompleted();
                    _s = new Subject<TSource>();
                    base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
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
                    _s.OnNext(value);

                    _n++;
                    if (_n == _parent._count)
                    {
                        newWindow = true;
                        _n = 0;
                        newId = ++_windowId;

                        _s.OnCompleted();
                        _s = new Subject<TSource>();
                        base._observer.OnNext(new WindowObservable<TSource>(_s, _refCountDisposable));
                    }
                }

                if (newWindow)
                    CreateTimer(newId);
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

    class Window<TSource, TWindowClosing> : Producer<IObservable<TSource>>
    {
        private readonly IObservable<TSource> _source;
        private readonly Func<IObservable<TWindowClosing>> _windowClosingSelector;
        private readonly IObservable<TWindowClosing> _windowBoundaries;

        public Window(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
        {
            _source = source;
            _windowClosingSelector = windowClosingSelector;
        }

        public Window(IObservable<TSource> source, IObservable<TWindowClosing> windowBoundaries)
        {
            _source = source;
            _windowBoundaries = windowBoundaries;
        }

        protected override IDisposable Run(IObserver<IObservable<TSource>> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_windowClosingSelector != null)
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

        class _ : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource, TWindowClosing> _parent;

            public _(Window<TSource, TWindowClosing> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private ISubject<TSource> _window;
            private object _gate;
            private AsyncLock _windowGate;

            private SerialDisposable _m;
            private RefCountDisposable _refCountDisposable;

            public IDisposable Run()
            {
                _window = new Subject<TSource>();
                _gate = new object();
                _windowGate = new AsyncLock();

                _m = new SerialDisposable();
                var groupDisposable = new CompositeDisposable(2) { _m };
                _refCountDisposable = new RefCountDisposable(groupDisposable);

                var window = new WindowObservable<TSource>(_window, _refCountDisposable);
                base._observer.OnNext(window);

                groupDisposable.Add(_parent._source.SubscribeSafe(this));

                _windowGate.Wait(CreateWindowClose);

                return _refCountDisposable;
            }

            private void CreateWindowClose()
            {
                var windowClose = default(IObservable<TWindowClosing>);
                try
                {
                    windowClose = _parent._windowClosingSelector();
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
                closingSubscription.Disposable = windowClose.SubscribeSafe(new ω(this, closingSubscription));
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

            class ω : IObserver<TWindowClosing>
            {
                private readonly _ _parent;
                private readonly IDisposable _self;

                public ω(_ parent, IDisposable self)
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

        class β : Sink<IObservable<TSource>>, IObserver<TSource>
        {
            private readonly Window<TSource, TWindowClosing> _parent;

            public β(Window<TSource, TWindowClosing> parent, IObserver<IObservable<TSource>> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            private ISubject<TSource> _window;
            private object _gate;

            private RefCountDisposable _refCountDisposable;            

            public IDisposable Run()
            {
                _window = new Subject<TSource>();
                _gate = new object();

                var d = new CompositeDisposable(2);
                _refCountDisposable = new RefCountDisposable(d);

                var window = new WindowObservable<TSource>(_window, _refCountDisposable);
                base._observer.OnNext(window);

                d.Add(_parent._source.SubscribeSafe(this));
                d.Add(_parent._windowBoundaries.SubscribeSafe(new ω(this)));

                return _refCountDisposable;
            }

            class ω : IObserver<TWindowClosing>
            {
                private readonly β _parent;

                public ω(β parent)
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

    class WindowObservable<TSource> : AddRef<TSource>
    {
        public WindowObservable(IObservable<TSource> source, RefCountDisposable refCount)
            : base(source, refCount)
        {
        }
    }
}
#endif