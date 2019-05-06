// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public abstract class VirtualTimeAsyncScheduler<TAbsolute, TRelative> : IAsyncScheduler
        where TAbsolute : IComparable<TAbsolute>
    {
        /// <summary>
        /// Creates a new virtual time scheduler with the default value of TAbsolute as the initial clock value.
        /// </summary>
        protected VirtualTimeAsyncScheduler()
            : this(default, Comparer<TAbsolute>.Default) { }

        /// <summary>
        /// Creates a new virtual time scheduler with the specified initial clock value and absolute time comparer.
        /// </summary>
        /// <param name="initialClock">Initial value for the clock.</param>
        /// <param name="comparer">Comparer to determine causality of events based on absolute time.</param>
        /// <exception cref="ArgumentNullException"><paramref name="comparer"/> is <c>null</c>.</exception>
        protected VirtualTimeAsyncScheduler(TAbsolute initialClock, IComparer<TAbsolute> comparer)
        {
            Clock = initialClock;
            Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// Gets the scheduler's notion of current time.
        /// </summary>
        public DateTimeOffset Now => ToDateTimeOffset(Clock);

        /// <summary>
        /// Gets the scheduler's absolute time clock value.
        /// </summary>
        public TAbsolute Clock { get; protected set; }

        /// <summary>
        /// Gets the comparer used to compare absolute time values.
        /// </summary>
        protected IComparer<TAbsolute> Comparer { get; }

        /// <summary>
        /// Gets whether the scheduler is enabled to run work.
        /// </summary>
        public bool IsEnabled { get; private set; }



        /// <summary>
        /// Adds a relative time value to an absolute time value.
        /// </summary>
        /// <param name="absolute">Absolute time value.</param>
        /// <param name="relative">Relative time value to add.</param>
        /// <returns>The resulting absolute time sum value.</returns>
        protected abstract TAbsolute Add(TAbsolute absolute, TRelative relative);

        /// <summary>
        /// Converts the absolute time value to a DateTimeOffset value.
        /// </summary>
        /// <param name="absolute">Absolute time value to convert.</param>
        /// <returns>The corresponding DateTimeOffset value.</returns>
        protected abstract DateTimeOffset ToDateTimeOffset(TAbsolute absolute);

        /// <summary>
        /// Converts the TimeSpan value to a relative time value.
        /// </summary>
        /// <param name="timeSpan">TimeSpan value to convert.</param>
        /// <returns>The corresponding relative time value.</returns>
        protected abstract TRelative ToRelative(TimeSpan timeSpan);

        /// <summary>
        /// Gets the next scheduled item to be executed.
        /// </summary>
        /// <returns>The next scheduled item.</returns>
        internal abstract Task<ScheduledItem<TAbsolute>> GetNext();

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        public abstract Task<IAsyncDisposable> ScheduleAbsolute(TAbsolute dueTime, Func<CancellationToken, Task> action);

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        public Task<IAsyncDisposable> ScheduleRelative(TRelative dueTime, Func<CancellationToken, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var runAt = Add(Clock, dueTime);
            return ScheduleAbsolute(runAt, action);
        }

        public Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action) => ScheduleAbsolute(Clock, action);

        public Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action, TimeSpan dueTime) => ScheduleRelative(ToRelative(dueTime), action);

        public Task<IAsyncDisposable> ScheduleAsync(Func<CancellationToken, Task> action, DateTimeOffset dueTime) => ScheduleRelative(ToRelative(dueTime - Now), action);

        /// <summary>
        /// Starts the virtual time scheduler.
        /// </summary>
        public async Task Start()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
                do
                {
                    var next = await GetNext().ConfigureAwait(false);
                    if (next != null)
                    {
                        if (Comparer.Compare(next.DueTime, Clock) > 0)
                        {
                            Clock = next.DueTime;
                        }

                        await next.Invoke().ConfigureAwait(false);
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                } while (IsEnabled);
            }
        }

        /// <summary>
        /// Stops the virtual time scheduler.
        /// </summary>
        public void Stop()
        {
            IsEnabled = false;
        }
    }
}
