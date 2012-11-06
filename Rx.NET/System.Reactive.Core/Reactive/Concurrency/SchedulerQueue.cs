// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Efficient scheduler queue that maintains scheduled items sorted by absolute time.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    /// <remarks>This type is not thread safe; users should ensure proper synchronization.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "But it *is* a queue!")]
    public class SchedulerQueue<TAbsolute>
        where TAbsolute : IComparable<TAbsolute>
    {
        private readonly PriorityQueue<ScheduledItem<TAbsolute>> _queue;

        /// <summary>
        /// Creates a new scheduler queue with a default initial capacity.
        /// </summary>
        public SchedulerQueue()
            : this(1024)
        {
        }

        /// <summary>
        /// Creats a new scheduler queue with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity of the scheduler queue.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than zero.</exception>
        public SchedulerQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");

            _queue = new PriorityQueue<ScheduledItem<TAbsolute>>(capacity);
        }

        /// <summary>
        /// Gets the number of scheduled items in the scheduler queue.
        /// </summary>
        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }

        /// <summary>
        /// Enqueues the specified work item to be scheduled.
        /// </summary>
        /// <param name="scheduledItem">Work item to be scheduled.</param>
        public void Enqueue(ScheduledItem<TAbsolute> scheduledItem)
        {
            _queue.Enqueue(scheduledItem);
        }

        /// <summary>
        /// Removes the specified work item from the scheduler queue.
        /// </summary>
        /// <param name="scheduledItem">Work item to be removed from the scheduler queue.</param>
        /// <returns>true if the item was found; false otherwise.</returns>
        public bool Remove(ScheduledItem<TAbsolute> scheduledItem)
        {
            return _queue.Remove(scheduledItem);
        }

        /// <summary>
        /// Dequeues the next work item from the scheduler queue.
        /// </summary>
        /// <returns>Next work item in the scheduler queue (removed).</returns>
        public ScheduledItem<TAbsolute> Dequeue()
        {
            return _queue.Dequeue();
        }

        /// <summary>
        /// Peeks the next work item in the scheduler queue.
        /// </summary>
        /// <returns>Next work item in the scheduler queue (not removed).</returns>
        public ScheduledItem<TAbsolute> Peek()
        {
            return _queue.Peek();
        }
    }
}
