// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Scheduler with support for starting long-running tasks.
    /// This type of scheduler can be used to run loops more efficiently instead of using recursive scheduling.
    /// </summary>
    public interface ISchedulerLongRunning
    {
        /// <summary>
        /// Schedules a long-running piece of work.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <remarks>
        /// <para><b>Notes to implementers</b></para>
        /// The returned disposable object should not prevent the work from starting, but only set the cancellation flag passed to the specified action.
        /// </remarks>
        IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action);
    }
}