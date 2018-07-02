// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class Timer
    {
        internal abstract class Single : Producer<long, Single._>
        {
            private readonly IScheduler _scheduler;

            protected Single(IScheduler scheduler)
            {
                _scheduler = scheduler;
            }

            internal sealed class Relative : Single
            {
                private readonly TimeSpan _dueTime;

                public Relative(TimeSpan dueTime, IScheduler scheduler)
                    : base(scheduler)
                {
                    _dueTime = dueTime;
                }

                protected override _ CreateSink(IObserver<long> observer) => new _(observer);

                protected override void Run(_ sink) => sink.Run(this, _dueTime);
            }

            internal sealed class Absolute : Single
            {
                private readonly DateTimeOffset _dueTime;

                public Absolute(DateTimeOffset dueTime, IScheduler scheduler)
                    : base(scheduler)
                {
                    _dueTime = dueTime;
                }

                protected override _ CreateSink(IObserver<long> observer) => new _(observer);

                protected override void Run(_ sink) => sink.Run(this, _dueTime);
            }

            internal sealed class _ : IdentitySink<long>
            {
                public _(IObserver<long> observer)
                    : base(observer)
                {
                }

                public void Run(Single parent, DateTimeOffset dueTime)
                {
                    SetUpstream(parent._scheduler.ScheduleAction(this, dueTime, state => state.Invoke()));
                }

                public void Run(Single parent, TimeSpan dueTime)
                {
                    SetUpstream(parent._scheduler.ScheduleAction(this, dueTime, state => state.Invoke()));
                }

                private void Invoke()
                {
                    ForwardOnNext(0);
                    ForwardOnCompleted();
                }
            }
        }

        internal abstract class Periodic : Producer<long, Periodic._>
        {
            private readonly TimeSpan _period;
            private readonly IScheduler _scheduler;

            protected Periodic(TimeSpan period, IScheduler scheduler)
            {
                _period = period;
                _scheduler = scheduler;
            }

            internal sealed class Relative : Periodic
            {
                private readonly TimeSpan _dueTime;

                public Relative(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
                    : base(period, scheduler)
                {
                    _dueTime = dueTime;
                }

                protected override _ CreateSink(IObserver<long> observer) => new _(_period, observer);

                protected override void Run(_ sink) => sink.Run(this, _dueTime);
            }

            internal sealed class Absolute : Periodic
            {
                private readonly DateTimeOffset _dueTime;

                public Absolute(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
                    : base(period, scheduler)
                {
                    _dueTime = dueTime;
                }

                protected override _ CreateSink(IObserver<long> observer) => new _(_period, observer);

                protected override void Run(_ sink) => sink.Run(this, _dueTime);
            }

            internal sealed class _ : IdentitySink<long>
            {
                private readonly TimeSpan _period;
                private long _index;

                public _(TimeSpan period, IObserver<long> observer)
                    : base(observer)
                {
                    _period = period;
                }

                public void Run(Periodic parent, DateTimeOffset dueTime)
                {
                    SetUpstream(parent._scheduler.Schedule(this, dueTime, (innerScheduler, @this) => @this.InvokeStart(innerScheduler)));
                }

                public void Run(Periodic parent, TimeSpan dueTime)
                {
                    //
                    // Optimize for the case of Observable.Interval.
                    //
                    if (dueTime == _period)
                    {
                        SetUpstream(parent._scheduler.SchedulePeriodic(this, _period, @this => @this.Tick()));
                    }
                    else
                    {
                        SetUpstream(parent._scheduler.Schedule(this, dueTime, (innerScheduler, @this) => @this.InvokeStart(innerScheduler)));
                    }
                }

                //
                // BREAKING CHANGE v2 > v1.x - No more correction for time drift based on absolute time. This
                //                             didn't work for large period values anyway; the fractional
                //                             error exceeded corrections. Also complicated dealing with system
                //                             clock change conditions and caused numerous bugs.
                //
                // - For more precise scheduling, use a custom scheduler that measures TimeSpan values in a
                //   better way, e.g. spinning to make up for the last part of the period. Whether or not the
                //   values of the TimeSpan period match NT time or wall clock time is up to the scheduler.
                //
                // - For more accurate scheduling wrt the system clock, use Generate with DateTimeOffset time
                //   selectors. When the system clock changes, intervals will not be the same as diffs between
                //   consecutive absolute time values. The precision will be low (1s range by default).
                //
                private void Tick()
                {
                    var count = _index;
                    _index = unchecked(count + 1);

                    ForwardOnNext(count);
                }

                private int _pendingTickCount;
                private IDisposable _periodic;

                private IDisposable InvokeStart(IScheduler self)
                {
                    //
                    // Notice the first call to OnNext will introduce skew if it takes significantly long when
                    // using the following naive implementation:
                    //
                    //    Code:  base._observer.OnNext(0L);
                    //           return self.SchedulePeriodicEmulated(1L, _period, (Func<long, long>)Tick);
                    //
                    // What we're saying here is that Observable.Timer(dueTime, period) is pretty much the same
                    // as writing Observable.Timer(dueTime).Concat(Observable.Interval(period)).
                    //
                    //    Expected:  dueTime
                    //                  |
                    //                  0--period--1--period--2--period--3--period--4--...
                    //                  |
                    //                  +-OnNext(0L)-|
                    //    
                    //    Actual:    dueTime
                    //                  |
                    //                  0------------#--period--1--period--2--period--3--period--4--...
                    //                  |
                    //                  +-OnNext(0L)-|
                    //
                    // Different solutions for this behavior have different problems:
                    //
                    // 1. Scheduling the periodic job first and using an AsyncLock to serialize the OnNext calls
                    //    has the drawback that InvokeStart may never return. This happens when every callback
                    //    doesn't meet the period's deadline, hence the periodic job keeps queueing stuff up. In
                    //    this case, InvokeStart stays the owner of the AsyncLock and the call to Wait will never
                    //    return, thus not allowing any interleaving of work on this scheduler's logical thread.
                    //
                    // 2. Scheduling the periodic job first and using a (blocking) synchronization primitive to
                    //    signal completion of the OnNext(0L) call to the Tick call requires quite a bit of state
                    //    and careful handling of the case when OnNext(0L) throws. What's worse is the blocking
                    //    behavior inside Tick.
                    //
                    // In order to avoid blocking behavior, we need a scheme much like SchedulePeriodic emulation
                    // where work to dispatch OnNext(n + 1) is delegated to a catch up loop in case OnNext(n) was
                    // still running. Because SchedulePeriodic emulation exhibits such behavior in all cases, we
                    // only need to deal with the overlap of OnNext(0L) with future periodic OnNext(n) dispatch
                    // jobs. In the worst case where every callback takes longer than the deadline implied by the
                    // period, the periodic job will just queue up work that's dispatched by the tail-recursive
                    // catch up loop. In the best case, all work will be dispatched on the periodic scheduler.
                    //

                    //
                    // We start with one tick pending because we're about to start doing OnNext(0L).
                    //
                    _pendingTickCount = 1;

                    var d = new SingleAssignmentDisposable();
                    _periodic = d;
                    _index = 1;
                    d.Disposable = self.SchedulePeriodic(this, _period, @this => @this.Tock());

                    try
                    {
                        ForwardOnNext(0L);
                    }
                    catch (Exception e)
                    {
                        d.Dispose();
                        e.Throw();
                    }

                    //
                    // If the periodic scheduling job already ran before we finished dispatching the OnNext(0L)
                    // call, we'll find pendingTickCount to be > 1. In this case, we need to catch up by dispatching
                    // subsequent calls to OnNext as fast as possible, but without running a loop in order to ensure
                    // fair play with the scheduler. So, we run a tail-recursive loop in CatchUp instead.
                    //
                    if (Interlocked.Decrement(ref _pendingTickCount) > 0)
                    {
                        var c = self.Schedule((@this: this, index: 1L), (tuple, action) => tuple.@this.CatchUp(tuple.index, action));

                        return StableCompositeDisposable.Create(d, c);
                    }

                    return d;
                }

                private void Tock()
                {
                    //
                    // Notice the handler for (emulated) periodic scheduling is non-reentrant.
                    //
                    // When there's no overlap with the OnNext(0L) call, the following code will cycle through
                    // pendingTickCount 0 -> 1 -> 0 for the remainder of the timer's execution.
                    //
                    // If there's overlap with the OnNext(0L) call, pendingTickCount will increase to record
                    // the number of catch up OnNext calls required, which will be dispatched by the recursive
                    // scheduling loop in CatchUp (which quits when it reaches 0 pending ticks).
                    //
                    if (Interlocked.Increment(ref _pendingTickCount) == 1)
                    {
                        var count = _index;
                        _index = unchecked(count + 1);

                        ForwardOnNext(count);
                        Interlocked.Decrement(ref _pendingTickCount);
                    }
                }

                private void CatchUp(long count, Action<(_, long)> recurse)
                {
                    try
                    {
                        ForwardOnNext(count);
                    }
                    catch (Exception e)
                    {
                        _periodic.Dispose();
                        e.Throw();
                    }

                    //
                    // We can simply bail out if we decreased the tick count to 0. In that case, the Tock
                    // method will take over when it sees the 0 -> 1 transition.
                    //
                    if (Interlocked.Decrement(ref _pendingTickCount) > 0)
                    {
                        recurse((this, unchecked(count + 1)));
                    }
                }
            }
        }
    }
}
