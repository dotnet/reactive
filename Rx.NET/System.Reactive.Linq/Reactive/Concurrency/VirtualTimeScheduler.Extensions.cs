// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides a set of extension methods for virtual time scheduling.
    /// </summary>
    public static class VirtualTimeSchedulerExtensions
    {
        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
        /// <typeparam name="TRelative">Relative time representation type.</typeparam>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        public static IDisposable ScheduleRelative<TAbsolute, TRelative>(this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TRelative dueTime, Action action)
            where TAbsolute : IComparable<TAbsolute>
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (action == null)
                throw new ArgumentNullException("action");

            return scheduler.ScheduleRelative(action, dueTime, Invoke);
        }

        /// <summary>
        /// Schedules an action to be executed at dueTime.
        /// </summary>
        /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
        /// <typeparam name="TRelative">Relative time representation type.</typeparam>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is null.</exception>
        public static IDisposable ScheduleAbsolute<TAbsolute, TRelative>(this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TAbsolute dueTime, Action action)
            where TAbsolute : IComparable<TAbsolute>
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (action == null)
                throw new ArgumentNullException("action");

            return scheduler.ScheduleAbsolute(action, dueTime, Invoke);
        }

        static IDisposable Invoke(IScheduler scheduler, Action action)
        {
            action();
            return Disposable.Empty;
        }
    }
}
