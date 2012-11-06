// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + FromEventPattern +

        #region Strongly typed

        #region Action<EventHandler>

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern(addHandler, removeHandler);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern(addHandler, removeHandler);
        }
#endif

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern(addHandler, removeHandler, scheduler);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern(addHandler, removeHandler, scheduler);
        }
#endif

        #endregion

        #region Action<TDelegate>

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on a supplied event delegate type, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on a supplied event delegate type, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler&lt;TEventArgs&gt;"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="conversion">A function used to convert the given event handler to a delegate compatible with the underlying .NET event. The resulting delegate is used in calls to the addHandler and removeHandler action parameters.</param>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversion"/> or <paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (conversion == null)
                throw new ArgumentNullException("conversion");
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler&lt;TEventArgs&gt;"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="conversion">A function used to convert the given event handler to a delegate compatible with the underlying .NET event. The resulting delegate is used in calls to the addHandler and removeHandler action parameters.</param>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversion"/> or <paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (conversion == null)
                throw new ArgumentNullException("conversion");
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on a supplied event delegate type with a strongly typed sender parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on a supplied event delegate type with a strongly typed sender parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TDelegate, TSender, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #endregion

        #region Action<EventHandler<TEventArgs>>

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler&lt;TEventArgs&gt;"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEventPattern<TEventArgs>(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event, conforming to the standard .NET event pattern based on <see cref="EventHandler&lt;TEventArgs&gt;"/>, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #endregion

        #endregion

        #region Reflection

        #region Instance events

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with an <see cref="EventArgs"/> parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(object target, string eventName)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern(target, eventName);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(object target, string eventName)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern(target, eventName);
        }
#endif

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with an <see cref="EventArgs"/> parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(object target, string eventName, IScheduler scheduler)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern(target, eventName, scheduler);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(object target, string eventName, IScheduler scheduler)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");
            
            return s_impl.FromEventPattern(target, eventName, scheduler);
        }
#endif

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern<TEventArgs>(target, eventName);
        }

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TEventArgs>(target, eventName, scheduler);
        }

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with a strongly typed sender and strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's first argument type is not assignable to TSender. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern<TSender, TEventArgs>(target, eventName);
        }

        /// <summary>
        /// Converts an instance .NET event, conforming to the standard .NET event pattern with a strongly typed sender and strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the target object type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="target">Object instance that exposes the event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's first argument type is not assignable to TSender. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TSender, TEventArgs>(target, eventName, scheduler);
        }

        #endregion

        #region Static events

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with an <see cref="EventArgs"/> parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(Type type, string eventName)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern(type, eventName);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern(type, eventName);
        }
#endif

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with an <see cref="EventArgs"/> parameter, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
#if !NO_EVENTARGS_CONSTRAINT
        public static IObservable<EventPattern<EventArgs>> FromEventPattern(Type type, string eventName, IScheduler scheduler)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern(type, eventName, scheduler);
        }
#else
        public static IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName, IScheduler scheduler)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern(type, eventName, scheduler);
        }
