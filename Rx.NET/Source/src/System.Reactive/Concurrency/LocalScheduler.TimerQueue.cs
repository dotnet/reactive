// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Threading;

namespace System.Reactive.Concurrency
{
    public partial class LocalScheduler
    {
        /// <summary>
        /// Gate to protect local scheduler queues.
        /// </summary>
        private static readonly object _gate = new object();

        /// <summary>
        /// Gate to protect queues and to synchronize scheduling decisions and system clock
        /// change management.
        /// </summary>
        private static readonly object s_gate = new object();

        /// <summary>
        /// Long term work queue. Contains work that's due beyond SHORTTERM, computed at the
        /// time of enqueueing.
        /// </summary>
        private static readonly PriorityQueue<WorkItem/*!*/> s_longTerm = new PriorityQueue<WorkItem/*!*/>();

        /// <summary>
        /// Disposable resource for the long term timer that will reevaluate and dispatch the
        /// first item in the long term queue. A serial disposable is used to make "dispose
        /// current and assign new" logic easier. The disposable itself is never disposed.
        /// </summary>
        private static readonly SerialDisposable s_nextLongTermTimer = new SerialDisposable();

        /// <summary>
        /// Item at the head of the long term queue for which the current long term timer is
        /// running. Used to detect changes in the queue and decide whether we should replace
        /// or can continue using the current timer (because no earlier long term work was
        /// added to the queue).
        /// </summary>
        private static WorkItem s_nextLongTermWorkItem = null;

        /// <summary>
        /// Short term work queue. Contains work that's due soon, computed at the time of
        /// enqueueing or upon reevaluation of the long term queue causing migration of work
        /// items. This queue is kept in order to be able to relocate short term items back
        /// to the long term queue in case a system clock change occurs.
        /// </summary>
        private readonly PriorityQueue<WorkItem/*!*/> _shortTerm = new PriorityQueue<WorkItem/*!*/>();

        /// <summary>
        /// Set of disposable handles to all of the current short term work Schedule calls,
        /// allowing those to be cancelled upon a system clock change.
        /// </summary>
        private readonly HashSet<IDisposable> _shortTermWork = new HashSet<IDisposable>();

        /// <summary>
        /// Threshold where an item is considered to be short term work or gets moved from
        /// long term to short term.
        /// </summary>
        private static readonly TimeSpan SHORTTERM = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Maximum error ratio for timer drift. We've seen machines with 10s drift on a
        /// daily basis, which is in the order 10E-4, so we allow for extra margin here.
        /// This value is used to calculate early arrival for the long term queue timer
        /// that will reevaluate work for the short term queue.
        /// 
        /// Example:  -------------------------------...---------------------*-----$
        ///                                                                  ^     ^
        ///                                                                  |     |
        ///                                                                early  due
        ///                                                                0.999  1.0
        ///                                                                
        /// We also make the gap between early and due at least LONGTOSHORT so we have
        /// enough time to transition work to short term and as a courtesy to the
        /// destination scheduler to manage its queues etc.
        /// </summary>
        private const int MAXERRORRATIO = 1000;

        /// <summary>
        /// Minimum threshold for the long term timer to fire before the queue is reevaluated
        /// for short term work. This value is chosen to be less than SHORTTERM in order to
        /// ensure the timer fires and has work to transition to the short term queue.
        /// </summary>
        private static readonly TimeSpan LONGTOSHORT = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Threshold used to determine when a short term timer has fired too early compared
        /// to the absolute due time. This provides a last chance protection against early
        /// completion of scheduled work, which can happen in case of time adjustment in the
        /// operating system (cf. GetSystemTimeAdjustment).
        /// </summary>
        private static readonly TimeSpan RETRYSHORT = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// Longest interval supported by timers in the BCL.
        /// </summary>
        private static readonly TimeSpan MAXSUPPORTEDTIMER = TimeSpan.FromMilliseconds((1L << 32) - 2);

        /// <summary>
        /// Creates a new local scheduler.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "We can't really lift this into a field initializer, and would end up checking for an initialization flag in every static method anyway (which is roughly what the JIT does in a thread-safe manner).")]
        protected LocalScheduler()
        {
            //
            // Hook up for system clock change notifications. This doesn't do anything until the
            // AddRef method is called (which can throw).
            //
            SystemClock.Register(this);
        }

