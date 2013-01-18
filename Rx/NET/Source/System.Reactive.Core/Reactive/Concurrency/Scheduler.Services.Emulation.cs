// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Threading;

namespace System.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        /// <summary>
        /// Schedules a periodic piece of work by dynamically discovering the scheduler's capabilities.
        /// If the scheduler supports periodic scheduling, the request will be forwarded to the periodic scheduling implementation.
        /// If the scheduler provides stopwatch functionality, the periodic task will be emulated using recursive scheduling with a stopwatch to correct for time slippage.
        /// Otherwise, the periodic task will be emulated using recursive scheduling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">The scheduler to run periodic work on.</param>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public static IDisposable SchedulePeriodic<TState>(this IScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            return SchedulePeriodic_(scheduler, state, period, action);
        }

        /// <summary>
        /// Schedules a periodic piece of work by dynamically discovering the scheduler's capabilities.
        /// If the scheduler supports periodic scheduling, the request will be forwarded to the periodic scheduling implementation.
        /// If the scheduler provides stopwatch functionality, the periodic task will be emulated using recursive scheduling with a stopwatch to correct for time slippage.
        /// Otherwise, the periodic task will be emulated using recursive scheduling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public static IDisposable SchedulePeriodic<TState>(this IScheduler scheduler, TState state, TimeSpan period, Action<TState> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            return SchedulePeriodic_(scheduler, state, period, state_ => { action(state_); return state_; });
        }

        /// <summary>
        /// Schedules a periodic piece of work by dynamically discovering the scheduler's capabilities.
        /// If the scheduler supports periodic scheduling, the request will be forwarded to the periodic scheduling implementation.
        /// If the scheduler provides stopwatch functionality, the periodic task will be emulated using recursive scheduling with a stopwatch to correct for time slippage.
        /// Otherwise, the periodic task will be emulated using recursive scheduling.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public static IDisposable SchedulePeriodic(this IScheduler scheduler, TimeSpan period, Action action)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            return SchedulePeriodic_(scheduler, action, period, a => { a(); return a; });
        }

        /// <summary>
        /// Starts a new stopwatch object by dynamically discovering the scheduler's capabilities.
        /// If the scheduler provides stopwatch functionality, the request will be forwarded to the stopwatch provider implementation.
        /// Otherwise, the stopwatch will be emulated using the scheduler's notion of absolute time.
        /// </summary>
        /// <param name="scheduler">Scheduler to obtain a stopwatch for.</param>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        /// <remarks>The resulting stopwatch object can have non-monotonic behavior.</remarks>
        public static IStopwatch StartStopwatch(this IScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            //
            // All schedulers deriving from LocalScheduler will automatically pick up this
            // capability based on a local stopwatch, typically using QueryPerformanceCounter
            // through the System.Diagnostics.Stopwatch class.
            //
            // Notice virtual time schedulers do implement this facility starting from Rx v2.0,
            // using subtraction of their absolute time notion to compute elapsed time values.
            // This is fine because those schedulers do not allow the clock to go back in time.
            //
            // For schedulers that don't have a stopwatch, we have to pick some fallback logic
            // here. We could either dismiss the scheduler's notion of time and go for the CAL's
            // stopwatch facility, or go with a stopwatch based on "scheduler.Now", which has
            // the drawback of potentially going back in time:
            //
            //   - Using the CAL's stopwatch facility causes us to abondon the scheduler's
            //     potentially virtualized notion of time, always going for the local system
            //     time instead.
            //
            //   - Using the scheduler's Now property for calculations can break monotonicity,
            //     and there's no right answer on how to deal with jumps back in time.
            //
            // However, even the built-in stopwatch in the BCL can potentially fall back to
            // subtraction of DateTime values in case no high-resolution performance counter is
            // available, causing monotonicity to break down. We're not trying to solve this
            // problem there either (though we could check IsHighResolution and smoothen out
            // non-monotonic points somehow), so we pick the latter option as the lesser of
            // two evils (also because it should occur rarely).
            //
            // Users of the stopwatch retrieved by this method could detect non-sensical data
            // revealing a jump back in time, or implement custom fallback logic like the one
            // shown below.
            //
            var swp = scheduler.AsStopwatchProvider();
            if (swp != null)
                return swp.StartStopwatch();

            return new EmulatedStopwatch(scheduler);
        }

        private static IDisposable SchedulePeriodic_<TState>(IScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
        {
            //
            // Design rationale:
            //
            //   In Rx v1.x, we employed recursive scheduling for periodic tasks. The following code
            //   fragment shows how the Timer (and hence Interval) function used to be implemented:
            //
            //     var p = Normalize(period);
            //
            //     return new AnonymousObservable<long>(observer =>
            //     {
            //         var d = dueTime;
            //         long count = 0;
            //         return scheduler.Schedule(d, self =>
            //         {
            //             if (p > TimeSpan.Zero)
            //             {
            //                 var now = scheduler.Now;
            //                 d = d + p;
            //                 if (d <= now)
            //                     d = now + p;
            //             }
            //
            //             observer.OnNext(count);
            //             count = unchecked(count + 1);
            //             self(d);
            //         });
            //     });
            //
            //   Despite the purity of this approach, it suffered from a set of drawbacks:
            //
            //    1) Usage of IScheduler.Now to correct for time drift did have a positive effect for
            //       a limited number of scenarios, in particular when a short period was used. The
            //       major issues with this are:
            //
            //       a) Relying on absolute time at the LINQ layer in Rx's layer map, causing issues
            //          when the system clock changes. Various customers hit this issue, reported to
            //          us on the MSDN forums. Basically, when the clock goes forward, the recursive
            //          loop wants to catch up as quickly as it can; when it goes backwards, a long
            //          silence will occur. (See 2 for a discussion of WP7 related fixes.)
            //
            //       b) Even if a) would be addressed by using Rx v2.0's capabilities to monitor for
            //          system clock changes, the solution would violate the reasonable expectation
            //          of operators overloads using TimeSpan *not* relying on absolute time.
            //
            //       c) Drift correction doesn't work for large periods when the system encounters
            //          systematic drift. For example, in the lab we've seen cases of drift up to
            //          tens of seconds on a 24 hour timeframe. Correcting for this drift by making
            //          a recursive call with a due time of 24 * 3600 with 10 seconds of adjustment
            //          won't fix systematic drift.
            //
            //    2) This implementation has been plagued with issues around application container
            //       lifecycle models, in particular Windows Phone 7's model of tombstoning and in
            //       particular its "dormant state". This feature was introduced in Mango to enable
            //       fast application switching. Essentially, the phone's OS puts the application
            //       in a suspended state when the user navigates "forward" (or takes an incoming
            //       call for instance). When the application is woken up again, threads are resumed
            //       and we're faced with an illusion of missed events due to the use of absolute
            //       time, not relative to how the application observes it. This caused nightmare
            //       scenarios of fast battery drain due to the flood of catch-up work.
            //
            //       See http://msdn.microsoft.com/en-us/library/ff817008(v=vs.92).aspx for more
            //       information on this.
            //
            //    3) Recursive scheduling imposes a non-trivial cost due to the creation of many
            //       single-shot timers and closures. For high frequency timers, this can cause a
            //       lot of churn in the GC, which we like to avoid (operators shouldn't have hidden
            //       linear - or worse - allocation cost).
            //
            //   Notice these drawbacks weren't limited to the use of Timer and Interval directly,
            //   as many operators such as Sample, Buffer, and Window used such sequences for their
            //   periodic behavior (typically by delegating to a more general overload).
            //
            //   As a result, in Rx v2.0, we took the decision to improve periodic timing based on
            //   the following design decisions:
            //
            //    1) When the scheduler has the ability to run a periodic task, it should implement
            //       the ISchedulerPeriodic interface and expose it through the IServiceProvider
            //       interface. Passing the intent of the user through all layers of Rx, down to the
            //       underlying infrastructure provides delegation of responsibilities. This allows
            //       the target scheduler to optimize execution in various ways, e.g. by employing
            //       techniques such as timer coalescing.
            //
            //       See http://www.bing.com/search?q=windows+timer+coalescing for information on
            //       techniques like timer coalescing which may be applied more aggressively in
            //       future OS releases in order to reduce power consumption.
            //
            //    2) Emulation of periodic scheduling is used to avoid breaking existing code that
            //       uses schedulers without this capability. We expect those fallback paths to be
            //       exercised rarely, though the use of DisableOptimizations can trigger them as
            //       well. In such cases we rely on stopwatches or a carefully crafted recursive
            //       scheme to deal with (or maximally compensate for) slippage or time. Behavior
            //       of periodic tasks is expected to be as follows:
            //
            //         timer ticks   0-------1-------2-------3-------4-------5-------6----...
            //                       |       |       |       +====+  +==+    |       |
            //         user code     +~~~|   +~|     +~~~~~~~~~~~|+~~~~|+~~| +~~~|   +~~|
            //
            //       rather than the following scheme, where time slippage is introduced by user
            //       code running on the scheduler:
            //
            //         timer ticks   0####-------1##-------2############-------3#####-----...
            //                       |           |         |                   |
            //         user code     +~~~|       +~|       +~~~~~~~~~~~|       +~~~~|
            //
            //       (Side-note: Unfortunately, we didn't reserve the name Interval for the latter
            //                   behavior, but used it as an alias for "periodic scheduling" with
            //                   the former behavior, delegating to the Timer implementation. One
            //                   can simulate this behavior using Generate, which uses tail calls.)
            //
            //       This behavior is important for operations like Sample, Buffer, and Window, all
            //       of which expect proper spacing of events, even if the user code takes a long
            //       time to complete (considered a bad practice nonetheless, cf. ObserveOn).
            //
            //    3) To deal with the issue of suspensions induced by application lifecycle events
            //       in Windows Phone and WinRT applications, we decided to hook available system
            //       events through IHostLifecycleNotifications, discovered through the PEP in order
            //       to maintain portability of the core of Rx.
            //
            var periodic = scheduler.AsPeriodic();
            if (periodic != null)
            {
                return periodic.SchedulePeriodic(state, period, action);
            }

            var swp = scheduler.AsStopwatchProvider();
            if (swp != null)
            {
                var spr = new SchedulePeriodicStopwatch<TState>(scheduler, state, period, action, swp);
                return spr.Start();
            }
            else
            {
                var spr = new SchedulePeriodicRecursive<TState>(scheduler, state, period, action);
                return spr.Start();
            }
        }

        class SchedulePeriodicStopwatch<TState>
        {
            private readonly IScheduler _scheduler;
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;
            private readonly IStopwatchProvider _stopwatchProvider;

            public SchedulePeriodicStopwatch(IScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action, IStopwatchProvider stopwatchProvider)
            {
                _scheduler = scheduler;
                _period = period;
                _action = action;
                _stopwatchProvider = stopwatchProvider;

                _state = state;
                _runState = STOPPED;
            }

            private TState _state;

            private readonly object _gate = new object();
            private readonly AutoResetEvent _resumeEvent = new AutoResetEvent(false);
            private volatile int _runState;
            private IStopwatch _stopwatch;
            private TimeSpan _nextDue;
            private TimeSpan _suspendedAt;
            private TimeSpan _inactiveTime;

            //
            // State transition diagram:
            //                                        (c)
            //                             +-----------<-----------+
            //                            /                         \
            //                           /            (b)            \
            //                          |           +-->--SUSPENDED---+
            //                  (a)     v          /          |
            //    ^----STOPPED -->-- RUNNING -->--+           v (e)
            //                                     \          |
            //                                      +-->--DISPOSED----$
            //                                        (d)
            //
            //  (a) Start --> call to Schedule the Tick method
            //  (b) Suspending event handler --> Tick gets blocked waiting for _resumeEvent
            //  (c) Resuming event handler --> _resumeEvent is signaled, Tick continues
            //  (d) Dispose returned object from Start --> scheduled work is cancelled
            //  (e) Dispose returned object from Start --> unblocks _resumeEvent, Tick exits
            //
            private const int STOPPED = 0;
            private const int RUNNING = 1;
            private const int SUSPENDED = 2;
            private const int DISPOSED = 3;

            public IDisposable Start()
            {
                RegisterHostLifecycleEventHandlers();

                _stopwatch = _stopwatchProvider.StartStopwatch();
                _nextDue = _period;
                _runState = RUNNING;

                return new CompositeDisposable(2)
                {
                    _scheduler.Schedule(_nextDue, Tick),
                    Disposable.Create(Cancel)
                };
            }

            private void Tick(Action<TimeSpan> recurse)
            {
                _nextDue += _period;
                _state = _action(_state);

                var next = default(TimeSpan);

                while (true)
                {
                    var shouldWaitForResume = false;

                    lock (_gate)
                    {
                        if (_runState == RUNNING)
                        {
                            //
                            // This is the fast path. We just let the stopwatch continue to
                            // run while we're suspended, but compensate for time that was
                            // recorded as inactive based on cumulative deltas computed in
                            // the suspend and resume event handlers.
                            //
                            next = Normalize(_nextDue - (_stopwatch.Elapsed - _inactiveTime));
                            break;
                        }
                        else if (_runState == DISPOSED)
                        {
                            //
                            // In case the periodic job gets disposed but we are currently
                            // waiting to come back out of suspension, we should make sure
                            // we don't remain blocked indefinitely. Hence, we set the event
                            // in the Cancel method and trap this case here to bail out from
                            // the scheduled work gracefully.
                            //
                            return;
                        }
                        else
                        {
                            //
                            // This is the least common case where we got suspended and need
                            // to block such that future reevaluations of the next due time
                            // will pick up the cumulative inactive time delta.
                            //
                            Debug.Assert(_runState == SUSPENDED);
                            shouldWaitForResume = true;
                        }
                    }

                    //
                    // Only happens in the SUSPENDED case; otherwise we will have broken from
                    // the loop or have quit the Tick method. After returning from the wait,
                    // we'll either be RUNNING again, quit due to a DISPOSED transition, or
                    // be extremely unlucky to find ourselves SUSPENDED again and be blocked
                    // once more.
                    //
                    if (shouldWaitForResume)
                        _resumeEvent.WaitOne();
                }

                recurse(next);
            }

            private void Cancel()
            {
                UnregisterHostLifecycleEventHandlers();

                lock (_gate)
                {
                    _runState = DISPOSED;

                    if (!Environment.HasShutdownStarted)
                        _resumeEvent.Set();
                }
            }

            private void Suspending(object sender, HostSuspendingEventArgs args)
            {
                //
                // The host is telling us we're about to be suspended. At this point, time
                // computations will still be in a valid range (next <= _period), but after
                // we're woken up again, Tick would start to go on a crucade to catch up.
                //
                // This has caused problems in the past, where the flood of events caused
                // batteries to drain etc (see design rationale discussion higher up).
                //
                // In order to mitigate this problem, we force Tick to suspend before its
                // next computation of the next due time. Notice we can't afford to block
                // during the Suspending event handler; the host expects us to respond to
                // this event quickly, such that we're not keeping the application from
                // suspending promptly.
                //
                lock (_gate)
                {
                    if (_runState == RUNNING)
                    {
                        _suspendedAt = _stopwatch.Elapsed;
                        _runState = SUSPENDED;

                        if (!Environment.HasShutdownStarted)
                            _resumeEvent.Reset();
                    }
                }
            }

            private void Resuming(object sender, HostResumingEventArgs args)
            {
                //
                // The host is telling us we're being resumed. At this point, code will
                // already be running in the process, so a past timer may still expire and
                // cause the code in Tick to run. Two interleavings are possible now:
                //
                //   1) We enter the gate first, and will adjust the cumulative inactive
                //      time delta used for correction. The code in Tick will have the
                //      illusion nothing happened and find itself RUNNING when entering
                //      the gate, resuming activities as before.
                //
                //   2) The code in Tick enters the gate first, and takes notice of the
                //      currently SUSPENDED state. It leaves the gate, entering the wait
                //      state for _resumeEvent. Next, we enter to adjust the cumulative
                //      inactive time delta, switch to the RUNNING state and signal the
                //      event for Tick to carry on and recompute its next due time based
                //      on the new cumulative delta.
                //
                lock (_gate)
                {
                    if (_runState == SUSPENDED)
                    {
                        _inactiveTime += _stopwatch.Elapsed - _suspendedAt;
                        _runState = RUNNING;

                        if (!Environment.HasShutdownStarted)
                            _resumeEvent.Set();
                    }
                }
            }

            private void RegisterHostLifecycleEventHandlers()
            {
                HostLifecycleService.Suspending += Suspending;
                HostLifecycleService.Resuming += Resuming;
                HostLifecycleService.AddRef();
            }

            private void UnregisterHostLifecycleEventHandlers()
            {
                HostLifecycleService.Suspending -= Suspending;
                HostLifecycleService.Resuming -= Resuming;
                HostLifecycleService.Release();
            }
        }

        class SchedulePeriodicRecursive<TState>
        {
            private readonly IScheduler _scheduler;
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;

            public SchedulePeriodicRecursive(IScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
            {
                _scheduler = scheduler;
                _period = period;
                _action = action;

                _state = state;
            }

            private TState _state;
            private int _pendingTickCount;
            private IDisposable _cancel;

            public IDisposable Start()
            {
                _pendingTickCount = 0;

                var d = new SingleAssignmentDisposable();
                _cancel = d;

                d.Disposable = _scheduler.Schedule(TICK, _period, Tick);

                return d;
            }

            //
            // The protocol using the three commands is explained in the Tick implementation below.
            //
            private const int TICK = 0;
            private const int DISPATCH_START = 1;
            private const int DISPATCH_END = 2;

            private void Tick(int command, Action<int, TimeSpan> recurse)
            {
                switch (command)
                {
                    case TICK:
                        //
                        // Ticks keep going at the specified periodic rate. We do a head call such
                        // that no slippage is introduced because of DISPATCH_START work involving
                        // user code that may take arbitrarily long.
                        //
                        recurse(TICK, _period);

                        //
                        // If we're not transitioning from 0 to 1 pending tick, another processing
                        // request is in flight which will see a non-zero pending tick count after
                        // doing the final decrement, causing it to reschedule immediately. We can
                        // safely bail out, delegating work to the catch-up tail calls.
                        //
                        if (Interlocked.Increment(ref _pendingTickCount) == 1)
                            goto case DISPATCH_START;

                        break;

                    case DISPATCH_START:
                        try
                        {
                            _state = _action(_state);
                        }
                        catch (Exception e)
                        {
                            _cancel.Dispose();
                            e.Throw();
                        }

                        //
                        // This is very subtle. We can't do a goto case DISPATCH_END here because it
                        // wouldn't introduce interleaving of periodic ticks that are due. In order
                        // to have best effort behavior for schedulers that don't have concurrency,
                        // we yield by doing a recursive call here. Notice this doesn't heal all of
                        // the problem, because the TICK commands that may be dispatched before the
                        // scheduled DISPATCH_END will do a "recurse(TICK, period)", which is relative
                        // from the point of entrance. Really all we're doing here is damage control
                        // for the case there's no stopwatch provider which should be rare (notice
                        // the LocalScheduler base class always imposes a stopwatch, but it can get
                        // disabled using DisableOptimizations; legacy implementations of schedulers
                        // from the v1.x days will not have a stopwatch).
                        //
                        recurse(DISPATCH_END, TimeSpan.Zero);

                        break;

                    case DISPATCH_END:
                        //
                        // If work was due while we were still running user code, the count will have
                        // been incremented by the periodic tick handler above. In that case, we will
                        // reschedule ourselves for dispatching work immediately.
                        //
                        // Notice we don't run a loop here, in order to allow interleaving of work on
                        // the scheduler by making recursive calls. In case we would use AsyncLock to
                        // ensure serialized execution the owner could get stuck in such a loop, thus
                        // we make tail calls to play nice with the scheduler.
                        //
                        if (Interlocked.Decrement(ref _pendingTickCount) > 0)
                            recurse(DISPATCH_START, TimeSpan.Zero);

                        break;
                }
            }
        }

        class EmulatedStopwatch : IStopwatch
        {
            private readonly IScheduler _scheduler;
            private readonly DateTimeOffset _start;

            public EmulatedStopwatch(IScheduler scheduler)
            {
                _scheduler = scheduler;
                _start = _scheduler.Now;
            }

            public TimeSpan Elapsed
            {
                get { return Scheduler.Normalize(_scheduler.Now - _start); }
            }
        }
    }
}
