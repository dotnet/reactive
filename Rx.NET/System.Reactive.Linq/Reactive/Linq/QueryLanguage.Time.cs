// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + Buffer +

        #region TimeSpan only

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan)
        {
            return Buffer_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return Buffer_<TSource>(source, timeSpan, timeSpan, scheduler);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            return Buffer_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            return Buffer_<TSource>(source, timeSpan, timeShift, scheduler);
        }

        private static IObservable<IList<TSource>> Buffer_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
#if !NO_PERF
            return new Buffer<TSource>(source, timeSpan, timeShift, scheduler);
#else
            return source.Window(timeSpan, timeShift, scheduler).SelectMany(Observable.ToList);
#endif
        }

        #endregion

        #region TimeSpan + int

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            return Buffer_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            return Buffer_<TSource>(source, timeSpan, count, scheduler);
        }

        private static IObservable<IList<TSource>> Buffer_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
#if !NO_PERF
            return new Buffer<TSource>(source, timeSpan, count, scheduler);
#else
            return source.Window(timeSpan, count, scheduler).SelectMany(Observable.ToList);
#endif
        }

        #endregion

        #endregion

        #region + Delay +

        #region TimeSpan

        public virtual IObservable<TSource> Delay<TSource>(IObservable<TSource> source, TimeSpan dueTime)
        {
            return Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Delay<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return Delay_<TSource>(source, dueTime, scheduler);
        }

        private static IObservable<TSource> Delay_<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new Delay<TSource>(source, dueTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var q = new Queue<Timestamped<Notification<TSource>>>();
                var active = false;
                var running = false;
                var cancelable = new SerialDisposable();
                var exception = default(Exception);

                var subscription = source.Materialize().Timestamp(scheduler).Subscribe(notification =>
                {
                    var shouldRun = false;

                    lock (gate)
                    {
                        if (notification.Value.Kind == NotificationKind.OnError)
                        {
                            q.Clear();
                            q.Enqueue(notification);
                            exception = notification.Value.Exception;
                            shouldRun = !running;
                        }
                        else
                        {
                            q.Enqueue(new Timestamped<Notification<TSource>>(notification.Value, notification.Timestamp.Add(dueTime)));
                            shouldRun = !active;
                            active = true;
                        }
                    }

                    if (shouldRun)
                    {
                        if (exception != null)
                            observer.OnError(exception);
                        else
                        {
                            var d = new SingleAssignmentDisposable();
                            cancelable.Disposable = d;
                            d.Disposable = scheduler.Schedule(dueTime, self =>
                            {
                                lock (gate)
                                {
                                    if (exception != null)
                                        return;
                                    running = true;
                                }
                                Notification<TSource> result;

                                do
                                {
                                    result = null;
                                    lock (gate)
                                    {
                                        if (q.Count > 0 && q.Peek().Timestamp.CompareTo(scheduler.Now) <= 0)
                                            result = q.Dequeue().Value;
                                    }

                                    if (result != null)
                                        result.Accept(observer);
                                } while (result != null);

                                var shouldRecurse = false;
                                var recurseDueTime = TimeSpan.Zero;
                                var e = default(Exception);
                                lock (gate)
                                {
                                    if (q.Count > 0)
                                    {
                                        shouldRecurse = true;
                                        recurseDueTime = TimeSpan.FromTicks(Math.Max(0, q.Peek().Timestamp.Subtract(scheduler.Now).Ticks));
                                    }
                                    else
                                        active = false;
                                    e = exception;
                                    running = false;
                                }
                                if (e != null)
                                    observer.OnError(e);
                                else if (shouldRecurse)
                                    self(recurseDueTime);
                            });
                        }
                    }
                });

                return new CompositeDisposable(subscription, cancelable);
            });
#endif
        }

        #endregion

        #region DateTimeOffset

        public virtual IObservable<TSource> Delay<TSource>(IObservable<TSource> source, DateTimeOffset dueTime)
        {
            return Delay_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Delay<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return Delay_<TSource>(source, dueTime, scheduler);
        }

        private static IObservable<TSource> Delay_<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new Delay<TSource>(source, dueTime, scheduler);
#else
            return Observable.Defer(() =>
            {
                var timeSpan = dueTime.Subtract(scheduler.Now);
                return Delay_<TSource>(source, timeSpan, scheduler);
            });
