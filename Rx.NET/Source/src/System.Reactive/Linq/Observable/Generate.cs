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

            protected override _ CreateSink(IObserver<TResult> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly Func<TState, bool> _condition;
                private readonly Func<TState, TState> _iterate;
                private readonly Func<TState, TResult> _resultSelector;

                public _(NoTime parent, IObserver<TResult> observer)
                    : base(observer)
                {
                    _condition = parent._condition;
                    _iterate = parent._iterate;
                    _resultSelector = parent._resultSelector;

                    _state = parent._initialState;
                    _first = true;
                }

                private TState _state;
                private bool _first;

                public void Run(IScheduler _scheduler)
                {
                    var longRunning = _scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        SetUpstream(longRunning.ScheduleLongRunning(this, (@this, c) => @this.Loop(c)));
                    }
                    else
                    {
                        SetUpstream(_scheduler.Schedule(this, (@this, a) => @this.LoopRec(a)));
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
                                _state = _iterate(_state);
                            }

                            hasResult = _condition(_state);

                            if (hasResult)
                            {
                                result = _resultSelector(_state);
                            }
                        }
                        catch (Exception exception)
                        {
                            ForwardOnError(exception);
                            return;
                        }

                        if (hasResult)
                        {
                            ForwardOnNext(result);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (!cancel.IsDisposed)
                    {
                        ForwardOnCompleted();
                    }
                }

                private void LoopRec(Action<_> recurse)
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
                            _state = _iterate(_state);
                        }

                        hasResult = _condition(_state);

                        if (hasResult)
                        {
                            result = _resultSelector(_state);
                        }
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return;
                    }

                    if (hasResult)
                    {
                        ForwardOnNext(result);
                        recurse(this);
                    }
                    else
                    {
                        ForwardOnCompleted();
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

            protected override _ CreateSink(IObserver<TResult> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler, _initialState);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly Func<TState, bool> _condition;
                private readonly Func<TState, TState> _iterate;
                private readonly Func<TState, TResult> _resultSelector;
                private readonly Func<TState, DateTimeOffset> _timeSelector;

                public _(Absolute parent, IObserver<TResult> observer)
                    : base(observer)
                {
                    _condition = parent._condition;
                    _iterate = parent._iterate;
                    _resultSelector = parent._resultSelector;
                    _timeSelector = parent._timeSelector;

                    _first = true;
                }

                private bool _first;
                private bool _hasResult;
                private TResult _result;

                private IDisposable _timerDisposable;

                public void Run(IScheduler outerScheduler, TState initialState)
                {
                    var timer = new SingleAssignmentDisposable();
                    Disposable.TrySetMultiple(ref _timerDisposable, timer);
                    timer.Disposable = outerScheduler.Schedule((@this: this, initialState), (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.initialState));
                }

                protected override void Dispose(bool disposing)
                {
                    Disposable.TryDispose(ref _timerDisposable);
                    base.Dispose(disposing);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    var time = default(DateTimeOffset);

                    if (_hasResult)
                    {
                        ForwardOnNext(_result);
                    }

                    try
                    {
                        if (_first)
                        {
                            _first = false;
                        }
                        else
                        {
                            state = _iterate(state);
                        }

                        _hasResult = _condition(state);

                        if (_hasResult)
                        {
                            _result = _resultSelector(state);
                            time = _timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return Disposable.Empty;
                    }

                    if (!_hasResult)
                    {
                        ForwardOnCompleted();
                        return Disposable.Empty;
                    }

                    var timer = new SingleAssignmentDisposable();
                    Disposable.TrySetMultiple(ref _timerDisposable, timer);
                    timer.Disposable = self.Schedule((@this: this, state), time, (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.state));

                    return Disposable.Empty;
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

            protected override _ CreateSink(IObserver<TResult> observer) => new _(this, observer);

            protected override void Run(_ sink) => sink.Run(_scheduler, _initialState);

            internal sealed class _ : IdentitySink<TResult>
            {
                private readonly Func<TState, bool> _condition;
                private readonly Func<TState, TState> _iterate;
                private readonly Func<TState, TResult> _resultSelector;
                private readonly Func<TState, TimeSpan> _timeSelector;


                public _(Relative parent, IObserver<TResult> observer)
                    : base(observer)
                {
                    _condition = parent._condition;
                    _iterate = parent._iterate;
                    _resultSelector = parent._resultSelector;
                    _timeSelector = parent._timeSelector;

                    _first = true;
                }

                private bool _first;
                private bool _hasResult;
                private TResult _result;

                private IDisposable _timerDisposable;

                public void Run(IScheduler outerScheduler, TState initialState)
                {
                    var timer = new SingleAssignmentDisposable();
                    Disposable.TrySetMultiple(ref _timerDisposable, timer);
                    timer.Disposable = outerScheduler.Schedule((@this: this, initialState), (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.initialState));
                }

                protected override void Dispose(bool disposing)
                {
                    Disposable.TryDispose(ref _timerDisposable);
                    base.Dispose(disposing);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    var time = default(TimeSpan);

                    if (_hasResult)
                    {
                        ForwardOnNext(_result);
                    }

                    try
                    {
                        if (_first)
                        {
                            _first = false;
                        }
                        else
                        {
                            state = _iterate(state);
                        }

                        _hasResult = _condition(state);

                        if (_hasResult)
                        {
                            _result = _resultSelector(state);
                            time = _timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        ForwardOnError(exception);
                        return Disposable.Empty;
                    }

                    if (!_hasResult)
                    {
                        ForwardOnCompleted();
                        return Disposable.Empty;
                    }

                    var timer = new SingleAssignmentDisposable();
                    Disposable.TrySetMultiple(ref _timerDisposable, timer);
                    timer.Disposable = self.Schedule((@this: this, state), time, (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.state));

                    return Disposable.Empty;
                }
            }
        }
    }
}
