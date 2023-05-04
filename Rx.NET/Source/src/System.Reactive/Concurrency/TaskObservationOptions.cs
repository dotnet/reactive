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
    /// The <see cref="IgnoreExceptionsAfterUnsubscribe"/> property determines how to deal with tasks
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
    public sealed class TaskObservationOptions
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

        internal Value ToValue() => new(Scheduler, IgnoreExceptionsAfterUnsubscribe);

        /// <summary>
        /// Value-type representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The public API surface area for <see cref="TaskObservationOptions"/> is a class because
        /// using a value type would run into various issues. The type might appear in expression
        /// trees due to use of <see cref="System.Reactive.Linq.IQbservable{T}"/>, which limits us
        /// to a fairly old subset of C#. It means we can't use the <c>in</c> modifier on
        /// parameters, which in turn prevents us from passing options by reference, increasing the
        /// overhead of each method call. Also, options types such as this aren't normally value
        /// types, so it would be a curious design choice.
        /// </para>
        /// <para>
        /// The downside of using a class is that it entails an extra allocation. Since the feature
        /// for which this is designed (the ability to swallow unhandled exceptions thrown by tasks
        /// after unsubscription) is one we don't expect most applications to use, that shouldn't
        /// be a problem. However, to accommodate this feature, common code paths shared by various
        /// overloads need the information that a <see cref="TaskObservationOptions"/> holds. The
        /// easy approach would be to construct an instance of this type in overloads that don't
        /// take one as an argument. But that would be impose an additional allocation on code that
        /// doesn't want this new feature.
        /// </para>
        /// <para>
        /// So although we can't use a value type with <c>in</c> in public APIs dues to constraints
        /// on expression trees, we can do so internally. This type is a value-typed version of
        /// <see cref="TaskObservationOptions"/> enabling us to share code paths without forcing
        /// new allocations on existing code.
        /// </para>
        /// </remarks>
        internal readonly struct Value
        {
            internal Value(IScheduler? scheduler, bool ignoreExceptionsAfterUnsubscribe)
            {
                Scheduler = scheduler;
                IgnoreExceptionsAfterUnsubscribe = ignoreExceptionsAfterUnsubscribe;
            }

            public IScheduler? Scheduler { get; }
            public bool IgnoreExceptionsAfterUnsubscribe { get; }
        }
    }
}
