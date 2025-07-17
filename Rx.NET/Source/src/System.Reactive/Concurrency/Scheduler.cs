﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Globalization;
using System.Reactive.PlatformServices;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides a set of static properties to access commonly used schedulers.
    /// </summary>
    public static partial class Scheduler
    {
        // TODO - Review whether this is too eager.
        // Make first use of Scheduler trigger access to and initialization of the CAL.

        // HACK: Causes race condition with Locks in DefaultScheduler's static ctor chain
        // private static DefaultScheduler s_default = DefaultScheduler.Instance;

        /// <summary>
        /// Gets the current time according to the local machine's system clock.
        /// </summary>
        public static DateTimeOffset Now => SystemClock.UtcNow;

        /// <summary>
        /// Normalizes the specified <see cref="TimeSpan"/> value to a positive value.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> value to normalize.</param>
        /// <returns>The specified TimeSpan value if it is zero or positive; otherwise, <see cref="TimeSpan.Zero"/>.</returns>
        public static TimeSpan Normalize(TimeSpan timeSpan) => timeSpan.Ticks < 0 ? TimeSpan.Zero : timeSpan;

        /// <summary>
        /// Gets a scheduler that schedules work immediately on the current thread.
        /// </summary>
        public static ImmediateScheduler Immediate => ImmediateScheduler.Instance;

        /// <summary>
        /// Gets a scheduler that schedules work as soon as possible on the current thread.
        /// </summary>
        public static CurrentThreadScheduler CurrentThread => CurrentThreadScheduler.Instance;

        /// <summary>
        /// Gets a scheduler that schedules work on the platform's default scheduler.
        /// </summary>
        public static DefaultScheduler Default => DefaultScheduler.Instance;


        //
        // Notice we include all of the scheduler properties below, unconditionally. In Rx v2.0
        // beta and RC, we limited this a la carte menu to reflect the platform's capabilities.
        // However, this caused different builds for Windows 8, .NET 4.5, and Portable Library
        // to be required. In the RTM timeframe, we opted for unifying all of this based on a
        // single Portable Library build of the core set of assemblies. As such, we're presented
        // with a choice of either locking down those properties to the intersection, or keeping
        // compatibility for those who upgrade from.NET 4.0 to .NET 4.5. We chose the latter, so
        // we need to keep properties like NewThread here, even though they'll be obsolete from
        // day 0 of Rx v2.0 (including our Portable Library story). Also, the NewThread one will
        // be non-functional for Windows 8, causing a runtime exception to be thrown.
        //


        private static readonly Lazy<IScheduler> _threadPool = new(static () => Initialize("ThreadPool"));

        /// <summary>
        /// Gets a scheduler that schedules work on the thread pool.
        /// </summary>
        [Obsolete(Constants_Core.ObsoleteSchedulerThreadpool)]
        public static IScheduler ThreadPool => _threadPool.Value;

        private static readonly Lazy<IScheduler> _newThread = new(static () => Initialize("NewThread"));

        /// <summary>
        /// Gets a scheduler that schedules work on a new thread using default thread creation options.
        /// </summary>
        [Obsolete(Constants_Core.ObsoleteSchedulerNewthread)]
        public static IScheduler NewThread => _newThread.Value;

        private static readonly Lazy<IScheduler> _taskPool = new(static () => Initialize("TaskPool"));

        /// <summary>
        /// Gets a scheduler that schedules work on Task Parallel Library (TPL) task pool using the default TaskScheduler.
        /// </summary>
        [Obsolete(Constants_Core.ObsoleteSchedulerTaskpool)]
        public static IScheduler TaskPool => _taskPool.Value;

        private static IScheduler Initialize(string name)
        {
            return PlatformEnlightenmentProvider.Current.GetService<IScheduler>(name)
                ?? throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings_Core.CANT_OBTAIN_SCHEDULER, name));
        }
    }
}
