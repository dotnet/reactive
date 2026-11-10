// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of asynchronous work.
    /// </summary>
    /// <remarks>
    /// <para>
    /// As with <c>IScheduler</c> in Rx.NET, every method takes a <c>state</c> argument
    /// that is passed back to the scheduled action. This lets callers supply a static (non-capturing)
    /// delegate and hand it whatever it needs through the state, so a scheduling call need not
    /// allocate a closure and a delegate instance. Callers that do not need this can use the
    /// stateless overloads that <see cref="AsyncScheduler"/> provides as extension methods.
    /// </para>
    /// <para>
    /// The scheduled action receives a <see cref="CancellationToken"/> that is signalled if the
    /// <see cref="IAsyncDisposable"/> returned from the scheduling call is disposed.
    /// </para>
    /// <para>
    /// A scheduling call must not run any other caller's work, or the continuation of any other
    /// caller's task, inline before it returns. A scheduler may run the action it has just been
    /// given synchronously (as <see cref="ImmediateAsyncScheduler"/> does), but it must not use
    /// the scheduling thread to run work that other callers have queued or are awaiting. The
    /// library's operators schedule work from inside their critical sections and part way
    /// through delivering notifications, and they rely on the principle that the consumer of a
    /// notification never completes its producer's task: a scheduler that completed another
    /// caller's task from within a scheduling call would make the scheduling caller that
    /// consumer, and the other caller's continuation could then wait for something the
    /// scheduling caller has yet to do, which is a deadlock. Implementers of custom schedulers
    /// must keep to this, for example by resuming waiters on the thread pool rather than inline.
    /// </para>
    /// </remarks>
    public interface IAsyncScheduler : IClock
    {
        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, Func<TState, CancellationToken, ValueTask> action);

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, TimeSpan dueTime, Func<TState, CancellationToken, ValueTask> action);

        /// <summary>
        /// Schedules an action to be executed at the specified absolute due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        ValueTask<IAsyncDisposable> ScheduleAsync<TState>(TState state, DateTimeOffset dueTime, Func<TState, CancellationToken, ValueTask> action);
    }
}
