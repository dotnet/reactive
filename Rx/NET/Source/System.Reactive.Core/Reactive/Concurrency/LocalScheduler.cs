// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Abstract base class for machine-local schedulers, using the local system clock for time-based operations.
    /// </summary>
    public abstract partial class LocalScheduler : IScheduler, IStopwatchProvider, IServiceProvider
    {
        /// <summary>
        /// Gets the scheduler's notion of current time.
        /// </summary>
        public virtual DateTimeOffset Now
        {
            get { return Scheduler.Now; }
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public virtual IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return Schedule(state, TimeSpan.Zero, action);
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        public abstract IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action);

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public virtual IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return Enqueue(this, state, dueTime, action);
        }

        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        /// <remarks>
        /// Platform-specific scheduler implementations should reimplement IStopwatchProvider to provide a more
        /// efficient IStopwatch implementation (if available).
        /// </remarks>
        public virtual IStopwatch StartStopwatch()
        {
            return ConcurrencyAbstractionLayer.Current.StartStopwatch();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetService(serviceType);
        }

        /// <summary>
        /// Discovers scheduler services by interface type. The base class implementation returns
        /// requested services for each scheduler interface implemented by the derived class. For
        /// more control over service discovery, derived types can override this method.
        /// </summary>
        /// <param name="serviceType">Scheduler service interface type to discover.</param>
        /// <returns>Object implementing the requested service, if available; null otherwise.</returns>
        protected virtual object GetService(Type serviceType)
        {
#if !NO_PERF
            if (serviceType == typeof(IStopwatchProvider))
                return this as IStopwatchProvider;
            else if (serviceType == typeof(ISchedulerLongRunning))
                return this as ISchedulerLongRunning;
            else if (serviceType == typeof(ISchedulerPeriodic))
                return this as ISchedulerPeriodic;
#endif

            return null;
        }
    }
}