        /// <summary>
        /// Enqueues absolute time scheduled work in the timer queue or the short term work list.
        /// </summary>
        /// <param name="state">State to pass to the action.</param>
        /// <param name="dueTime">Absolute time to run the work on. The timer queue is responsible to execute the work close to the specified time, also accounting for system clock changes.</param>
        /// <param name="action">Action to run, potentially recursing into the scheduler.</param>
        /// <returns>Disposable object to prevent the work from running.</returns>
        private IDisposable Enqueue<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            //
            // Work that's due in the past is sent to the underlying scheduler through the Schedule
            // overload for execution at TimeSpan.Zero. We don't go to the overload for immediate
            // scheduling in order to:
            //
            // - Preserve the time-based nature of the call as surfaced to the underlying scheduler,
            //   as it may use different queuing strategies.
            //
            // - Optimize for the default behavior of LocalScheduler where a virtual call to Schedule
            //   for immediate execution calls into the abstract Schedule method with TimeSpan.Zero.
            //
            var due = Scheduler.Normalize(dueTime - Now);
            if (due == TimeSpan.Zero)
            {
                return Schedule<TState>(state, TimeSpan.Zero, action);
            }

            //
            // We're going down the path of queueing up work or scheduling it, so we need to make
            // sure we can get system clock change notifications. If not, the call below is expected
            // to throw NotSupportedException. WorkItem.Invoke decreases the ref count again to allow
            // the system clock monitor to stop if there's no work left. Notice work items always
            // reach an execution stage since we don't dequeue items but merely mark them as cancelled
            // through WorkItem.Dispose. Double execution is also prevented, so the ref count should
            // correctly balance out.
            //
            SystemClock.AddRef();

            var workItem = new WorkItem<TState>(this, state, dueTime, action);

            if (due <= SHORTTERM)
            {
                ScheduleShortTermWork(workItem);
            }
            else
            {
                ScheduleLongTermWork(workItem);
            }

            return workItem;
        }

        /// <summary>
        /// Schedule work that's due in the short term. This leads to relative scheduling calls to the
        /// underlying scheduler for short TimeSpan values. If the system clock changes in the meantime,
        /// the short term work is attempted to be cancelled and reevaluated.
        /// </summary>
        /// <param name="item">Work item to schedule in the short term. The caller is responsible to determine the work is indeed short term.</param>
        private void ScheduleShortTermWork(WorkItem/*!*/ item)
        {
            lock (_gate)
            {
                _shortTerm.Enqueue(item);

                //
                // We don't bother trying to dequeue the item or stop the timer upon cancellation,
                // but always let the timer fire to do the queue maintenance. When the item is
                // cancelled, it won't run (see WorkItem.Invoke). In the event of a system clock
                // change, all outstanding work in _shortTermWork is cancelled and the short
                // term queue is reevaluated, potentially prompting rescheduling of short term
                // work. Notice work is protected against double execution by the implementation
                // of WorkItem.Invoke.
                //
                var d = new SingleAssignmentDisposable();
                _shortTermWork.Add(d);

                //
                // We normalize the time delta again (possibly redundant), because we can't assume
                // the underlying scheduler implementations is valid and deals with negative values
                // (though it should).
                //
                var dueTime = Scheduler.Normalize(item.DueTime - item.Scheduler.Now);
                d.Disposable = item.Scheduler.Schedule(d, dueTime, ExecuteNextShortTermWorkItem);
            }
        }

