// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class Generate<TState, TResult> : Producer<TResult>
    {
        private readonly TState _initialState;
        private readonly Func<TState, bool> _condition;
        private readonly Func<TState, TState> _iterate;
        private readonly Func<TState, TResult> _resultSelector;
        private readonly Func<TState, DateTimeOffset> _timeSelectorA;
        private readonly Func<TState, TimeSpan> _timeSelectorR;
        private readonly IScheduler _scheduler;

        public Generate(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
        {
            _initialState = initialState;
            _condition = condition;
            _iterate = iterate;
            _resultSelector = resultSelector;
            _scheduler = scheduler;
        }

        public Generate(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler)
        {
            _initialState = initialState;
            _condition = condition;
            _iterate = iterate;
            _resultSelector = resultSelector;
            _timeSelectorA = timeSelector;
            _scheduler = scheduler;
        }

        public Generate(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler)
        {
            _initialState = initialState;
            _condition = condition;
            _iterate = iterate;
            _resultSelector = resultSelector;
            _timeSelectorR = timeSelector;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TResult> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_timeSelectorA != null)
            {
                var sink = new α(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else if (_timeSelectorR != null)
            {
                var sink = new δ(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
            else
            {
                var sink = new _(this, observer, cancel);
                setSink(sink);
                return sink.Run();
            }
        }

        class α : Sink<TResult>
        {
            private readonly Generate<TState, TResult> _parent;

            public α(Generate<TState, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
                    base._observer.OnNext(_result);
                try
                {
                    if (_first)
                        _first = false;
                    else
                        state = _parent._iterate(state);
                    _hasResult = _parent._condition(state);
                    if (_hasResult)
                    {
                        _result = _parent._resultSelector(state);
                        time = _parent._timeSelectorA(state);
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

        class δ : Sink<TResult>
        {
            private readonly Generate<TState, TResult> _parent;

            public δ(Generate<TState, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
                    base._observer.OnNext(_result);
                try
                {
                    if (_first)
                        _first = false;
                    else
                        state = _parent._iterate(state);
                    _hasResult = _parent._condition(state);
                    if (_hasResult)
                    {
                        _result = _parent._resultSelector(state);
                        time = _parent._timeSelectorR(state);
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

        class _ : Sink<TResult>
        {
            private readonly Generate<TState, TResult> _parent;

            public _(Generate<TState, TResult> parent, IObserver<TResult> observer, IDisposable cancel)
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
                            _first = false;
                        else
                            _state = _parent._iterate(_state);
                        hasResult = _parent._condition(_state);
                        if (hasResult)
                            result = _parent._resultSelector(_state);
                    }
                    catch (Exception exception)
                    {
                        base._observer.OnError(exception);
                        base.Dispose();
                        return;
                    }

                    if (hasResult)
                        base._observer.OnNext(result);
                    else
                        break;
                }

                if (!cancel.IsDisposed)
                    base._observer.OnCompleted();

                base.Dispose();
            }

            private void LoopRec(Action recurse)
            {
                var hasResult = false;
                var result = default(TResult);
                try
                {
                    if (_first)
                        _first = false;
                    else
                        _state = _parent._iterate(_state);
                    hasResult = _parent._condition(_state);
                    if (hasResult)
                        result = _parent._resultSelector(_state);
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
}
#endif