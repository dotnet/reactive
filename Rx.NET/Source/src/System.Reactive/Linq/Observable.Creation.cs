﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Create +

        /// <summary>
        /// Creates an observable sequence from a specified Subscribe method implementation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribe">Implementation of the resulting observable sequence's Subscribe method.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is null.</exception>
        /// <remarks>
        /// Use of this operator is preferred over manual implementation of the <see cref="IObservable{T}"/> interface. In case
        /// you need a type implementing <see cref="IObservable{T}"/> rather than an anonymous implementation, consider using
        /// the <see cref="ObservableBase{T}"/> abstract base class.
        /// </remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, IDisposable> subscribe)
        {
            if (subscribe == null)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return s_impl.Create(subscribe);
        }

        /// <summary>
        /// Creates an observable sequence from a specified Subscribe method implementation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribe">Implementation of the resulting observable sequence's Subscribe method, returning an Action delegate that will be wrapped in an IDisposable.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is null.</exception>
        /// <remarks>
        /// Use of this operator is preferred over manual implementation of the <see cref="IObservable{T}"/> interface. In case
        /// you need a type implementing <see cref="IObservable{T}"/> rather than an anonymous implementation, consider using
        /// the <see cref="ObservableBase{T}"/> abstract base class.
        /// </remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Action> subscribe)
        {
            if (subscribe == null)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return s_impl.Create(subscribe);
        }

        #endregion

        #region + CreateAsync +

        /// <summary>
        /// Creates an observable sequence from a specified cancellable asynchronous Subscribe method.
        /// The CancellationToken passed to the asynchronous Subscribe method is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to produce elements.</param>
        /// <returns>The observable sequence surfacing the elements produced by the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous subscribe function will be signaled.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        /// <summary>
        /// Creates an observable sequence from a specified asynchronous Subscribe method.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to produce elements.</param>
        /// <returns>The observable sequence surfacing the elements produced by the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        /// <summary>
        /// Creates an observable sequence from a specified cancellable asynchronous Subscribe method.
        /// The CancellationToken passed to the asynchronous Subscribe method is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to implement the resulting sequence's Subscribe method.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous subscribe function will be signaled.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<IDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        /// <summary>
        /// Creates an observable sequence from a specified asynchronous Subscribe method.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to implement the resulting sequence's Subscribe method.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<IDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        /// <summary>
        /// Creates an observable sequence from a specified cancellable asynchronous Subscribe method.
        /// The CancellationToken passed to the asynchronous Subscribe method is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to implement the resulting sequence's Subscribe method, returning an Action delegate that will be wrapped in an IDisposable.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous subscribe function will be signaled.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<Action>> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        /// <summary>
        /// Creates an observable sequence from a specified asynchronous Subscribe method.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="subscribeAsync">Asynchronous method used to implement the resulting sequence's Subscribe method, returning an Action delegate that will be wrapped in an IDisposable.</param>
        /// <returns>The observable sequence with the specified implementation for the Subscribe method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subscribeAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<Action>> subscribeAsync)
        {
            if (subscribeAsync == null)
            {
                throw new ArgumentNullException(nameof(subscribeAsync));
            }

            return s_impl.Create(subscribeAsync);
        }

        #endregion

        #region + Defer +

        /// <summary>
        /// Returns an observable sequence that invokes the specified factory function whenever a new observer subscribes.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="observableFactory">Observable factory function to invoke for each observer that subscribes to the resulting sequence.</param>
        /// <returns>An observable sequence whose observers trigger an invocation of the given observable factory function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observableFactory"/> is null.</exception>
        public static IObservable<TResult> Defer<TResult>(Func<IObservable<TResult>> observableFactory)
        {
            if (observableFactory == null)
            {
                throw new ArgumentNullException(nameof(observableFactory));
            }

            return s_impl.Defer(observableFactory);
        }

        #endregion

        #region + DeferAsync +

        /// <summary>
        /// Returns an observable sequence that starts the specified asynchronous factory function whenever a new observer subscribes.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="observableFactoryAsync">Asynchronous factory function to start for each observer that subscribes to the resulting sequence.</param>
        /// <returns>An observable sequence whose observers trigger the given asynchronous observable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observableFactoryAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IObservable<TResult> Defer<TResult>(Func<Task<IObservable<TResult>>> observableFactoryAsync)
        {
            return Defer(observableFactoryAsync, ignoreExceptionsAfterUnsubscribe: false);
        }

        /// <summary>
        /// Returns an observable sequence that starts the specified asynchronous factory function whenever a new observer subscribes.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="observableFactoryAsync">Asynchronous factory function to start for each observer that subscribes to the resulting sequence.</param>
        /// <param name="ignoreExceptionsAfterUnsubscribe">
        /// If true, exceptions that occur after cancellation has been initiated by unsubscribing from the observable
        /// this method returns will be handled and silently ignored. If false, they will go unobserved, meaning they
        /// will eventually emerge through <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </param>
        /// <returns>An observable sequence whose observers trigger the given asynchronous observable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observableFactoryAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static IObservable<TResult> Defer<TResult>(Func<Task<IObservable<TResult>>> observableFactoryAsync, bool ignoreExceptionsAfterUnsubscribe)
        {
            if (observableFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(observableFactoryAsync));
            }

            return s_impl.Defer(observableFactoryAsync, ignoreExceptionsAfterUnsubscribe);
        }

        /// <summary>
        /// Returns an observable sequence that starts the specified cancellable asynchronous factory function whenever a new observer subscribes.
        /// The CancellationToken passed to the asynchronous factory function is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="observableFactoryAsync">Asynchronous factory function to start for each observer that subscribes to the resulting sequence.</param>
        /// <returns>An observable sequence whose observers trigger the given asynchronous observable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observableFactoryAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous observable factory function will be signaled.</remarks>
        public static IObservable<TResult> DeferAsync<TResult>(Func<CancellationToken, Task<IObservable<TResult>>> observableFactoryAsync)
        {
            return DeferAsync(observableFactoryAsync, ignoreExceptionsAfterUnsubscribe: false);
        }

        /// <summary>
        /// Returns an observable sequence that starts the specified cancellable asynchronous factory function whenever a new observer subscribes.
        /// The CancellationToken passed to the asynchronous factory function is tied to the returned disposable subscription, allowing best-effort cancellation.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the sequence returned by the factory function, and in the resulting sequence.</typeparam>
        /// <param name="observableFactoryAsync">Asynchronous factory function to start for each observer that subscribes to the resulting sequence.</param>
        /// <param name="ignoreExceptionsAfterUnsubscribe">
        /// If true, exceptions that occur after cancellation has been initiated by unsubscribing from the observable
        /// this method returns will be handled and silently ignored. If false, they will go unobserved, meaning they
        /// will eventually emerge through <see cref="TaskScheduler.UnobservedTaskException"/>.
        /// </param>
        /// <returns>An observable sequence whose observers trigger the given asynchronous observable factory function to be started.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observableFactoryAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous observable factory function will be signaled.</remarks>
        public static IObservable<TResult> DeferAsync<TResult>(Func<CancellationToken, Task<IObservable<TResult>>> observableFactoryAsync, bool ignoreExceptionsAfterUnsubscribe)
        {
            if (observableFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(observableFactoryAsync));
            }

            return s_impl.Defer(observableFactoryAsync, ignoreExceptionsAfterUnsubscribe);
        }

        #endregion

        #region + Empty +

        /// <summary>
        /// Returns an empty observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <returns>An observable sequence with no elements.</returns>
        public static IObservable<TResult> Empty<TResult>()
        {
            return s_impl.Empty<TResult>();
        }

        /// <summary>
        /// Returns an empty observable sequence.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="TResult"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>An observable sequence with no elements.</returns>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required for type inference
        public static IObservable<TResult> Empty<TResult>(TResult witness)
