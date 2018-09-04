// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Base class for historical schedulers, which are virtual time schedulers that use <see cref="DateTimeOffset"/> for absolute time and <see cref="TimeSpan"/> for relative time.
    /// </summary>
    public abstract class HistoricalSchedulerBase : VirtualTimeSchedulerBase<DateTimeOffset, TimeSpan>
    {
        /// <summary>
        /// Creates a new historical scheduler with the minimum value of <see cref="DateTimeOffset"/> as the initial clock value.
        /// </summary>
        protected HistoricalSchedulerBase()
            : base(DateTimeOffset.MinValue, Comparer<DateTimeOffset>.Default)
        {
        }

        /// <summary>
        /// Creates a new historical scheduler with the specified initial clock value.
        /// </summary>
        /// <param name="initialClock">Initial clock value.</param>
        protected HistoricalSchedulerBase(DateTimeOffset initialClock)
            : base(initialClock, Comparer<DateTimeOffset>.Default)
        {
        }

        /// <summary>
        /// Creates a new historical scheduler with the specified initial clock value and absolute time comparer.
        /// </summary>
        /// <param name="initialClock">Initial value for the clock.</param>
        /// <param name="comparer">Comparer to determine causality of events based on absolute time.</param>
        protected HistoricalSchedulerBase(DateTimeOffset initialClock, IComparer<DateTimeOffset> comparer)
            : base(initialClock, comparer)
        {
        }

        /// <summary>
        /// Adds a relative time value to an absolute time value.
        /// </summary>
        /// <param name="absolute">Absolute time value.</param>
        /// <param name="relative">Relative time value to add.</param>
        /// <returns>The resulting absolute time sum value.</returns>
        protected override DateTimeOffset Add(DateTimeOffset absolute, TimeSpan relative)
        {
            return absolute.Add(relative);
        }

        /// <summary>
        /// Converts the absolute time value to a <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="absolute">Absolute time value to convert.</param>
        /// <returns>The corresponding <see cref="DateTimeOffset"/> value.</returns>
        protected override DateTimeOffset ToDateTimeOffset(DateTimeOffset absolute) => absolute;

        /// <summary>
        /// Converts the <see cref="TimeSpan"/> value to a relative time value.
        /// </summary>
        /// <param name="timeSpan"><see cref="TimeSpan"/> value to convert.</param>
        /// <returns>The corresponding relative time value.</returns>
        protected override TimeSpan ToRelative(TimeSpan timeSpan) => timeSpan;
    }

    /// <summary>
    /// Provides a virtual time scheduler that uses <see cref="DateTimeOffset"/> for absolute time and <see cref="TimeSpan"/> for relative time.
    /// </summary>
    [DebuggerDisplay("\\{ " +
        nameof(Clock) + " = {" + nameof(Clock) + "} " +
        nameof(Now) + " = {" + nameof(Now) + ".ToString(\"O\")} " +
    "\\}")]
    public class HistoricalScheduler : HistoricalSchedulerBase
    {
        private readonly SchedulerQueue<DateTimeOffset> _queue = new SchedulerQueue<DateTimeOffset>();

        /// <summary>
        /// Creates a new historical scheduler with the minimum value of <see cref="DateTimeOffset"/> as the initial clock value.
        /// </summary>
        public HistoricalScheduler()
        {
        }

        /// <summary>
        /// Creates a new historical scheduler with the specified initial clock value.
        /// </summary>
        /// <param name="initialClock">Initial value for the clock.</param>
        public HistoricalScheduler(DateTimeOffset initialClock)
            : base(initialClock)
        {
        }

        /// <summary>
        /// Creates a new historical scheduler with the specified initial clock value.
        /// </summary>
        /// <param name="initialClock">Initial value for the clock.</param>
        /// <param name="comparer">Comparer to determine causality of events based on absolute time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <c>null</c>.</exception>
        public HistoricalScheduler(DateTimeOffset initialClock, IComparer<DateTimeOffset> comparer)
            : base(initialClock, comparer)
        {
        }

        /// <summary>
        /// Gets the next scheduled item to be executed.
        /// </summary>
        /// <returns>The next scheduled item.</returns>
        protected override IScheduledItem<DateTimeOffset> GetNext()
        {
            while (_queue.Count > 0)
            {
                var next = _queue.Peek();

                if (next.IsCanceled)
                {
                    _queue.Dequeue();
                }
                else
                {
                    return next;
                }
            }

            return null;
        }

        /// <summary>
        /// Schedules an action to be executed at <paramref name="dueTime"/>.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable ScheduleAbsolute<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var si = default(ScheduledItem<DateTimeOffset, TState>);

            var run = new Func<IScheduler, TState, IDisposable>((scheduler, state1) =>
            {
                _queue.Remove(si);
                return action(scheduler, state1);
            });

            si = new ScheduledItem<DateTimeOffset, TState>(this, state, run, dueTime, Comparer);
            _queue.Enqueue(si);

            return si;
        }
    }
}