#endif
        }

        #endregion

        #region Duration selector

        public virtual IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            return Delay_<TSource, TDelay>(source, null, delayDurationSelector);
        }

        public virtual IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            return Delay_<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
        }

        private static IObservable<TSource> Delay_<TSource, TDelay>(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
#if !NO_PERF
            return new Delay<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var delays = new CompositeDisposable();

                var gate = new object();
                var atEnd = false;

                var done = new Action(() =>
                {
                    if (atEnd && delays.Count == 0)
                    {
                        observer.OnCompleted();
                    }
                });

                var subscription = new SerialDisposable();

                var start = new Action(() =>
                {
                    subscription.Disposable = source.Subscribe(
                        x =>
                        {
                            var delay = default(IObservable<TDelay>);
                            try
                            {
                                delay = delayDurationSelector(x);
                            }
                            catch (Exception error)
                            {
                                lock (gate)
                                    observer.OnError(error);

                                return;
                            }

                            var d = new SingleAssignmentDisposable();
                            delays.Add(d);
                            d.Disposable = delay.Subscribe(
                                _ =>
                                {
                                    lock (gate)
                                    {
                                        observer.OnNext(x);

                                        delays.Remove(d);
                                        done();
                                    }
                                },
                                exception =>
                                {
                                    lock (gate)
                                        observer.OnError(exception);
                                },
                                () =>
                                {
                                    lock (gate)
                                    {
                                        observer.OnNext(x);

                                        delays.Remove(d);
                                        done();
                                    }
                                }
                            );
                        },
                        exception =>
                        {
                            lock (gate)
                            {
                                observer.OnError(exception);
                            }
                        },
                        () =>
                        {
                            lock (gate)
                            {
                                atEnd = true;
                                subscription.Dispose();

                                done();
                            }
                        }
                    );
                });

                if (subscriptionDelay == null)
                {
                    start();
                }
                else
                {
                    subscription.Disposable = subscriptionDelay.Subscribe(
                        _ =>
                        {
                            start();
                        },
                        observer.OnError,
                        start
                    );
                }

                return new CompositeDisposable(subscription, delays);
            });
#endif
        }

        #endregion

        #endregion

        #region + DelaySubscription +

        public virtual IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, TimeSpan dueTime)
        {
            return DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return DelaySubscription_<TSource>(source, dueTime, scheduler);
        }

        private static IObservable<TSource> DelaySubscription_<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new DelaySubscription<TSource>(source, dueTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var d = new MultipleAssignmentDisposable();

                var dt = Normalize(dueTime);

                d.Disposable = scheduler.Schedule(dt, () =>
                {
                    d.Disposable = source.Subscribe(observer);
                });

                return d;
            });
#endif
        }

        public virtual IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, DateTimeOffset dueTime)
        {
            return DelaySubscription_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return DelaySubscription_<TSource>(source, dueTime, scheduler);
        }

        private static IObservable<TSource> DelaySubscription_<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new DelaySubscription<TSource>(source, dueTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var d = new MultipleAssignmentDisposable();

                d.Disposable = scheduler.Schedule(dueTime, () =>
                {
                    d.Disposable = source.Subscribe(observer);
                });

                return d;
            });
#endif
        }

        #endregion

        #region + Generate +

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector)
        {
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler)
        {
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
        }

        private static IObservable<TResult> Generate_<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler)
        {
#if !NO_PERF
            return new Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var state = initialState;
                var first = true;
                var hasResult = false;
                var result = default(TResult);
                var time = default(TimeSpan);
                return scheduler.Schedule(TimeSpan.Zero, self =>
                {
                    if (hasResult)
                        observer.OnNext(result);
                    try
                    {
                        if (first)
                            first = false;
                        else
                            state = iterate(state);
                        hasResult = condition(state);
                        if (hasResult)
                        {
                            result = resultSelector(state);
                            time = timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }

                    if (hasResult)
                        self(time);
                    else
                        observer.OnCompleted();
                });
            });
#endif
        }

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector)
        {
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler)
        {
            return Generate_<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
        }

        private static IObservable<TResult> Generate_<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler)
        {
#if !NO_PERF
            return new Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var state = initialState;
                var first = true;
                var hasResult = false;
                var result = default(TResult);
                var time = default(DateTimeOffset);
                return scheduler.Schedule(scheduler.Now, self =>
                {
                    if (hasResult)
                        observer.OnNext(result);
                    try
                    {
                        if (first)
                            first = false;
                        else
                            state = iterate(state);
                        hasResult = condition(state);
                        if (hasResult)
                        {
                            result = resultSelector(state);
                            time = timeSelector(state);
                        }
                    }
                    catch (Exception exception)
                    {
                        observer.OnError(exception);
                        return;
                    }

                    if (hasResult)
                        self(time);
                    else
                        observer.OnCompleted();
                });
            });
