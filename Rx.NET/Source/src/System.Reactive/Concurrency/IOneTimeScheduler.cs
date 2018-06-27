// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Indicates the Scheduler can support 
    /// so-called one-time tasks (immediate or delayed)
    /// that don't require the ability to schedule
    /// recursively.
    /// </summary>
    internal interface IOneTimeScheduler
    {
        /// <summary>
        /// Schedule the immediate execution of an action with the
        /// given associated state.
        /// </summary>
        /// <typeparam name="TState">The type of the state instance.</typeparam>
        /// <param name="state">The state to be handed to the action when run
        /// by this scheduler.</param>
        /// <param name="action">The action to execute on this scheduler.</param>
        /// <returns>The disposable instance that allows canceling the action before it runs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        IDisposable ScheduleAction<TState>(TState state, Action<TState> action);

        /// <summary>
        /// Schedule the delayed execution of an action
        /// with the given associated state.
        /// </summary>
        /// <typeparam name="TState">The type of the state instance.</typeparam>
        /// <param name="state">The state to be handed to the action when run
        /// by this scheduler.</param>
        /// <param name="dueTime">The time delay before the action executes.</param>
        /// <param name="action">The action to execute on this scheduler.</param>
        /// <returns>The disposable instance that allows canceling the action before it runs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        IDisposable ScheduleAction<TState>(TState state, TimeSpan dueTime, Action<TState> action);
    }
}
