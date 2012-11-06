// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Globalization;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides a set of static properties to access commonly used schedulers.
    /// </summary>
    public static partial class Scheduler
    {
        // TODO - Review whether this is too eager.
        // Make first use of Scheduler trigger access to and initialization of the CAL.
        private static DefaultScheduler s_default = DefaultScheduler.Instance;

        /// <summary>
        /// Gets the current time according to the local machine's system clock.
        /// </summary>
        public static DateTimeOffset Now
        {
            get
            {
                return SystemClock.UtcNow;
            }
        }

        /// <summary>
        /// Normalizes the specified TimeSpan value to a positive value.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan value to normalize.</param>
        /// <returns>The specified TimeSpan value if it is zero or positive; otherwise, TimeSpan.Zero.</returns>
        public static TimeSpan Normalize(TimeSpan timeSpan)
        {
            if (timeSpan.Ticks < 0)
                return TimeSpan.Zero;
            return timeSpan;
        }

        /// <summary>
        /// Gets a scheduler that schedules work immediately on the current thread.
        /// </summary>
        public static ImmediateScheduler Immediate
        {
            get
            {
                return ImmediateScheduler.Instance;
            }
        }

        /// <summary>
        /// Gets a scheduler that schedules work as soon as possible on the current thread.
        /// </summary>
        public static CurrentThreadScheduler CurrentThread
        {
            get
            {
                return CurrentThreadScheduler.Instance;
            }
        }

        /// <summary>
        /// Gets a scheduler that schedules work on the platform's default scheduler.
        /// </summary>
        public static DefaultScheduler Default
        {
            get
            {
                return s_default;
            }
        }


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


        private static Lazy<IScheduler> s_threadPool = new Lazy<IScheduler>(() => Initialize("ThreadPool"));

        /// <summary>
        /// Gets a scheduler that schedules work on the thread pool.
        /// </summary>
        [Obsolete(Constants_Core.OBSOLETE_SCHEDULER_THREADPOOL)]
        public static IScheduler ThreadPool
        {
            get
            {
                return s_threadPool.Value;
            }
        }

        private static Lazy<IScheduler> s_newThread = new Lazy<IScheduler>(() => Initialize("NewThread"));

        /// <summary>
        /// Gets a scheduler that schedules work on a new thread using default thread creation options.
        /// </summary>
        [Obsolete(Constants_Core.OBSOLETE_SCHEDULER_NEWTHREAD)]
        public static IScheduler NewThread
        {
            get
            {
                return s_newThread.Value;
            }
        }

#if !NO_TPL
        private static Lazy<IScheduler> s_taskPool = new Lazy<IScheduler>(() => Initialize("TaskPool"));

        /// <summary>
        /// Gets a scheduler that schedules work on Task Parallel Library (TPL) task pool using the default TaskScheduler.
        /// </summary>
        [Obsolete(Constants_Core.OBSOLETE_SCHEDULER_TASKPOOL)]
        public static IScheduler TaskPool
        {
            get
            {
                return s_taskPool.Value;
            }
        }
#endif

        private static IScheduler Initialize(string name)
        {
            var res = PlatformEnlightenmentProvider.Current.GetService<IScheduler>(name);
            if (res == null)
                throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings_Core.CANT_OBTAIN_SCHEDULER, name));
            return res;
        }
    }
}
