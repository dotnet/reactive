// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
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

            protected override _ CreateSink(IObserver<TResult> observer) => new(this, observer);

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

                public void Run(IScheduler scheduler)
                {
                    var longRunning = scheduler.AsLongRunning();
                    if (longRunning != null)
                    {
                        SetUpstream(longRunning.ScheduleLongRunning(this, static (@this, c) => @this.Loop(c)));
                    }
                    else
                    {
                        SetUpstream(scheduler.Schedule(this, static (@this, a) => @this.LoopRec(a)));
                    }
                }

                private void Loop(ICancelable cancel)
                {
                    while (!cancel.IsDisposed)
                    {
                        bool hasResult;
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
                            ForwardOnNext(result!);
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
                    bool hasResult;
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
                        ForwardOnNext(result!);
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

            protected override _ CreateSink(IObserver<TResult> observer) => new(this, observer);

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
                private TResult? _result;

                private MultipleAssignmentDisposableValue _timerDisposable;

                public void Run(IScheduler outerScheduler, TState initialState)
                {
                    var timer = new SingleAssignmentDisposable();
                    _timerDisposable.Disposable = timer;
                    timer.Disposable = outerScheduler.Schedule((@this: this, initialState), static (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.initialState));
                }

                protected override void Dispose(bool disposing)
                {
                    _timerDisposable.Dispose();
                    base.Dispose(disposing);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    if (_hasResult)
                    {
                        ForwardOnNext(_result!);
                    }

                    var time = default(DateTimeOffset);

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
                    _timerDisposable.Disposable = timer;
                    timer.Disposable = self.Schedule((@this: this, state), time, static (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.state));

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

            protected override _ CreateSink(IObserver<TResult> observer) => new(this, observer);

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
                private TResult? _result;

                private MultipleAssignmentDisposableValue _timerDisposable;

                public void Run(IScheduler outerScheduler, TState initialState)
                {
                    var timer = new SingleAssignmentDisposable();
                    _timerDisposable.Disposable = timer;
                    timer.Disposable = outerScheduler.Schedule((@this: this, initialState), static (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.initialState));
                }

                protected override void Dispose(bool disposing)
                {
                    _timerDisposable.Dispose();
                    base.Dispose(disposing);
                }

                private IDisposable InvokeRec(IScheduler self, TState state)
                {
                    if (_hasResult)
                    {
                        ForwardOnNext(_result!);
                    }

                    var time = default(TimeSpan);

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
                    _timerDisposable.Disposable = timer;
                    timer.Disposable = self.Schedule((@this: this, state), time, static (scheduler, tuple) => tuple.@this.InvokeRec(scheduler, tuple.state));

                    return Disposable.Empty;
                }
            }
        }
    }
}