#endif
        }

        #endregion

        #region + Interval +

        public virtual IObservable<long> Interval(TimeSpan period)
        {
            return Timer_(period, period, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
        {
            return Timer_(period, period, scheduler);
        }

        #endregion

        #region + Sample +

        public virtual IObservable<TSource> Sample<TSource>(IObservable<TSource> source, TimeSpan interval)
        {
            return Sample_<TSource>(source, interval, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Sample<TSource>(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
        {
            return Sample_<TSource>(source, interval, scheduler);
        }

        private static IObservable<TSource> Sample_<TSource>(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
        {
#if !NO_PERF
            return new Sample<TSource>(source, interval, scheduler);
#else
            var sampler = Observable.Interval(interval, scheduler);
            return Sample_<TSource, long>(source, sampler);
#endif
        }

        public virtual IObservable<TSource> Sample<TSource, TSample>(IObservable<TSource> source, IObservable<TSample> sampler)
        {
            return Sample_<TSource, TSample>(source, sampler);
        }

        private static IObservable<TSource> Sample_<TSource, TSample>(IObservable<TSource> source, IObservable<TSample> sampler)
        {
#if !NO_PERF
            return new Sample<TSource, TSample>(source, sampler);
#else
            return Combine(source, sampler, (IObserver<TSource> observer, IDisposable leftSubscription, IDisposable rightSubscription) =>
            {
                var value = default(Notification<TSource>);
                var atEnd = false;
                return new BinaryObserver<TSource, TSample>(
                    newValue =>
                    {
                        switch (newValue.Kind)
                        {
                            case NotificationKind.OnNext:
                                value = newValue;
                                break;
                            case NotificationKind.OnError:
                                newValue.Accept(observer);
                                break;
                            case NotificationKind.OnCompleted:
                                atEnd = true;
                                break;
                        }
                    },
                    _ =>
                    {
                        var myValue = value;
                        value = null;
                        if (myValue != null)
                            myValue.Accept(observer);
                        if (atEnd)
                            observer.OnCompleted();
                    });
            });
#endif
        }

        #endregion

        #region + Skip +

        public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, TimeSpan duration)
        {
            return Skip_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            return Skip_<TSource>(source, duration, scheduler);
        }

        private static IObservable<TSource> Skip_<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
#if !NO_PERF
            var skip = source as Skip<TSource>;
            if (skip != null && skip._scheduler == scheduler)
                return skip.Ω(duration);

            return new Skip<TSource>(source, duration, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var open = false;

                var t = scheduler.Schedule(duration, () => open = true);

                var d = source.Subscribe(
                    x =>
                    {
                        if (open)
                            observer.OnNext(x);
                    },
                    observer.OnError,
                    observer.OnCompleted
                );

                return new CompositeDisposable(t, d);
            });
#endif
        }

        #endregion

        #region + SkipLast +

        public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, TimeSpan duration)
        {
            return SkipLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            return SkipLast_<TSource>(source, duration, scheduler);
        }

        private static IObservable<TSource> SkipLast_<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
#if !NO_PERF
            return new SkipLast<TSource>(source, duration, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var q = new Queue<System.Reactive.TimeInterval<TSource>>();

                var swp = scheduler.AsStopwatchProvider();
                var sw = swp != null ? swp.StartStopwatch() : new DefaultStopwatch();

                return source.Subscribe(
                    x =>
                    {
                        var now = sw.Elapsed;
                        q.Enqueue(new System.Reactive.TimeInterval<TSource>(x, now));
                        while (q.Count > 0 && now - q.Peek().Interval >= duration)
                            observer.OnNext(q.Dequeue().Value);
                    },
                    observer.OnError,
                    () =>
                    {
                        var now = sw.Elapsed;
                        while (q.Count > 0 && now - q.Peek().Interval >= duration)
                            observer.OnNext(q.Dequeue().Value);
                        observer.OnCompleted();
                    }
                );
            });
#endif
        }

        #endregion

        #region + SkipUntil +

        public virtual IObservable<TSource> SkipUntil<TSource>(IObservable<TSource> source, DateTimeOffset startTime)
        {
            return SkipUntil_<TSource>(source, startTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> SkipUntil<TSource>(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
        {
            return SkipUntil_<TSource>(source, startTime, scheduler);
        }

        private static IObservable<TSource> SkipUntil_<TSource>(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
        {
#if !NO_PERF
            var skipUntil = source as SkipUntil<TSource>;
            if (skipUntil != null && skipUntil._scheduler == scheduler)
                return skipUntil.Ω(startTime);

            return new SkipUntil<TSource>(source, startTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var open = false;

                var t = scheduler.Schedule(startTime, () => open = true);

                var d = source.Subscribe(
                    x =>
                    {
                        if (open)
                            observer.OnNext(x);
                    },
                    observer.OnError,
                    observer.OnCompleted
                );

                return new CompositeDisposable(t, d);
            });
#endif
        }

        #endregion

        #region + Take +

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, TimeSpan duration)
        {
            return Take_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            return Take_<TSource>(source, duration, scheduler);
        }

        private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
#if !NO_PERF
            var take = source as Take<TSource>;
            if (take != null && take._scheduler == scheduler)
                return take.Ω(duration);

            return new Take<TSource>(source, duration, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();

                var t = scheduler.Schedule(duration, () =>
                {
                    lock (gate)
                    {
                        observer.OnCompleted();
                    }
                });

                var d = source.Synchronize(gate).Subscribe(observer);

                return new CompositeDisposable(t, d);
            });
#endif
        }

        #endregion

        #region + TakeLast +

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration)
        {
            return TakeLast_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            return TakeLast_<TSource>(source, duration, scheduler, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler timerScheduler, IScheduler loopScheduler)
        {
            return TakeLast_<TSource>(source, duration, timerScheduler, loopScheduler);
        }

        private static IObservable<TSource> TakeLast_<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler timerScheduler, IScheduler loopScheduler)
        {
#if !NO_PERF
            return new TakeLast<TSource>(source, duration, timerScheduler, loopScheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var q = new Queue<System.Reactive.TimeInterval<TSource>>();

                var swp = timerScheduler.AsStopwatchProvider();
                var sw = swp != null ? swp.StartStopwatch() : new DefaultStopwatch();

                var trim = new Action<TimeSpan>(now =>
                {
                    while (q.Count > 0 && now - q.Peek().Interval >= duration)
                        q.Dequeue();
                });

                var g = new CompositeDisposable();

                g.Add(source.Subscribe(
                    x =>
                    {
                        var now = sw.Elapsed;

                        q.Enqueue(new System.Reactive.TimeInterval<TSource>(x, now));
                        trim(now);
                    },
                    observer.OnError,
                    () =>
                    {
                        var now = sw.Elapsed;
                        trim(now);

                        g.Add(loopScheduler.Schedule(rec =>
                        {
                            if (q.Count > 0)
                            {
                                observer.OnNext(q.Dequeue().Value);
                                rec();
                            }
                            else
                            {
                                observer.OnCompleted();
                            }
                        }));
                    }
                ));

                return g;
            });
