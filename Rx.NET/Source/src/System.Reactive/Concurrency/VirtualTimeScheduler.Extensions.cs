// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides a set of extension methods for virtual time scheduling.
    /// </summary>
    public static class VirtualTimeSchedulerExtensions
    {
        /// <summary>
        /// Schedules an action to be executed at <paramref name="dueTime"/>.
        /// </summary>
        /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
        /// <typeparam name="TRelative">Relative time representation type.</typeparam>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleRelative<TAbsolute, TRelative>(this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TRelative dueTime, Action action)
            where TAbsolute : IComparable<TAbsolute>
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // As stated in Scheduler.Simple.cs,
            // an anonymous delegate will allow delegate caching.
            // Watch https://github.com/dotnet/roslyn/issues/5835 for compiler
            // support for caching delegates from method groups.
            return scheduler.ScheduleRelative(action, dueTime, (s, a) => Invoke(s, a));
        }

        /// <summary>
        /// Schedules an action to be executed at <paramref name="dueTime"/>.
        /// </summary>
        /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
        /// <typeparam name="TRelative">Relative time representation type.</typeparam>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAbsolute<TAbsolute, TRelative>(this VirtualTimeSchedulerBase<TAbsolute, TRelative> scheduler, TAbsolute dueTime, Action action)
            where TAbsolute : IComparable<TAbsolute>
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.ScheduleAbsolute(action, dueTime, (s, a) => Invoke(s, a));
        }

        private static IDisposable Invoke(IScheduler scheduler, Action action)
        {
            action();
            return Disposable.Empty;
        }
    }
}