#pragma warning restore IDE0060
        {
            return s_impl.Empty<TResult>(); // Pure inference - no specialized target method.
        }

        /// <summary>
        /// Returns an empty observable sequence, using the specified scheduler to send out the single OnCompleted message.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="scheduler">Scheduler to send the termination call on.</param>
        /// <returns>An observable sequence with no elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Empty<TResult>(IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Empty<TResult>(scheduler);
        }

        /// <summary>
        /// Returns an empty observable sequence, using the specified scheduler to send out the single OnCompleted message.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="scheduler">Scheduler to send the termination call on.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="TResult"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>An observable sequence with no elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required for type inference
        public static IObservable<TResult> Empty<TResult>(IScheduler scheduler, TResult witness)
#pragma warning restore IDE0060
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Empty<TResult>(scheduler); // Pure inference - no specialized target method.
        }

        #endregion

        #region + Generate +

        /// <summary>
        /// Generates an observable sequence by running a state-driven loop producing the sequence's elements.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (iterate == null)
            {
                throw new ArgumentNullException(nameof(iterate));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.Generate(initialState, condition, iterate, resultSelector);
        }

        /// <summary>
        /// Generates an observable sequence by running a state-driven loop producing the sequence's elements, using the specified scheduler to send out observer messages.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (iterate == null)
            {
                throw new ArgumentNullException(nameof(iterate));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Generate(initialState, condition, iterate, resultSelector, scheduler);
        }

        #endregion

        #region + Never +

        /// <summary>
        /// Returns a non-terminating observable sequence, which can be used to denote an infinite duration (e.g. when using reactive joins).
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <returns>An observable sequence whose observers will never get called.</returns>
        public static IObservable<TResult> Never<TResult>()
        {
            return s_impl.Never<TResult>();
        }

        /// <summary>
        /// Returns a non-terminating observable sequence, which can be used to denote an infinite duration (e.g. when using reactive joins).
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="TResult"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>An observable sequence whose observers will never get called.</returns>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required for type inference
        public static IObservable<TResult> Never<TResult>(TResult witness)