#endif
        }

        public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, TimeSpan duration)
        {
            return TakeLastBuffer_<TSource>(source, duration, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            return TakeLastBuffer_<TSource>(source, duration, scheduler);
        }

        private static IObservable<IList<TSource>> TakeLastBuffer_<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
#if !NO_PERF
            return new TakeLastBuffer<TSource>(source, duration, scheduler);
#else
            return new AnonymousObservable<IList<TSource>>(observer =>
            {
                var q = new Queue<System.Reactive.TimeInterval<TSource>>();

                var swp = scheduler.AsStopwatchProvider();
                var sw = swp != null ? swp.StartStopwatch() : new DefaultStopwatch();

                return source.Subscribe(
                    x =>
                    {
                        var now = sw.Elapsed;

                        q.Enqueue(new System.Reactive.TimeInterval<TSource>(x, now));
                        while (q.Count > 0 && now - q.Peek().Interval >= duration)
                            q.Dequeue();
                    },
                    observer.OnError,
                    () =>
                    {
                        var now = sw.Elapsed;

                        var res = new List<TSource>();
                        while (q.Count > 0)
                        {
                            var next = q.Dequeue();
                            if (now - next.Interval <= duration)
                                res.Add(next.Value);
                        }
                        
                        observer.OnNext(res);
                        observer.OnCompleted();
                    }
                );
            });
#endif
        }

        #endregion

        #region + TakeUntil +

        public virtual IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, DateTimeOffset endTime)
        {
            return TakeUntil_<TSource>(source, endTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
        {
            return TakeUntil_<TSource>(source, endTime, scheduler);
        }

        private static IObservable<TSource> TakeUntil_<TSource>(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
        {
#if !NO_PERF
            var takeUntil = source as TakeUntil<TSource>;
            if (takeUntil != null && takeUntil._scheduler == scheduler)
                return takeUntil.Ω(endTime);

            return new TakeUntil<TSource>(source, endTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();

                var t = scheduler.Schedule(endTime, () =>
                {
                    lock (gate)
                    {
                        observer.OnCompleted();
                    }
                });

                var d = source.Synchronize(gate).Subscribe(observer);

                return new CompositeDisposable(t, d);
            });
#endif
        }

        #endregion

        #region + Throttle +

        public virtual IObservable<TSource> Throttle<TSource>(IObservable<TSource> source, TimeSpan dueTime)
        {
            return Throttle_<TSource>(source, dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Throttle<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return Throttle_<TSource>(source, dueTime, scheduler);
        }

        private static IObservable<TSource> Throttle_<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new Throttle<TSource>(source, dueTime, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var value = default(TSource);
                var hasValue = false;
                var cancelable = new SerialDisposable();
                var id = 0UL;

                var subscription = source.Subscribe(x =>
                    {
                        ulong currentid;
                        lock (gate)
                        {
                            hasValue = true;
                            value = x;
                            id = unchecked(id + 1);
                            currentid = id;
                        }
                        var d = new SingleAssignmentDisposable();
                        cancelable.Disposable = d;
                        d.Disposable = scheduler.Schedule(dueTime, () =>
                            {
                                lock (gate)
                                {
                                    if (hasValue && id == currentid)
                                        observer.OnNext(value);
                                    hasValue = false;
                                }
                            });
                    },
                    exception =>
                    {
                        cancelable.Dispose();

                        lock (gate)
                        {
                            observer.OnError(exception);
                            hasValue = false;
                            id = unchecked(id + 1);
                        }                        
                    },
                    () =>
                    {
                        cancelable.Dispose();

                        lock (gate)
                        {
                            if (hasValue)
                                observer.OnNext(value);
                            observer.OnCompleted();
                            hasValue = false;
                            id = unchecked(id + 1);
                        }
                    });

                return new CompositeDisposable(subscription, cancelable);
            });
#endif
        }

        public virtual IObservable<TSource> Throttle<TSource, TThrottle>(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
        {
#if !NO_PERF
            return new Throttle<TSource, TThrottle>(source, throttleDurationSelector);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var value = default(TSource);
                var hasValue = false;
                var cancelable = new SerialDisposable();
                var id = 0UL;

                var subscription = source.Subscribe(
                    x =>
                    {
                        var throttle = default(IObservable<TThrottle>);
                        try
                        {
                            throttle = throttleDurationSelector(x);
                        }
                        catch (Exception error)
                        {
                            lock (gate)
                                observer.OnError(error);

                            return;
                        }

                        ulong currentid;
                        lock (gate)
                        {
                            hasValue = true;
                            value = x;
                            id = unchecked(id + 1);
                            currentid = id;
                        }

                        var d = new SingleAssignmentDisposable();
                        cancelable.Disposable = d;
                        d.Disposable = throttle.Subscribe(
                            _ =>
                            {
                                lock (gate)
                                {
                                    if (hasValue && id == currentid)
                                        observer.OnNext(value);

                                    hasValue = false;
                                    d.Dispose();
                                }
                            },
                            exception =>
                            {
                                lock (gate)
                                {
                                    observer.OnError(exception);
                                }
                            },
                            () =>
                            {
                                lock (gate)
                                {
                                    if (hasValue && id == currentid)
                                        observer.OnNext(value);

                                    hasValue = false;
                                    d.Dispose();
                                }
                            }
                        );
                    },
                    exception =>
                    {
                        cancelable.Dispose();

                        lock (gate)
                        {
                            observer.OnError(exception);
                            hasValue = false;
                            id = unchecked(id + 1);
                        }
                    },
                    () =>
                    {
                        cancelable.Dispose();

                        lock (gate)
                        {
                            if (hasValue)
                                observer.OnNext(value);
                            observer.OnCompleted();
                            hasValue = false;
                            id = unchecked(id + 1);
                        }
                    });

                return new CompositeDisposable(subscription, cancelable);
            });
#endif
        }

        #endregion

        #region + TimeInterval +

        public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(IObservable<TSource> source)
        {
            return TimeInterval_<TSource>(source, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return TimeInterval_<TSource>(source, scheduler);
        }

#if !NO_PERF
        private static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval_<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return new TimeInterval<TSource>(source, scheduler);
        }
