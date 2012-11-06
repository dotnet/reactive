// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Threading;
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the current thread.
    /// </summary>
    /// <seealso cref="Scheduler.CurrentThread">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class CurrentThreadScheduler : LocalScheduler
    {
        private static readonly CurrentThreadScheduler s_instance = new CurrentThreadScheduler();

        CurrentThreadScheduler()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the current thread scheduler.
        /// </summary>
        public static CurrentThreadScheduler Instance
        {
            get { return s_instance; }
        }

#if !NO_TLS
        [ThreadStatic]
        static SchedulerQueue<TimeSpan> s_threadLocalQueue;

        [ThreadStatic]
        static IStopwatch s_clock;

        private static SchedulerQueue<TimeSpan> GetQueue()
        {
            return s_threadLocalQueue;
        }

        private static void SetQueue(SchedulerQueue<TimeSpan> newQueue)
        {
            s_threadLocalQueue = newQueue;
        }

        private static TimeSpan Time
        {
            get
            {
                if (s_clock == null)
                    s_clock = ConcurrencyAbstractionLayer.Current.StartStopwatch();

                return s_clock.Elapsed;
            }
        }
#else
        private static readonly System.Collections.Generic.Dictionary<int, SchedulerQueue<TimeSpan>> s_queues = new System.Collections.Generic.Dictionary<int, SchedulerQueue<TimeSpan>>();
        
        private static readonly System.Collections.Generic.Dictionary<int, IStopwatch> s_clocks = new System.Collections.Generic.Dictionary<int, IStopwatch>();

        private static SchedulerQueue<TimeSpan> GetQueue()
        {
            lock (s_queues)
            { 
                var item = default(SchedulerQueue<TimeSpan>);
                if (s_queues.TryGetValue(Thread.CurrentThread.ManagedThreadId, out item))
                    return item;

                return null;	
            }
        }

        private static void SetQueue(SchedulerQueue<TimeSpan> newQueue)
        {
            lock (s_queues)
            {
                if (newQueue == null)
                    s_queues.Remove(Thread.CurrentThread.ManagedThreadId);
                else
                    s_queues[Thread.CurrentThread.ManagedThreadId] = newQueue;
            }
        }

        private static TimeSpan Time
        {
            get
            {
                lock (s_clocks)
                {
                    var clock = default(IStopwatch);
                    if (!s_clocks.TryGetValue(Thread.CurrentThread.ManagedThreadId, out clock))
                        s_clocks[Thread.CurrentThread.ManagedThreadId] = clock = ConcurrencyAbstractionLayer.Current.StartStopwatch();

                    return clock.Elapsed;
                }
            }
        }
#endif

        /// <summary>
        /// Gets a value that indicates whether the caller must call a Schedule method.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Now marked as obsolete.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(Constants_Core.OBSOLETE_SCHEDULEREQUIRED)] // Preferring static method call over instance method call.
        public bool ScheduleRequired
        {
            get
            {
                return IsScheduleRequired;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the caller must call a Schedule method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static bool IsScheduleRequired
        {
            get
            {
                return GetQueue() == null;
            }
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var dt = Time + Scheduler.Normalize(dueTime);

            var si = new ScheduledItem<TimeSpan, TState>(this, state, action, dt);

            var queue = GetQueue();

            if (queue == null)
            {
                queue = new SchedulerQueue<TimeSpan>(4);
                queue.Enqueue(si);

                CurrentThreadScheduler.SetQueue(queue);
                try
                {
                    Trampoline.Run(queue);
                }
                finally
                {
                    CurrentThreadScheduler.SetQueue(null);
                }
            }
            else
            {
                queue.Enqueue(si);
            }

            return Disposable.Create(si.Cancel);
        }

        static class Trampoline
        {
            public static void Run(SchedulerQueue<TimeSpan> queue)
            {
                while (queue.Count > 0)
                {
                    var item = queue.Dequeue();
                    if (!item.IsCanceled)
                    {
                        var wait = item.DueTime - CurrentThreadScheduler.Time;
                        if (wait.Ticks > 0)
                        {
                            ConcurrencyAbstractionLayer.Current.Sleep(wait);
                        }

                        if (!item.IsCanceled)
                            item.Invoke();
                    }
                }
            }
        }
    }
}
