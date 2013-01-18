// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace System.Reactive.Concurrency
{
    //
    // NOTE: When adding interface-based optimizations here, ensure to add the type to the list of
    //       interface-based optimizations used by DisableOptimizations and the RawScheduler type.
    //
    public static partial class Scheduler
    {
        internal static Type[] OPTIMIZATIONS = new Type[] {
            typeof(ISchedulerLongRunning),
            typeof(IStopwatchProvider),
            typeof(ISchedulerPeriodic),
            /* update this list if new interface-based optimizations are added */
        };

        /// <summary>
        /// Returns the ISchedulerLongRunning implementation of the specified scheduler, or null if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the ISchedulerLongRunning implementation for.</param>
        /// <returns>The scheduler's ISchedulerLongRunning implementation if available; null otherwise.</returns>
        /// <remarks>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </remarks>
        public static ISchedulerLongRunning AsLongRunning(this IScheduler scheduler)
        {
            var svc = scheduler as IServiceProvider;
            if (svc != null)
                return (ISchedulerLongRunning)svc.GetService(typeof(ISchedulerLongRunning));

            return null;
        }

        /// <summary>
        /// Returns the IStopwatchProvider implementation of the specified scheduler, or null if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the IStopwatchProvider implementation for.</param>
        /// <returns>The scheduler's IStopwatchProvider implementation if available; null otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </para>
        /// <para>
        /// Consider using <see cref="Scheduler.StartStopwatch"/> in case a stopwatch is required, but use of emulation stopwatch based
        /// on the scheduler's clock is acceptable. Use of this method is recommended for best-effort use of the stopwatch provider
        /// scheduler service, where the caller falls back to not using stopwatches if this facility wasn't found.
        /// </para>
        /// </remarks>
        public static IStopwatchProvider AsStopwatchProvider(this IScheduler scheduler)
        {
            var svc = scheduler as IServiceProvider;
            if (svc != null)
                return (IStopwatchProvider)svc.GetService(typeof(IStopwatchProvider));

            return null;
        }

        /// <summary>
        /// Returns the IStopwatchProvider implementation of the specified scheduler, or null if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the IStopwatchProvider implementation for.</param>
        /// <returns>The scheduler's IStopwatchProvider implementation if available; null otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </para>
        /// <para>
        /// Consider using the Scheduler.SchedulePeriodic extension methods for IScheduler in case periodic scheduling is required and
        /// emulation of periodic behavior using other scheduler services is desirable. Use of this method is recommended for best-effort
        /// use of the periodic scheduling service, where the caller falls back to not using periodic scheduling if this facility wasn't
        /// found.
        /// </para>
        /// </remarks>
        public static ISchedulerPeriodic AsPeriodic(this IScheduler scheduler)
        {
            var svc = scheduler as IServiceProvider;
            if (svc != null)
                return (ISchedulerPeriodic)svc.GetService(typeof(ISchedulerPeriodic));

            return null;
        }
    }
}