#else
        private IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval_<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Defer(() =>
            {
                var last = scheduler.Now;
                return source.Select(x =>
                {
                    var now = scheduler.Now;
                    var span = now.Subtract(last);
                    last = now;
                    return new System.Reactive.TimeInterval<TSource>(x, span);
                });
            });
        }
#endif

        #endregion

        #region + Timeout +

        #region TimeSpan

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime)
        {
            return Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>(new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>(new TimeoutException()), scheduler);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other)
        {
            return Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            return Timeout_<TSource>(source, dueTime, other, scheduler);
        }

        private static IObservable<TSource> Timeout_<TSource>(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timeout<TSource>(source, dueTime, other, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SerialDisposable();
                var timer = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                subscription.Disposable = original;

                var gate = new object();
                var id = 0UL;
                var switched = false;

                Action createTimer = () =>
                {
                    var myid = id;
                    timer.Disposable = scheduler.Schedule(dueTime, () =>
                    {
                        var timerWins = false;

                        lock (gate)
                        {
                            switched = (id == myid);
                            timerWins = switched;
                        }

                        if (timerWins)
                            subscription.Disposable = other.Subscribe(observer);
                    });
                };

                createTimer();

                original.Disposable = source.Subscribe(
                    x =>
                    {
                        var onNextWins = false;

                        lock (gate)
                        {
                            onNextWins = !switched;
                            if (onNextWins)
                            {
                                id = unchecked(id + 1);
                            }
                        }

                        if (onNextWins)
                        {
                            observer.OnNext(x);
                            createTimer();
                        }
                    },
                    exception =>
                    {
                        var onErrorWins = false;

                        lock (gate)
                        {
                            onErrorWins = !switched;
                            if (onErrorWins)
                            {
                                id = unchecked(id + 1);
                            }
                        }

                        if (onErrorWins)
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        var onCompletedWins = false;

                        lock (gate)
                        {
                            onCompletedWins = !switched;
                            if (onCompletedWins)
                            {
                                id = unchecked(id + 1);
                            }
                        }

                        if (onCompletedWins)
                            observer.OnCompleted();
                    });

                return new CompositeDisposable(subscription, timer);
            });
