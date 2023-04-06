// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Controls how completion or failure is handled when a <see cref="Task"/> or
    /// <see cref="Task{TResult}"/> is wrapped as an <see cref="IObservable{T}"/> and observed by
    /// an <see cref="IObserver{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type can be passed to overloads of the various method that adapt a TPL task as an
    /// <see cref="IObservable{T}"/>. It deals with two concerns that arise whenever this is done:
    /// the scheduler through which notifications are delivered, and the handling of exceptions
    /// that occur after all observers have unsubscribed.
    /// </para>
    /// <para>
    /// If the <see cref="Scheduler"/> property is non-null, it will be used to deliver all
    /// notifications to observers, whether those notifications occur immediately (because the task
    /// had already finished by the time it was observed) or they happen later.
    /// </para>
    /// <para>
    /// The <see cref="IgnoreExceptionsAfterUnsubscribe"/> property determines how to deal tasks
    /// that fail after unsubscription (i.e., if an application calls <see cref="IObservable{T}.Subscribe(IObserver{T})"/>
    /// on an observable wrapping, then calls Dispose on the result before that task completes, and
    /// the task subsequently enters a faulted state). Overloads that don't take a <see cref="TaskObservationOptions"/>
    /// argument do not observe the <see cref="Task.Exception"/> in this case, with the result that
    /// the exception will then emerge from <see cref="TaskScheduler.UnobservedTaskException"/>
    /// (which could terminate the process, depending on how the .NET application has been
    /// configured). This is consistent with how unobserved <see cref="Task"/> failures are
    /// normally handled, but it is not consistent with how Rx handles post-unsubcription failures
    /// in general. For example, if the projection callback for Select is in progress at the moment
    /// an observer unsubscribes, and that callback then goes on to throw an exception, that
    /// exception is simply swallowed. (One could argue that it should instead be sent to some
    /// application-level unhandled exception handler, but the current behaviour has been in place
    /// for well over a decade, so it's not something we can change.) So there is an argument that
    /// post-unsubscribe failures in <see cref="IObservable{T}"/>-wrapped tasks should be
    /// ignored in exactly the same way: the default behaviour for post-unsubscribe failures in
    /// tasks is inconsistent with the handling of all other post-unsubscribe failures. This has
    /// also been the case for over a decade, so that inconsistency of defaults cannot be changed,
    /// but the <see cref="IgnoreExceptionsAfterUnsubscribe"/> property enables applications to
    /// ask for task-originated post-unsubscribe exceptions to be ignored in the same way as
    /// non-task-originated post-unsubscribe exceptions are. (Where possible, applications should
    /// avoid getting into situations where they throw exceptions in scenarios where nothing is
    /// able to observe them is. This setting is a last resort for situations in which this is
    /// truly unavoidable.)
    /// </para>
    /// </remarks>
    public struct TaskObservationOptions
    {
        public TaskObservationOptions(
            IScheduler? scheduler,
            bool ignoreExceptionsAfterUnsubscribe)
        {
            Scheduler = scheduler;
            IgnoreExceptionsAfterUnsubscribe = ignoreExceptionsAfterUnsubscribe;
        }

        /// <summary>
        /// Gets the optional scheduler to use when delivering notifications of the tasks's
        /// progress.
        /// </summary>
        /// <remarks>
        /// If this is null, the behaviour depends on whether the task has already completed. If
        /// the task has finished, the relevant completion or error notifications will be delivered
        /// via <see cref="ImmediateScheduler.Instance"/>. If the task is still running (or not yet
        /// started) at the instant at which it is observed through Rx, no scheduler will be used
        /// if this property is null.
        /// </remarks>
        public IScheduler? Scheduler { get; }

        /// <summary>
        /// Gets a flag controlling handling of exceptions that occur after cancellation
        /// has been initiated by unsubscribing from the observable representing the task's
        /// progress.
        /// </summary>
        /// <remarks>
        /// If this is <c>true</c>, exceptions that occur after all observers have unsubscribed
        /// will be handled and silently ignored. If <c>false</c>, they will go unobserved, meaning
        /// they will eventually emerge through <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </remarks>
        public bool IgnoreExceptionsAfterUnsubscribe { get; }
    }
}
