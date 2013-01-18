// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace System.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        /// <summary>
        /// Returns a scheduler that represents the original scheduler, without any of its interface-based optimizations (e.g. long running scheduling).
        /// </summary>
        /// <param name="scheduler">Scheduler to disable all optimizations for.</param>
        /// <returns>Proxy to the original scheduler but without any optimizations enabled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IScheduler DisableOptimizations(this IScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return new DisableOptimizationsScheduler(scheduler);
        }

        /// <summary>
        /// Returns a scheduler that represents the original scheduler, without the specified set of interface-based optimizations (e.g. long running scheduling).
        /// </summary>
        /// <param name="scheduler">Scheduler to disable the specified optimizations for.</param>
        /// <param name="optimizationInterfaces">Types of the optimization interfaces that have to be disabled.</param>
        /// <returns>Proxy to the original scheduler but without the specified optimizations enabled.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="optimizationInterfaces"/> is null.</exception>
        public static IScheduler DisableOptimizations(this IScheduler scheduler, params Type[] optimizationInterfaces)
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (optimizationInterfaces == null)
                throw new ArgumentNullException("optimizationInterfaces");

            return new DisableOptimizationsScheduler(scheduler, optimizationInterfaces);
        }

        /// <summary>
        /// Returns a scheduler that wraps the original scheduler, adding exception handling for scheduled actions.
        /// </summary>
        /// <typeparam name="TException">Type of the exception to check for.</typeparam>
        /// <param name="scheduler">Scheduler to apply an exception filter for.</param>
        /// <param name="handler">Handler that's run if an exception is caught. The exception will be rethrown if the handler returns false.</param>
        /// <returns>Wrapper around the original scheduler, enforcing exception handling.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="handler"/> is null.</exception>
        public static IScheduler Catch<TException>(this IScheduler scheduler, Func<TException, bool> handler)
            where TException : Exception
        {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            if (handler == null)
                throw new ArgumentNullException("handler");

            return new CatchScheduler<TException>(scheduler, handler);
        }
    }
}
