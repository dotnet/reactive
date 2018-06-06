// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Scheduler with support for running periodic tasks.
    /// This type of scheduler can be used to run timers more efficiently instead of using recursive scheduling.
    /// </summary>
    public interface ISchedulerPeriodic
    {
        /// <summary>
        /// Schedules a periodic piece of work.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action);
    }
}
