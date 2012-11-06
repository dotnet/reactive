// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_PERF
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq.Observαble
{
    class Timer : Producer<long>
    {
        private readonly DateTimeOffset? _dueTimeA;
        private readonly TimeSpan? _dueTimeR;
        private readonly TimeSpan? _period;
        private readonly IScheduler _scheduler;

        public Timer(DateTimeOffset dueTime, TimeSpan? period, IScheduler scheduler)
        {
            _dueTimeA = dueTime;
            _period = period;
            _scheduler = scheduler;
        }

        public Timer(TimeSpan dueTime, TimeSpan? period, IScheduler scheduler)
        {
            _dueTimeR = dueTime;
            _period = period;
            _scheduler = scheduler;
        }

        protected override IDisposable Run(IObserver<long> observer, IDisposable cancel, Action<IDisposable> setSink)
        {
            if (_period.HasValue)
            {
                var sink = new π(this, observer, cancel);
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

        class _ : Sink<long>
        {
            private readonly Timer _parent;

            public _(Timer parent, IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
            }

            public IDisposable Run()
            {
                if (_parent._dueTimeA.HasValue)
                {
                    return _parent._scheduler.Schedule(_parent._dueTimeA.Value, Invoke);
                }
                else
                {
                    return _parent._scheduler.Schedule(_parent._dueTimeR.Value, Invoke);
                }
            }

            private void Invoke()
            {
                base._observer.OnNext(0);
                base._observer.OnCompleted();
                base.Dispose();
            }
        }

        class π : Sink<long>
        {
            private readonly Timer _parent;
            private readonly TimeSpan _period;

            public π(Timer parent, IObserver<long> observer, IDisposable cancel)
                : base(observer, cancel)
            {
                _parent = parent;
                _period = _parent._period.Value;
            }

            public IDisposable Run()
            {
                if (_parent._dueTimeA.HasValue)
                {
                    var dueTime = _parent._dueTimeA.Value;
                    return _parent._scheduler.Schedule(default(object), dueTime, InvokeStart);
                }
                else
                {
                    var dueTime = _parent._dueTimeR.Value;

                    //
                    // Optimize for the case of Observable.Interval.
                    //
                    if (dueTime == _period)
                    {
                        return _parent._scheduler.SchedulePeriodic(0L, _period, (Func<long, long>)Tick);
                    }

                    return _parent._scheduler.Schedule(default(object), dueTime, InvokeStart);
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
            private long Tick(long count)
            {
                base._observer.OnNext(count);
                return unchecked(count + 1);
            }

            private int _pendingTickCount;
            private IDisposable _periodic;

            private IDisposable InvokeStart(IScheduler self, object state)
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
                d.Disposable = self.SchedulePeriodic(1L, _period, (Func<long, long>)Tock);

                try
                {
                    base._observer.OnNext(0L);
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
                    var c = new SingleAssignmentDisposable();
                    c.Disposable = self.Schedule(1L, CatchUp);

                    return new CompositeDisposable(2) { d, c };
                }

                return d;
            }

            private long Tock(long count)
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
                    base._observer.OnNext(count);
                    Interlocked.Decrement(ref _pendingTickCount);
                }

                return unchecked(count + 1);
            }

            private void CatchUp(long count, Action<long> recurse)
            {
                try
                {
                    base._observer.OnNext(count);
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
                    recurse(unchecked(count + 1));
                }
            }
        }
    }
}
#endif