// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
        /// Returns the <see cref="ISchedulerLongRunning"/> implementation of the specified scheduler, or <c>null</c> if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the <see cref="ISchedulerLongRunning"/> implementation for.</param>
        /// <returns>The scheduler's <see cref="ISchedulerLongRunning"/> implementation if available; <c>null</c> otherwise.</returns>
        /// <remarks>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </remarks>
        public static ISchedulerLongRunning AsLongRunning(this IScheduler scheduler) => As<ISchedulerLongRunning>(scheduler);

        /// <summary>
        /// Returns the <see cref="IStopwatchProvider"/> implementation of the specified scheduler, or <c>null</c> if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the <see cref="IStopwatchProvider"/> implementation for.</param>
        /// <returns>The scheduler's <see cref="IStopwatchProvider"/> implementation if available; <c>null</c> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </para>
        /// <para>
        /// Consider using <see cref="StartStopwatch"/> in case a stopwatch is required, but use of emulation stopwatch based
        /// on the scheduler's clock is acceptable. Use of this method is recommended for best-effort use of the stopwatch provider
        /// scheduler service, where the caller falls back to not using stopwatches if this facility wasn't found.
        /// </para>
        /// </remarks>
        public static IStopwatchProvider AsStopwatchProvider(this IScheduler scheduler) => As<IStopwatchProvider>(scheduler);

        /// <summary>
        /// Returns the <see cref="ISchedulerPeriodic"/> implementation of the specified scheduler, or <c>null</c> if no such implementation is available.
        /// </summary>
        /// <param name="scheduler">Scheduler to get the <see cref="ISchedulerPeriodic"/> implementation for.</param>
        /// <returns>The scheduler's <see cref="ISchedulerPeriodic"/> implementation if available; <c>null</c> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// This helper method is made available for query operator authors in order to discover scheduler services by using the required
        /// IServiceProvider pattern, which allows for interception or redefinition of scheduler services.
        /// </para>
        /// <para>
        /// Consider using the <see cref="SchedulePeriodic"/> extension methods for <see cref="IScheduler"/> in case periodic scheduling
        /// is required and emulation of periodic behavior using other scheduler services is desirable. Use of this method is recommended
        /// for best-effort use of the periodic scheduling service, where the caller falls back to not using periodic scheduling if this
        /// facility wasn't found.
        /// </para>
        /// </remarks>
        public static ISchedulerPeriodic AsPeriodic(this IScheduler scheduler) => As<ISchedulerPeriodic>(scheduler);

        private static T As<T>(IScheduler scheduler)
            where T : class
        {
            if (scheduler is IServiceProvider svc)
            {
                return (T)svc.GetService(typeof(T));
            }

            return null;
        }
    }
}