#endif

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern<TEventArgs>(type, eventName);
        }

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TEventArgs>(type, eventName, scheduler);
        }

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with a strongly typed sender and strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's first argument type is not assignable to TSender. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEventPattern, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEventPattern, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");

            return s_impl.FromEventPattern<TSender, TEventArgs>(type, eventName);
        }

        /// <summary>
        /// Converts a static .NET event, conforming to the standard .NET event pattern with a strongly typed sender and strongly typed event arguments, to an observable sequence.
        /// Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// Reflection is used to discover the event based on the specified type and the specified event name.
        /// For conversion of events that don't conform to the standard .NET event pattern, use any of the FromEvent overloads instead.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="type">Type that exposes the static event to convert.</param>
        /// <param name="eventName">Name of the event to convert.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> or <paramref name="eventName"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The event could not be found. -or- The event does not conform to the standard .NET event pattern. -or- The event's first argument type is not assignable to TSender. -or- The event's second argument type is not assignable to TEventArgs.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEventPattern calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEventPattern that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEventPattern"/>
        public static IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName, IScheduler scheduler)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEventPattern<TSender, TEventArgs>(type, eventName, scheduler);
        }

        #endregion

        #endregion

        #endregion

        #region + FromEvent +

        #region Action<TDelegate>

        /// <summary>
        /// Converts a .NET event to an observable sequence, using a conversion function to obtain the event delegate. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="conversion">A function used to convert the given event handler to a delegate compatible with the underlying .NET event. The resulting delegate is used in calls to the addHandler and removeHandler action parameters.</param>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversion"/> or <paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEvent, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEvent, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
                throw new ArgumentNullException("conversion");
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event to an observable sequence, using a conversion function to obtain the event delegate. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="conversion">A function used to convert the given event handler to a delegate compatible with the underlying .NET event. The resulting delegate is used in calls to the addHandler and removeHandler action parameters.</param>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversion"/> or <paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEvent that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            if (conversion == null)
                throw new ArgumentNullException("conversion");
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEvent<TDelegate, TEventArgs>(conversion, addHandler, removeHandler, scheduler);
        }

        /// <summary>
        /// Converts a .NET event to an observable sequence, using a supplied event delegate type. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEvent, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEvent, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a .NET event to an observable sequence, using a supplied event delegate type. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEvent that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEvent<TDelegate, TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #endregion

        #region Action<Action<TEventArgs>>

        /// <summary>
        /// Converts a generic Action-based .NET event to an observable sequence. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEvent, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEvent, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEvent<TEventArgs>(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts a generic Action-based .NET event to an observable sequence. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEvent that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler, IScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEvent<TEventArgs>(addHandler, removeHandler, scheduler);
        }

        #endregion

        #region Action<Action>

        /// <summary>
        /// Converts an Action-based .NET event to an observable sequence. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// The current <see cref="SynchronizationContext"/> is captured during the call to FromEvent, and is used to post add and remove handler invocations.
        /// This behavior ensures add and remove handler operations for thread-affine events are accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// If no SynchronizationContext is present at the point of calling FromEvent, add and remove handler invocations are made synchronously on the thread
        /// making the Subscribe or Dispose call, respectively.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions due to the free-threaded nature of Reactive Extensions. Doing so
        /// makes the captured SynchronizationContext predictable. This best practice also reduces clutter of bridging code inside queries, making the query expressions
        /// more concise and easier to understand.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");

            return s_impl.FromEvent(addHandler, removeHandler);
        }

        /// <summary>
        /// Converts an Action-based .NET event to an observable sequence. Each event invocation is surfaced through an OnNext message in the resulting sequence.
        /// For conversion of events conforming to the standard .NET event pattern, use any of the FromEventPattern overloads instead.
        /// </summary>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <param name="scheduler">The scheduler to run the add and remove event handler logic on.</param>
        /// <returns>The observable sequence that contains the event argument objects passed to the invocations of the underlying .NET event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// Add and remove handler invocations are made whenever the number of observers grows beyond zero.
        /// As such, an event handler may be shared by multiple simultaneously active observers, using a subject for multicasting.
        /// </para>
        /// <para>
        /// Add and remove handler invocations are run on the specified scheduler. This behavior allows add and remove handler operations for thread-affine events to be
        /// accessed from the same context, as required by some UI frameworks.
        /// </para>
        /// <para>
        /// It's recommended to lift FromEvent calls outside event stream query expressions. This best practice reduces clutter of bridging code inside queries,
        /// making the query expressions more concise and easier to understand. This has additional benefits for overloads of FromEvent that omit the IScheduler
        /// parameter. For more information, see the remarks section on those overloads.
        /// </para>
        /// </remarks>
        /// <seealso cref="Observable.ToEvent"/>
        public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler, IScheduler scheduler)
        {
            if (addHandler == null)
                throw new ArgumentNullException("addHandler");
            if (removeHandler == null)
                throw new ArgumentNullException("removeHandler");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.FromEvent(addHandler, removeHandler, scheduler);
        }

        #endregion

        #endregion
    }
}
