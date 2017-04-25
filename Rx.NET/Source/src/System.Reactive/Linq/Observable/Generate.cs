// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Generate<TState, TResult>
    {
        internal sealed class NoTime : Producer<TResult, NoTime._>
        {
            private readonly TState _initialState;
            private readonly Func<TState, bool> _condition;
            private readonly Func<TState, TState> _iterate;
            private readonly Func<TState, TResult> _resultSelector;
            private readonly IScheduler _scheduler;

            public NoTime(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
            {
                _initialState = initialState;
                _condition = condition;
                _iterate = iterate;
                _resultSelector = resultSelector;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TResult>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly NoTime _parent;

                public _(NoTime parent, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                private TState _state;
                private bool _first;

                public IDisposable Run()
                {
                    _state = _parent._initialState;
                    _first = true;

                    var longRunning = _parent._scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        return longRunning.ScheduleLongRunning(Loop);
                    }
                    else
                    {
                        return _parent._scheduler.Schedule(LoopRec);
                    }
                }

                private void Loop(ICancelable cancel)
                {
                    while (!cancel.IsDisposed)
                    {
                        var hasResult = false;
                        var result = default(TResult);
                        try
                        {
                            if (_first)
                            {
                                _first = false;
                            }
                            else
                            {
                                _state = _parent._iterate(_state);
                            }

                            hasResult = _parent._condition(_state);

                            if (hasResult)
                            {
                                result = _parent._resultSelector(_state);
                            }
                        }
                        catch (Exception exception)
                        {
                            base._observer.OnError(exception);
                            base.Dispose();
                            return;
                        }

                        if (hasResult)
                        {
                            base._observer.OnNext(result);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!cancel.IsDisposed)
                    {
                        base._observer.OnCompleted();
                    }

                    base.Dispose();
                }

                private void LoopRec(Action recurse)
                {
                    var hasResult = false;
                    var result = default(TResult);
                    try
                    {
                        if (_first)
                        {
                            _first = false;
                        }
                        else
                        {
                            _state = _parent._iterate(_state);
                        }

                        hasResult = _parent._condition(_state);

                        if (hasResult)
                        {
                            result = _parent._resultSelector(_state);
                        }
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return;
                    }

                    if (hasResult)
                    {
                        base._observer.OnNext(result);
                        recurse();
                    }
                    else
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                    }
                }
            }
        }

        internal sealed class Absolute : Producer<TResult, Absolute._>
        {
            private readonly TState _initialState;
            private readonly Func<TState, bool> _condition;
            private readonly Func<TState, TState> _iterate;
            private readonly Func<TState, TResult> _resultSelector;
            private readonly Func<TState, DateTimeOffset> _timeSelector;
            private readonly IScheduler _scheduler;

            public Absolute(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler)
            {
                _initialState = initialState;
                _condition = condition;
                _iterate = iterate;
                _resultSelector = resultSelector;
                _timeSelector = timeSelector;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TResult>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Absolute _parent;

                public _(Absolute parent, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                private bool _first;
                private bool _hasResult;
                private TResult _result;

                public IDisposable Run()
                {
                    _first = true;
                    _hasResult = false;
                    _result = default(TResult);

                    return _parent._scheduler.Schedule(_parent._initialState, InvokeRec);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    var time = default(DateTimeOffset);

                    if (_hasResult)
                    {
                        base._observer.OnNext(_result);
                    }

                    try
                    {
                        if (_first)
                        {
                            _first = false;
                        }
                        else
                        {
                            state = _parent._iterate(state);
                        }

                        _hasResult = _parent._condition(state);

                        if (_hasResult)
                        {
                            _result = _parent._resultSelector(state);
                            time = _parent._timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return Disposable.Empty;
                    }

                    if (!_hasResult)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return Disposable.Empty;
                    }

                    return self.Schedule(state, time, InvokeRec);
                }
            }
        }

        internal sealed class Relative : Producer<TResult, Relative._>
        {
            private readonly TState _initialState;
            private readonly Func<TState, bool> _condition;
            private readonly Func<TState, TState> _iterate;
            private readonly Func<TState, TResult> _resultSelector;
            private readonly Func<TState, TimeSpan> _timeSelector;
            private readonly IScheduler _scheduler;

            public Relative(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler)
            {
                _initialState = initialState;
                _condition = condition;
                _iterate = iterate;
                _resultSelector = resultSelector;
                _timeSelector = timeSelector;
                _scheduler = scheduler;
            }

            protected override _ CreateSink(IObserver<TResult> observer, IDisposable cancel) => new _(this, observer, cancel);

            protected override IDisposable Run(_ sink) => sink.Run();

            internal sealed class _ : Sink<TResult>
            {
                // CONSIDER: This sink has a parent reference that can be considered for removal.

                private readonly Relative _parent;

                public _(Relative parent, IObserver<TResult> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    _parent = parent;
                }

                private bool _first;
                private bool _hasResult;
                private TResult _result;

                public IDisposable Run()
                {
                    _first = true;
                    _hasResult = false;
                    _result = default(TResult);

                    return _parent._scheduler.Schedule(_parent._initialState, InvokeRec);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    var time = default(TimeSpan);

                    if (_hasResult)
                    {
                        base._observer.OnNext(_result);
                    }

                    try
                    {
                        if (_first)
                        {
                            _first = false;
                        }
                        else
                        {
                            state = _parent._iterate(state);
                        }

                        _hasResult = _parent._condition(state);

                        if (_hasResult)
                        {
                            _result = _parent._resultSelector(state);
                            time = _parent._timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return Disposable.Empty;
                    }

                    if (!_hasResult)
                    {
                        base._observer.OnCompleted();
                        base.Dispose();
                        return Disposable.Empty;
                    }

                    return self.Schedule(state, time, InvokeRec);
                }
            }
        }
    }
}