#endif
        }

        #endregion

        #region DateTimeOffset

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime)
        {
            return Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>(new TimeoutException()), SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return Timeout_<TSource>(source, dueTime, Observable.Throw<TSource>(new TimeoutException()), scheduler);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other)
        {
            return Timeout_<TSource>(source, dueTime, other, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            return Timeout_<TSource>(source, dueTime, other, scheduler);
        }

        private static IObservable<TSource> Timeout_<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timeout<TSource>(source, dueTime, other, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                subscription.Disposable = original;

                var gate = new object();
                var switched = false;

                var timer = scheduler.Schedule(dueTime, () =>
                {
                    var timerWins = false;

                    lock (gate)
                    {
                        timerWins = !switched;
                        switched = true;
                    }

                    if (timerWins)
                        subscription.Disposable = other.Subscribe(observer);
                });

                original.Disposable = source.Subscribe(
                    x =>
                    {
                        lock (gate)
                        {
                            if (!switched)
                                observer.OnNext(x);
                        }
                    },
                    exception =>
                    {
                        var onErrorWins = false;

                        lock (gate)
                        {
                            onErrorWins = !switched;
                            switched = true;
                        }

                        if (onErrorWins)
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        var onCompletedWins = false;

                        lock (gate)
                        {
                            onCompletedWins = !switched;
                            switched = true;
                        }

                        if (onCompletedWins)
                            observer.OnCompleted();
                    });

                return new CompositeDisposable(subscription, timer);
            });
