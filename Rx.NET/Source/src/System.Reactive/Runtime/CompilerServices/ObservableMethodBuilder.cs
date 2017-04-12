// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.ExceptionServices;
using System.Security;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Represents a builder for asynchronous methods that return a task-like <see cref="Observable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public struct ObservableMethodBuilder<T>
    {
        /// <summary>
        /// The compiler-generated asynchronous state machine representing the execution flow of the asynchronous
        /// method whose return type is a task-like <see cref="Observable{T}"/>.
        /// </summary>
        private IAsyncStateMachine _stateMachine;

        /// <summary>
        /// The underlying observable sequence representing the result produced by the asynchronous method.
        /// </summary>
        private IObservable<T> _result;

        /// <summary>
        /// Creates an instance of the <see cref="ObservableMethodBuilder{T}"/> struct.
        /// </summary>
        /// <returns>A new instance of the struct.</returns>
        public static ObservableMethodBuilder<T> Create() => default(ObservableMethodBuilder<T>);

        /// <summary>
        /// Begins running the builder with the associated state machine.
        /// </summary>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="stateMachine">The state machine instance, passed by reference.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stateMachine"/> is <c>null</c>.</exception>
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            if (stateMachine == null)
                throw new ArgumentNullException(nameof(stateMachine));

            stateMachine.MoveNext();
        }

        /// <summary>
        /// Associates the builder with the specified state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stateMachine"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The state machine was previously set.</exception>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            if (stateMachine == null)
                throw new ArgumentNullException(nameof(stateMachine));

            if (_stateMachine != null)
                throw new InvalidOperationException();

            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Marks the observable as successfully completed.
        /// </summary>
        /// <param name="result">The result to use to complete the observable sequence.</param>
        /// <exception cref="InvalidOperationException">The observable has already completed.</exception>
        public void SetResult(T result)
        {
            if (_result == null)
            {
                _result = Observable.Return<T>(result);
            }
            else
            {
                var subject = _result as AsyncSubject<T>;

                // NB: The IsCompleted is not protected by the subject's lock, so we could get a dirty read.
                //
                //     We can live with this limitation and merely put in this check to catch invalid
                //     manual usage for which behavior is undefined. In the compiler-generated code that
                //     interacts with the asynchronous method builder, no concurrent calls to the Set*
                //     methods should occur.

                if (subject == null || subject.IsCompleted)
                    throw new InvalidOperationException();

                subject.OnNext(result);
                subject.OnCompleted();
            }
        }

        /// <summary>
        /// Marks the observable as failed and binds the specified exception to the observable sequence.
        /// </summary>
        /// <param name="exception">The exception to bind to the observable sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The observable has already completed.</exception>
        public void SetException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (_result == null)
            {
                _result = Observable.Throw<T>(exception);
            }
            else
            {
                var subject = _result as AsyncSubject<T>;

                // NB: The IsCompleted is not protected by the subject's lock, so we could get a dirty read.
                //
                //     We can live with this limitation and merely put in this check to catch invalid
                //     manual usage for which behavior is undefined. In the compiler-generated code that
                //     interacts with the asynchronous method builder, no concurrent calls to the Set*
                //     methods should occur.

                if (subject == null || subject.IsCompleted)
                    throw new InvalidOperationException();

                subject.OnError(exception);
            }
        }

        /// <summary>
        /// Gets the observable sequence for this builder.
        /// </summary>
        public Observable<T> Task => new Observable<T>(_result ?? (_result = new AsyncSubject<T>()));

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">The awaiter.</param>
        /// <param name="stateMachine">The state machine.</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            try
            {
                if (_stateMachine == null)
                {
                    var ignored = Task; // NB: Ensure we have the observable backed by an async subject ready.

                    _stateMachine = stateMachine;
                    _stateMachine.SetStateMachine(_stateMachine);
                }

                // NB: Rx has historically not bothered with execution contexts, so we don't do it here either.

                awaiter.OnCompleted(_stateMachine.MoveNext);
            }
            catch (Exception ex)
            {
                // NB: Prevent reentrancy into the async state machine when an exception would be observed
                //     by the caller. This could cause concurrent execution of the async method. Instead,
                //     rethrow the exception elsewhere.

                Rethrow(ex);
            }
        }

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="awaiter">The awaiter.</param>
        /// <param name="stateMachine">The state machine.</param>
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            try
            {
                if (_stateMachine == null)
                {
                    var ignored = Task; // NB: Ensure we have the observable backed by an async subject ready.

                    _stateMachine = stateMachine;
                    _stateMachine.SetStateMachine(_stateMachine);
                }

                // NB: Rx has historically not bothered with execution contexts, so we don't do it here either.

                awaiter.UnsafeOnCompleted(_stateMachine.MoveNext);
            }
            catch (Exception ex)
            {
                // NB: Prevent reentrancy into the async state machine when an exception would be observed
                //     by the caller. This could cause concurrent execution of the async method. Instead,
                //     rethrow the exception elsewhere.

                Rethrow(ex);
            }
        }

        /// <summary>
        /// Rethrows an exception that was thrown from an awaiter's OnCompleted methods.
        /// </summary>
        /// <param name="exception">The exception to rethrow.</param>
        private static void Rethrow(Exception exception)
        {
            Scheduler.Default.Schedule(ExceptionDispatchInfo.Capture(exception), (state, recurse) => state.Throw());
        }
    }
}