#pragma warning restore IDE0060
        {
            return s_impl.Never<TResult>(); // Pure inference - no specialized target method.
        }

        #endregion

        #region + Range +

        /// <summary>
        /// Generates an observable sequence of integral numbers within a specified range.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <returns>An observable sequence that contains a range of sequential integral numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero. -or- <paramref name="start"/> + <paramref name="count"/> - 1 is larger than <see cref="int.MaxValue"/>.</exception>
        public static IObservable<int> Range(int start, int count)
        {
            var max = (long)start + count - 1;
            if (count < 0 || max > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.Range(start, count);
        }

        /// <summary>
        /// Generates an observable sequence of integral numbers within a specified range, using the specified scheduler to send out observer messages.
        /// </summary>
        /// <param name="start">The value of the first integer in the sequence.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        /// <param name="scheduler">Scheduler to run the generator loop on.</param>
        /// <returns>An observable sequence that contains a range of sequential integral numbers.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero. -or- <paramref name="start"/> + <paramref name="count"/> - 1 is larger than <see cref="int.MaxValue"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<int> Range(int start, int count, IScheduler scheduler)
        {
            var max = (long)start + count - 1;
            if (count < 0 || max > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Range(start, count, scheduler);
        }

        #endregion

        #region + Repeat +

        /// <summary>
        /// Generates an observable sequence that repeats the given element infinitely.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be repeated in the produced sequence.</typeparam>
        /// <param name="value">Element to repeat.</param>
        /// <returns>An observable sequence that repeats the given element infinitely.</returns>
        public static IObservable<TResult> Repeat<TResult>(TResult value)
        {
            return s_impl.Repeat(value);
        }

        /// <summary>
        /// Generates an observable sequence that repeats the given element infinitely, using the specified scheduler to send out observer messages.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be repeated in the produced sequence.</typeparam>
        /// <param name="value">Element to repeat.</param>
        /// <param name="scheduler">Scheduler to run the producer loop on.</param>
        /// <returns>An observable sequence that repeats the given element infinitely.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Repeat(value, scheduler);
        }

        /// <summary>
        /// Generates an observable sequence that repeats the given element the specified number of times.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be repeated in the produced sequence.</typeparam>
        /// <param name="value">Element to repeat.</param>
        /// <param name="repeatCount">Number of times to repeat the element.</param>
        /// <returns>An observable sequence that repeats the given element the specified number of times.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="repeatCount"/> is less than zero.</exception>
        public static IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount)
        {
            if (repeatCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repeatCount));
            }

            return s_impl.Repeat(value, repeatCount);
        }

        /// <summary>
        /// Generates an observable sequence that repeats the given element the specified number of times, using the specified scheduler to send out observer messages.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be repeated in the produced sequence.</typeparam>
        /// <param name="value">Element to repeat.</param>
        /// <param name="repeatCount">Number of times to repeat the element.</param>
        /// <param name="scheduler">Scheduler to run the producer loop on.</param>
        /// <returns>An observable sequence that repeats the given element the specified number of times.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="repeatCount"/> is less than zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount, IScheduler scheduler)
        {
            if (repeatCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repeatCount));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Repeat(value, repeatCount, scheduler);
        }

        #endregion

        #region + Return +

        /// <summary>
        /// Returns an observable sequence that contains a single element.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be returned in the produced sequence.</typeparam>
        /// <param name="value">Single element in the resulting observable sequence.</param>
        /// <returns>An observable sequence containing the single specified element.</returns>
        public static IObservable<TResult> Return<TResult>(TResult value)
        {
            return s_impl.Return(value);
        }

        /// <summary>
        /// Returns an observable sequence that contains a single element, using the specified scheduler to send out observer messages.
        /// </summary>
        /// <typeparam name="TResult">The type of the element that will be returned in the produced sequence.</typeparam>
        /// <param name="value">Single element in the resulting observable sequence.</param>
        /// <param name="scheduler">Scheduler to send the single element on.</param>
        /// <returns>An observable sequence containing the single specified element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Return(value, scheduler);
        }

        #endregion

        #region + Throw +

        /// <summary>
        /// Returns an observable sequence that terminates with an exception.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="exception">Exception object used for the sequence's termination.</param>
        /// <returns>The observable sequence that terminates exceptionally with the specified exception object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
        public static IObservable<TResult> Throw<TResult>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return s_impl.Throw<TResult>(exception);
        }

        /// <summary>
        /// Returns an observable sequence that terminates with an exception.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="exception">Exception object used for the sequence's termination.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="TResult"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>The observable sequence that terminates exceptionally with the specified exception object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required for type inference
        public static IObservable<TResult> Throw<TResult>(Exception exception, TResult witness)