        /// <summary>
        /// Callback to process the next short term work item.
        /// </summary>
        /// <param name="scheduler">Recursive scheduler supplied by the underlying scheduler.</param>
        /// <param name="cancel">Disposable used to identify the work the timer was triggered for (see code for usage).</param>
        /// <returns>Empty disposable. Recursive work cancellation is wired through the original WorkItem.</returns>
        private IDisposable ExecuteNextShortTermWorkItem(IScheduler scheduler, IDisposable cancel)
        {
            var next = default(WorkItem);

            lock (_gate)
            {
                //
                // Notice that even though we try to cancel all work in the short term queue upon a
                // system clock change, cancellation may not be honored immediately and there's a
                // small chance this code runs for work that has been cancelled. Because the handler
                // doesn't execute the work that triggered the time-based Schedule call, but always
                // runs the work from the short term queue in order, we need to make sure we're not
                // stealing items in the queue. We can do so by remembering the object identity of
                // the disposable and check whether it still exists in the short term work list. If
                // not, a system clock change handler has gotten rid of it as part of reevaluating
                // the short term queue, but we still ended up here because the inherent race in the
                // call to Dispose versus the underlying timer. It's also possible the underlying
                // scheduler does a bad job at cancellation, so this measure helps for that too.
                //
                if (_shortTermWork.Remove(cancel) && _shortTerm.Count > 0)
                {
                    next = _shortTerm.Dequeue();
                }
            }

            if (next != null)
            {
                //
                // If things don't make sense and we're way too early to run the work, this is our
                // final chance to prevent us from running before the due time. This situation can
                // arise when Windows applies system clock adjustment (see SetSystemTimeAdjustment)
                // and as a result the clock is ticking slower. If the clock is ticking faster due
                // to such an adjustment, too bad :-). We try to minimize the window for the final
                // relative time based scheduling such that 10%+ adjustments to the clock rate
                // have only "little" impact (range of 100s of ms). On an absolute time scale, we
                // don't provide stronger guarantees.
                //
                if (next.DueTime - next.Scheduler.Now >= RETRYSHORT)
                {
                    ScheduleShortTermWork(next);
                }
                else
                {
                    //
                    // Invocation happens on the recursive scheduler supplied to the function. We
                    // are already running on the target scheduler, so we should stay on board.
                    // Not doing so would have unexpected behavior for e.g. NewThreadScheduler,
                    // causing a whole new thread to be allocated because of a top-level call to
                    // the Schedule method rather than a recursive one.
                    //
                    // Notice if work got cancelled, the call to Invoke will not propagate to user
                    // code because of the IsDisposed check inside.
                    //
                    next.Invoke(scheduler);
                }
            }

            //
            // No need to return anything better here. We already handed out the original WorkItem
            // object upon the call to Enqueue (called itself by Schedule). The disposable inside
            // the work item allows a cancellation request to chase the underlying computation.
            //
            return Disposable.Empty;
        }

        /// <summary>
        /// Schedule work that's due on the long term. This leads to the work being queued up for
        /// eventual transitioning to the short term work list.
        /// </summary>
        /// <param name="item">Work item to schedule on the long term. The caller is responsible to determine the work is indeed long term.</param>
        private static void ScheduleLongTermWork(WorkItem/*!*/ item)
        {
            lock (s_gate)
            {
                s_longTerm.Enqueue(item);

                //
                // In case we're the first long-term item in the queue now, the timer will have
                // to be updated.
                //
                UpdateLongTermProcessingTimer();
            }
        }

        /// <summary>
        /// Updates the long term timer which is responsible to transition work from the head of the
        /// long term queue to the short term work list.
        /// </summary>
        /// <remarks>Should be called under the scheduler lock.</remarks>
        private static void UpdateLongTermProcessingTimer()
        {
            /*
             * CALLERS - Ensure this is called under the lock!
             * 
            lock (s_gate) */
            {
                if (s_longTerm.Count == 0)
                    return;

                //
                // To avoid setting the timer all over again for the first work item if it hasn't changed,
                // we keep track of the next long term work item that will be processed by the timer.
                //
                var next = s_longTerm.Peek();
                if (next == s_nextLongTermWorkItem)
                    return;

                //
                // We need to arrive early in order to accommodate for potential drift. The relative amount
                // of drift correction is kept in MAXERRORRATIO. At the very least, we want to be LONGTOSHORT
                // early to make the final jump from long term to short term, giving the target scheduler
                // enough time to process the item through its queue. LONGTOSHORT is chosen such that the
                // error due to drift is negligible.
                //
                var due = Scheduler.Normalize(next.DueTime - next.Scheduler.Now);
                var remainder = TimeSpan.FromTicks(Math.Max(due.Ticks / MAXERRORRATIO, LONGTOSHORT.Ticks));
                var dueEarly = due - remainder;

                //
                // Limit the interval to maximum supported by underlying Timer.
                //
                var dueCapped = TimeSpan.FromTicks(Math.Min(dueEarly.Ticks, MAXSUPPORTEDTIMER.Ticks));

                s_nextLongTermWorkItem = next;
                s_nextLongTermTimer.Disposable = ConcurrencyAbstractionLayer.Current.StartTimer(EvaluateLongTermQueue, null, dueCapped);
            }
        }