#endif
        }

        #endregion

        #region Duration selector

        public virtual IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
        {
            return Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, Observable.Throw<TSource>(new TimeoutException()));
        }

        public virtual IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other)
        {
            return Timeout_<TSource, TTimeout>(source, Observable.Never<TTimeout>(), timeoutDurationSelector, other);
        }

        public virtual IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
        {
            return Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, Observable.Throw<TSource>(new TimeoutException()));
        }

        public virtual IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other)
        {
            return Timeout_<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
        }

        private static IObservable<TSource> Timeout_<TSource, TTimeout>(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other)
        {
#if !NO_PERF
            return new Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SerialDisposable();
                var timer = new SerialDisposable();
                var original = new SingleAssignmentDisposable();

                subscription.Disposable = original;

                var gate = new object();
                var id = 0UL;
                var switched = false;

                Action<IObservable<TTimeout>> setTimer = timeout =>
                {
                    var myid = id;

                    Func<bool> timerWins = () =>
                    {
                        var res = false;

                        lock (gate)
                        {
                            switched = (id == myid);
                            res = switched;
                        }

                        return res;
                    };

                    var d = new SingleAssignmentDisposable();
                    timer.Disposable = d;
                    d.Disposable = timeout.Subscribe(
                        _ =>
                        {
                            if (timerWins())
                                subscription.Disposable = other.Subscribe(observer);

                            d.Dispose();
                        },
                        error =>
                        {
                            if (timerWins())
                                observer.OnError(error);
                        },
                        () =>
                        {
                            if (timerWins())
                                subscription.Disposable = other.Subscribe(observer);
                        }
                    );
                };

                setTimer(firstTimeout);

                Func<bool> observerWins = () =>
                {
                    var res = false;

                    lock (gate)
                    {
                        res = !switched;
                        if (res)
                        {
                            id = unchecked(id + 1);
                        }
                    }

                    return res;
                };

                original.Disposable = source.Subscribe(
                    x =>
                    {
                        if (observerWins())
                        {
                            observer.OnNext(x);

                            var timeout = default(IObservable<TTimeout>);
                            try
                            {
                                timeout = timeoutDurationSelector(x);
                            }
                            catch (Exception error)
                            {
                                observer.OnError(error);
                                return;
                            }

                            setTimer(timeout);
                        }
                    },
                    exception =>
                    {
                        if (observerWins())
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        if (observerWins())
                            observer.OnCompleted();
                    }
                );

                return new CompositeDisposable(subscription, timer);
            });
