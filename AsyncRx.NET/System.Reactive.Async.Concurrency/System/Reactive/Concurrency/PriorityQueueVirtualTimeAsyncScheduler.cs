using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Base class for virtual time schedulers using a priority queue for scheduled items.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    /// <typeparam name="TRelative">Relative time representation type.</typeparam>
    public abstract class PriorityQueueVirtualTimeAsyncScheduler<TAbsolute, TRelative> : VirtualTimeAsyncScheduler<TAbsolute, TRelative>
        where TAbsolute : IComparable<TAbsolute>
    {
        private readonly SchedulerQueue<TAbsolute> _queue = new SchedulerQueue<TAbsolute>();
        private readonly AsyncLock _queueGate = new AsyncLock();

        /// <summary>
        /// Creates a new virtual time scheduler with the default value of TAbsolute as the initial clock value.
        /// </summary>
        protected PriorityQueueVirtualTimeAsyncScheduler()
        {
        }

        /// <summary>
        /// Creates a new virtual time scheduler.
        /// </summary>
        /// <param name="initialClock">Initial value for the clock.</param>
        /// <param name="comparer">Comparer to determine causality of events based on absolute time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <c>null</c>.</exception>
        protected PriorityQueueVirtualTimeAsyncScheduler(TAbsolute initialClock, IComparer<TAbsolute> comparer)
            : base(initialClock, comparer)
        {
        }

        /// <summary>
        /// Gets the next scheduled item to be executed.
        /// </summary>
        /// <returns>The next scheduled item.</returns>
        internal override async Task<ScheduledItem<TAbsolute>> GetNext()
        {
            using (await _queueGate.LockAsync().ConfigureAwait(false))
            {
                while (_queue.Count > 0)
                {
                    var next = _queue.Peek();
                    if (next.IsDisposed)
                    {
                        _queue.Dequeue();
                    }
                    else
                    {
                        return next;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        public override async Task<IAsyncDisposable> ScheduleAbsolute(TAbsolute dueTime, Func<CancellationToken, Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var si = default(ScheduledItem<TAbsolute>);

            async Task<IAsyncDisposable> run()
            {
                using (await _queueGate.LockAsync().ConfigureAwait(false)) _queue.Remove(si);
                var cad = new CancellationAsyncDisposable();
                await action(cad.Token).ConfigureAwait(false);
                return cad;
            }

            si = new ScheduledItem<TAbsolute>(run, dueTime, Comparer);

            using (await _queueGate.LockAsync().ConfigureAwait(false)) _queue.Enqueue(si);

            return si;
        }
    }
}
