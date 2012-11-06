// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq.Observαble
{
    class ToObservable<TSource> : Producer<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly IScheduler _scheduler;

        public ToObservable(IEnumerable<TSource> source, IScheduler scheduler)
        {
            _source = source;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<TSource> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            var sink = new _(this, observer, cancel);
            setSink(sink);
            return sink.Run();
        }

        class _ : Sink<TSource>
        {
            private readonly ToObservable<TSource> _parent;

            public _(ToObservable<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                var e = default(IEnumerator<TSource>);
                try
                {
                    e = _parent._source.GetEnumerator();
                }
                catch (Exception exception)
                {
                    base._observer.OnError(exception);
                    base.Dispose();
                    return Disposable.Empty;
                }

                var longRunning = _parent._scheduler.AsLongRunning();
                if (longRunning != null)
                {
                    //
                    // Long-running schedulers have the contract they should *never* prevent
                    // the work from starting, such that the scheduled work has the chance
                    // to observe the cancellation and perform proper clean-up. In this case,
                    // we're sure Loop will be entered, allowing us to dispose the enumerator.
                    //
                    return longRunning.ScheduleLongRunning(e, Loop);
                }
                else
                {
                    //
                    // We never allow the scheduled work to be cancelled. Instead, the flag
                    // is used to have LoopRec bail out and perform proper clean-up of the
                    // enumerator.
                    //
                    var flag = new BooleanDisposable();
                    _parent._scheduler.Schedule(new State(flag, e), LoopRec);
                    return flag;
                }
            }

            class State
            {
                public readonly ICancelable flag;
                public readonly IEnumerator<TSource> enumerator;

                public State(ICancelable flag, IEnumerator<TSource> enumerator)
                {
                    this.flag = flag;
                    this.enumerator = enumerator;
                }
            }

            private void LoopRec(State state, Action<State> recurse)
            {
                var hasNext = false;
                var ex = default(Exception);
                var current = default(TSource);

                if (state.flag.IsDisposed)
                {
                    state.enumerator.Dispose();
                    return;
                }

                try
                {
                    hasNext = state.enumerator.MoveNext();
                    if (hasNext)
                        current = state.enumerator.Current;
                }
                catch (Exception exception)
                {
                    ex = exception;
                }

                if (ex != null)
                {
                    state.enumerator.Dispose();

                    base._observer.OnError(ex);
                    base.Dispose();
                    return;
                }

                if (!hasNext)
                {
                    state.enumerator.Dispose();

                    base._observer.OnCompleted();
                    base.Dispose();
                    return;
                }

                base._observer.OnNext(current);
                recurse(state);
            }

            private void Loop(IEnumerator<TSource> enumerator, ICancelable cancel)
            {
                while (!cancel.IsDisposed)
                {
                    var hasNext = false;
                    var ex = default(Exception);
                    var current = default(TSource);

                    try
                    {
                        hasNext = enumerator.MoveNext();
                        if (hasNext)
                            current = enumerator.Current;
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }

                    if (ex != null)
                    {
                        base._observer.OnError(ex);
                        break;
                    }

                    if (!hasNext)
                    {
                        base._observer.OnCompleted();
                        break;
                    }

                    base._observer.OnNext(current);
                }

                enumerator.Dispose();
                base.Dispose();
            }
        }
    }
}
#endif