#endif
        }

        #endregion

        #endregion

        #region + Timer +

        public virtual IObservable<long> Timer(TimeSpan dueTime)
        {
            return Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<long> Timer(DateTimeOffset dueTime)
        {
            return Timer_(dueTime, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
        {
            return Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
        {
            return Timer_(dueTime, period, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
        {
            return Timer_(dueTime, scheduler);
        }

        public virtual IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
        {
            return Timer_(dueTime, scheduler);
        }

        public virtual IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            return Timer_(dueTime, period, scheduler);
        }

        public virtual IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
        {
            return Timer_(dueTime, period, scheduler);
        }

        private static IObservable<long> Timer_(TimeSpan dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timer(dueTime, null, scheduler);
#else
            var d = Normalize(dueTime);

            return new AnonymousObservable<long>(observer =>
                scheduler.Schedule(d, () =>
                {
                    observer.OnNext(0);
                    observer.OnCompleted();
                }));
#endif
        }

#if !NO_PERF
        private static IObservable<long> Timer_(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            return new Timer(dueTime, period, scheduler);
        }
#else
        private IObservable<long> Timer_(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            var p = Normalize(period);

            return Defer(() => Timer(scheduler.Now + dueTime, p, scheduler));
        }
#endif

        private static IObservable<long> Timer_(DateTimeOffset dueTime, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timer(dueTime, null, scheduler);
#else
            return new AnonymousObservable<long>(observer =>
                scheduler.Schedule(dueTime, () =>
                {
                    observer.OnNext(0);
                    observer.OnCompleted();
                }));
#endif
        }

        private static IObservable<long> Timer_(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timer(dueTime, period, scheduler);
#else
            var p = Normalize(period);

            return new AnonymousObservable<long>(observer =>
            {
                var d = dueTime;
                var count = 0L;

                return scheduler.Schedule(d, self =>
                {
                    if (p > TimeSpan.Zero)
                    {
                        var now = scheduler.Now;
                        d = d + p;
                        if (d <= now)
                            d = now + p;
                    }

                    observer.OnNext(count);
                    count = unchecked(count + 1);
                    self(d);
                });
            });
#endif
        }

        #endregion

        #region + Timestamp +

        public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source)
        {
            return Timestamp_<TSource>(source, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Timestamp_<TSource>(source, scheduler);
        }

        private static IObservable<Timestamped<TSource>> Timestamp_<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
#if !NO_PERF
            return new Timestamp<TSource>(source, scheduler);
#else
            return source.Select(x => new Timestamped<TSource>(x, scheduler.Now));
#endif
        }

        #endregion

        #region + Window +

        #region TimeSpan only

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan)
        {
            return Window_<TSource>(source, timeSpan, timeSpan, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return Window_<TSource>(source, timeSpan, timeSpan, scheduler);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            return Window_<TSource>(source, timeSpan, timeShift, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            return Window_<TSource>(source, timeSpan, timeShift, scheduler);
        }

        private static IObservable<IObservable<TSource>> Window_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
#if !NO_PERF
            return new Window<TSource>(source, timeSpan, timeShift, scheduler);
#else
            return new AnonymousObservable<IObservable<TSource>>(observer =>
            {
                var totalTime = TimeSpan.Zero;
                var nextShift = timeShift;
                var nextSpan = timeSpan;

                var gate = new object();
                var q = new Queue<ISubject<TSource>>();

                var timerD = new SerialDisposable();
                var groupDisposable = new CompositeDisposable(2) { timerD };
                var refCountDisposable = new RefCountDisposable(groupDisposable);

                var createTimer = default(Action);
                createTimer = () =>
                {
                    var m = new SingleAssignmentDisposable();
                    timerD.Disposable = m;

                    var isSpan = false;
                    var isShift = false;
                    if (nextSpan == nextShift)
                    {
                        isSpan = true;
                        isShift = true;
                    }
                    else if (nextSpan < nextShift)
                        isSpan = true;
                    else
                        isShift = true;

                    var newTotalTime = isSpan ? nextSpan : nextShift;
                    var ts = newTotalTime - totalTime;
                    totalTime = newTotalTime;

                    if (isSpan)
                        nextSpan += timeShift;
                    if (isShift)
                        nextShift += timeShift;

                    m.Disposable = scheduler.Schedule(ts, () =>
                    {
                        lock (gate)
                        {
                            if (isShift)
                            {
                                var s = new Subject<TSource>();
                                q.Enqueue(s);
                                observer.OnNext(s.AddRef(refCountDisposable));
                            }
                            if (isSpan)
                            {
                                var s = q.Dequeue();
                                s.OnCompleted();
                            }
                        }

                        createTimer();
                    });
                };

                q.Enqueue(new Subject<TSource>());
                observer.OnNext(q.Peek().AddRef(refCountDisposable));
                createTimer();

                groupDisposable.Add(source.Subscribe(
                    x =>
                    {
                        lock (gate)
                        {
                            foreach (var s in q)
                                s.OnNext(x);
                        }
                    },
                    exception =>
                    {
                        lock (gate)
                        {
                            foreach (var s in q)
                                s.OnError(exception);
                            observer.OnError(exception);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            foreach (var s in q)
                                s.OnCompleted();
                            observer.OnCompleted();
                        }
                    }
                ));

                return refCountDisposable;
            });
#endif
        }

        #endregion

        #region TimeSpan + int

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            return Window_<TSource>(source, timeSpan, count, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            return Window_<TSource>(source, timeSpan, count, scheduler);
        }

        private static IObservable<IObservable<TSource>> Window_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
#if !NO_PERF
            return new Window<TSource>(source, timeSpan, count, scheduler);
#else
            return new AnonymousObservable<IObservable<TSource>>(observer =>
            {
                var gate = new object();
                var s = default(ISubject<TSource>);
                var n = 0;
                var windowId = 0;

                var timerD = new SerialDisposable();
                var groupDisposable = new CompositeDisposable(2) { timerD };
                var refCountDisposable = new RefCountDisposable(groupDisposable);

                var createTimer = default(Action<int>);
                createTimer = id =>
                {
                    var m = new SingleAssignmentDisposable();
                    timerD.Disposable = m;

                    m.Disposable = scheduler.Schedule(timeSpan, () =>
                    {
                        var newId = 0;
                        lock (gate)
                        {
                            if (id != windowId)
                                return;
                            n = 0;
                            newId = ++windowId;

                            s.OnCompleted();
                            s = new Subject<TSource>();
                            observer.OnNext(s.AddRef(refCountDisposable));
                        }

                        createTimer(newId);
                    });
                };

                s = new Subject<TSource>();
                observer.OnNext(s.AddRef(refCountDisposable));
                createTimer(0);

                groupDisposable.Add(source.Subscribe(
                    x =>
                    {
                        var newWindow = false;
                        var newId = 0;

                        lock (gate)
                        {
                            s.OnNext(x);

                            n++;
                            if (n == count)
                            {
                                newWindow = true;
                                n = 0;
                                newId = ++windowId;

                                s.OnCompleted();
                                s = new Subject<TSource>();
                                observer.OnNext(s.AddRef(refCountDisposable));
                            }
                        }

                        if (newWindow)
                            createTimer(newId);
                    },
                    exception =>
                    {
                        lock (gate)
                        {
                            s.OnError(exception);
                            observer.OnError(exception);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            s.OnCompleted();
                            observer.OnCompleted();
                        }
                    }
                ));

                return refCountDisposable;
            });
#endif
        }

        #endregion

        #endregion

        #region |> Helpers <|

#if NO_PERF
        private static TimeSpan Normalize(TimeSpan timeSpan)
        {
            if (timeSpan.CompareTo(TimeSpan.Zero) < 0)
                return TimeSpan.Zero;
            return timeSpan;
        }
#endif

        #endregion
    }
}