#pragma warning restore IDE0060
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return s_impl.Throw<TResult>(exception); // Pure inference - no specialized target method.
        }

        /// <summary>
        /// Returns an observable sequence that terminates with an exception, using the specified scheduler to send out the single OnError message.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="exception">Exception object used for the sequence's termination.</param>
        /// <param name="scheduler">Scheduler to send the exceptional termination call on.</param>
        /// <returns>The observable sequence that terminates exceptionally with the specified exception object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Throw<TResult>(exception, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that terminates with an exception, using the specified scheduler to send out the single OnError message.
        /// </summary>
        /// <typeparam name="TResult">The type used for the <see cref="IObservable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="exception">Exception object used for the sequence's termination.</param>
        /// <param name="scheduler">Scheduler to send the exceptional termination call on.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="TResult"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>The observable sequence that terminates exceptionally with the specified exception object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> or <paramref name="scheduler"/> is null.</exception>
#pragma warning disable IDE0060 // (Remove unused parameter.) Required for type inference
        public static IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler, TResult witness)
#pragma warning restore IDE0060
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Throw<TResult>(exception, scheduler); // Pure inference - no specialized target method.
        }

        #endregion

        #region + Using +

        /// <summary>
        /// Constructs an observable sequence that depends on a resource object, whose lifetime is tied to the resulting observable sequence's lifetime.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <typeparam name="TResource">The type of the resource used during the generation of the resulting sequence. Needs to implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="resourceFactory">Factory function to obtain a resource object.</param>
        /// <param name="observableFactory">Factory function to obtain an observable sequence that depends on the obtained resource.</param>
        /// <returns>An observable sequence whose lifetime controls the lifetime of the dependent resource object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceFactory"/> or <paramref name="observableFactory"/> is null.</exception>
        public static IObservable<TResult> Using<TResult, TResource>(Func<TResource> resourceFactory, Func<TResource, IObservable<TResult>> observableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
            {
                throw new ArgumentNullException(nameof(resourceFactory));
            }

            if (observableFactory == null)
            {
                throw new ArgumentNullException(nameof(observableFactory));
            }

            return s_impl.Using(resourceFactory, observableFactory);
        }

        #endregion

        #region + UsingAsync +

        /// <summary>
        /// Constructs an observable sequence that depends on a resource object, whose lifetime is tied to the resulting observable sequence's lifetime. The resource is obtained and used through asynchronous methods.
        /// The CancellationToken passed to the asynchronous methods is tied to the returned disposable subscription, allowing best-effort cancellation at any stage of the resource acquisition or usage.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <typeparam name="TResource">The type of the resource used during the generation of the resulting sequence. Needs to implement <see cref="IDisposable"/>.</typeparam>
        /// <param name="resourceFactoryAsync">Asynchronous factory function to obtain a resource object.</param>
        /// <param name="observableFactoryAsync">Asynchronous factory function to obtain an observable sequence that depends on the obtained resource.</param>
        /// <returns>An observable sequence whose lifetime controls the lifetime of the dependent resource object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceFactoryAsync"/> or <paramref name="observableFactoryAsync"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        /// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous resource factory and observable factory functions will be signaled.</remarks>
        public static IObservable<TResult> Using<TResult, TResource>(Func<CancellationToken, Task<TResource>> resourceFactoryAsync, Func<TResource, CancellationToken, Task<IObservable<TResult>>> observableFactoryAsync) where TResource : IDisposable
        {
            if (resourceFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(resourceFactoryAsync));
            }

            if (observableFactoryAsync == null)
            {
                throw new ArgumentNullException(nameof(observableFactoryAsync));
            }

            return s_impl.Using(resourceFactoryAsync, observableFactoryAsync);
        }

        #endregion
    }
}
