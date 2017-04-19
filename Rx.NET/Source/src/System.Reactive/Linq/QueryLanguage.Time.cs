// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Buffer +

        #region TimeSpan only

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan)
        {
            return Buffer_<TSource>(source, timeSpan, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return Buffer_<TSource>(source, timeSpan, scheduler);
        }

        private static IObservable<IList<TSource>> Buffer_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return new Buffer<TSource>.TimeHopping(source, timeSpan, scheduler);
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
            return new Buffer<TSource>.TimeSliding(source, timeSpan, timeShift, scheduler);
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
            return new Buffer<TSource>.Ferry(source, timeSpan, count, scheduler);
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
            return new Delay<TSource>.Relative(source, dueTime, scheduler);
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
            return new Delay<TSource>.Absolute(source, dueTime, scheduler);
        }

        #endregion

        #region Duration selector

        public virtual IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            return new Delay<TSource, TDelay>.Selector(source, delayDurationSelector);
        }

        public virtual IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            return new Delay<TSource, TDelay>.SelectorWithSubscriptionDelay(source, subscriptionDelay, delayDurationSelector);
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
            return new DelaySubscription<TSource>.Relative(source, dueTime, scheduler);
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
            return new DelaySubscription<TSource>.Absolute(source, dueTime, scheduler);
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
            return new Generate<TState, TResult>.Relative(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
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
            return new Generate<TState, TResult>.Absolute(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
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
            return new Sample<TSource>(source, interval, scheduler);
        }

        public virtual IObservable<TSource> Sample<TSource, TSample>(IObservable<TSource> source, IObservable<TSample> sampler)
        {
            return Sample_<TSource, TSample>(source, sampler);
        }

        private static IObservable<TSource> Sample_<TSource, TSample>(IObservable<TSource> source, IObservable<TSample> sampler)
        {
            return new Sample<TSource, TSample>(source, sampler);
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
            if (source is Skip<TSource>.Time skip && skip._scheduler == scheduler)
                return skip.Combine(duration);

            return new Skip<TSource>.Time(source, duration, scheduler);
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
            return new SkipLast<TSource>.Time(source, duration, scheduler);
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
            if (source is SkipUntil<TSource> skipUntil && skipUntil._scheduler == scheduler)
                return skipUntil.Combine(startTime);

            return new SkipUntil<TSource>(source, startTime, scheduler);
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
            if (source is Take<TSource>.Time take && take._scheduler == scheduler)
                return take.Combine(duration);

            return new Take<TSource>.Time(source, duration, scheduler);
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
            return new TakeLast<TSource>.Time(source, duration, timerScheduler, loopScheduler);
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
            return new TakeLastBuffer<TSource>.Time(source, duration, scheduler);
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
            if (source is TakeUntil<TSource> takeUntil && takeUntil._scheduler == scheduler)
                return takeUntil.Combine(endTime);

            return new TakeUntil<TSource>(source, endTime, scheduler);
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
            return new Throttle<TSource>(source, dueTime, scheduler);
        }

        public virtual IObservable<TSource> Throttle<TSource, TThrottle>(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
        {
            return new Throttle<TSource, TThrottle>(source, throttleDurationSelector);
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

        private static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval_<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return new TimeInterval<TSource>(source, scheduler);
        }

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
            return new Timeout<TSource>.Relative(source, dueTime, other, scheduler);
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
            return new Timeout<TSource>.Absolute(source, dueTime, other, scheduler);
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
            return new Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
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
            return new Timer.Single.Relative(dueTime, scheduler);
        }

        private static IObservable<long> Timer_(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            return new Timer.Periodic.Relative(dueTime, period, scheduler);
        }

        private static IObservable<long> Timer_(DateTimeOffset dueTime, IScheduler scheduler)
        {
            return new Timer.Single.Absolute(dueTime, scheduler);
        }

        private static IObservable<long> Timer_(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
        {
            return new Timer.Periodic.Absolute(dueTime, period, scheduler);
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
            return new Timestamp<TSource>(source, scheduler);
        }

        #endregion

        #region + Window +

        #region TimeSpan only

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan)
        {
            return Window_<TSource>(source, timeSpan, SchedulerDefaults.TimeBasedOperations);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return Window_<TSource>(source, timeSpan, scheduler);
        }

        private static IObservable<IObservable<TSource>> Window_<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            return new Window<TSource>.TimeHopping(source, timeSpan, scheduler);
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
            return new Window<TSource>.TimeSliding(source, timeSpan, timeShift, scheduler);
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
            return new Window<TSource>.Ferry(source, timeSpan, count, scheduler);
        }

        #endregion

        #endregion
    }
}