        /// <summary>
        /// Evaluates the long term queue, transitioning short term work to the short term list,
        /// and adjusting the new long term processing timer accordingly.
        /// </summary>
        /// <param name="state">Ignored.</param>
        private static void EvaluateLongTermQueue(object state)
        {
            lock (s_gate)
            {
                var next = default(WorkItem);

                while (s_longTerm.Count > 0)
                {
                    next = s_longTerm.Peek();

                    var due = Scheduler.Normalize(next.DueTime - next.Scheduler.Now);
                    if (due >= SHORTTERM)
                        break;

                    var item = s_longTerm.Dequeue();
                    item.Scheduler.ScheduleShortTermWork(item);
                }

                s_nextLongTermWorkItem = null;
                UpdateLongTermProcessingTimer();
            }
        }

        /// <summary>
        /// Callback invoked when a system clock change is observed in order to adjust and reevaluate
        /// the internal scheduling queues.
        /// </summary>
        /// <param name="args">Currently not used.</param>
        /// <param name="sender">Currently not used.</param>
        internal void SystemClockChanged(object sender, SystemClockChangedEventArgs args)
        {
            lock (s_gate)
            {
                lock (_gate)
                {
                    //
                    // Best-effort cancellation of short term work. A check for presence in the hash set
                    // is used to notice race conditions between cancellation and the timer firing (also
                    // guarded by the same gate object). See checks in ExecuteNextShortTermWorkItem.
                    //
                    foreach (var d in _shortTermWork)
                    {
                        d.Dispose();
                    }

                    _shortTermWork.Clear();

                    //
                    // Transition short term work to the long term queue for reevaluation by calling the
                    // EvaluateLongTermQueue method. We don't know which direction the clock was changed
                    // in, so we don't optimize for special cases, but always transition the whole queue.
                    // Notice the short term queue is bounded to SHORTTERM length.
                    //
                    while (_shortTerm.Count > 0)
                    {
                        var next = _shortTerm.Dequeue();
                        s_longTerm.Enqueue(next);
                    }

                    //
                    // Reevaluate the queue and don't forget to null out the current timer to force the
                    // method to create a new timer for the new first long term item.
                    //
                    s_nextLongTermWorkItem = null;
                    EvaluateLongTermQueue(null);
                }
            }
        }

        /// <summary>
        /// Represents a work item in the absolute time scheduler.
        /// </summary>
        /// <remarks>
        /// This type is very similar to ScheduledItem, but we need a different Invoke signature to allow customization
        /// of the target scheduler (e.g. when called in a recursive scheduling context, see ExecuteNextShortTermWorkItem).
        /// </remarks>
        private abstract class WorkItem : IComparable<WorkItem>, IDisposable
        {
            public readonly LocalScheduler Scheduler;
            public readonly DateTimeOffset DueTime;

            private readonly SingleAssignmentDisposable _disposable;
            private int _hasRun;

            public WorkItem(LocalScheduler scheduler, DateTimeOffset dueTime)
            {
                Scheduler = scheduler;
                DueTime = dueTime;

                _disposable = new SingleAssignmentDisposable();
                _hasRun = 0;
            }

            public void Invoke(IScheduler scheduler)
            {
                //
                // Protect against possible maltreatment of the scheduler queues or races in
                // execution of a work item that got relocated across system clock changes.
                // Under no circumstance whatsoever we should run work twice. The monitor's
                // ref count should also be subject to this policy.
                //
                if (Interlocked.Exchange(ref _hasRun, 1) == 0)
                {
                    try
                    {
                        if (!_disposable.IsDisposed)
                        {
                            _disposable.Disposable = InvokeCore(scheduler);
                        }
                    }
                    finally
                    {
                        SystemClock.Release();
                    }
                }
            }

            protected abstract IDisposable InvokeCore(IScheduler scheduler);

            public int CompareTo(WorkItem/*!*/ other) => Comparer<DateTimeOffset>.Default.Compare(DueTime, other.DueTime);

            public void Dispose() => _disposable.Dispose();
        }

        /// <summary>
        /// Represents a work item that closes over scheduler invocation state. Subtyping is
        /// used to have a common type for the scheduler queues.
        /// </summary>
        private sealed class WorkItem<TState> : WorkItem
        {
            private readonly TState _state;
            private readonly Func<IScheduler, TState, IDisposable> _action;

            public WorkItem(LocalScheduler scheduler, TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
                : base(scheduler, dueTime)
            {
                _state = state;
                _action = action;
            }

            protected override IDisposable InvokeCore(IScheduler scheduler) => _action(scheduler, _state);
        }
    }
}
