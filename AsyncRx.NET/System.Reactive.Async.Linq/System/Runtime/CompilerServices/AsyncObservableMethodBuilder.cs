// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Represents a builder for asynchronous methods that return a task-like <see cref="IAsyncObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public struct AsyncObservableMethodBuilder<T>
    {
        /// <summary>
        /// The compiler-generated asynchronous state machine representing the execution flow of the asynchronous
        /// method whose return type is a task-like <see cref="IAsyncObservable{T}"/>.
        /// </summary>
        private IAsyncStateMachine _stateMachine;

        /// <summary>
        /// The underlying observable sequence representing the result produced by the asynchronous method.
        /// </summary>
        private TaskObservable _inner;

        /// <summary>
        /// Creates an instance of the <see cref="AsyncObservableMethodBuilder{T}"/> struct.
        /// </summary>
        /// <returns>A new instance of the struct.</returns>
        public static AsyncObservableMethodBuilder<T> Create() => default(AsyncObservableMethodBuilder<T>);

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
            if (_inner == null)
            {
                _inner = new TaskObservable(result);
            }
            else
            {
                _inner.SetResult(result);
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

            if (_inner == null)
            {
                _inner = new TaskObservable(exception);
            }
            else
            {
                _inner.SetException(exception);
            }
        }

        /// <summary>
        /// Gets the observable sequence for this builder.
        /// </summary>
        public IAsyncObservable<T> Task => _inner ?? (_inner = new TaskObservable());

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
            TaskPoolAsyncScheduler.Default.ScheduleAsync(_ =>
            {
                ExceptionDispatchInfo.Capture(exception).Throw();

                return System.Threading.Tasks.Task.CompletedTask;
            });
        }

        /// <summary>
        /// Implementation of the IObservable&lt;T&gt; interface compatible with async method return types.
        /// </summary>
        /// <remarks>
        /// This class implements a "task-like" type that can be used as the return type of an asynchronous
        /// method in C# 7.0 and beyond. For example:
        /// <code>
        /// async Observable&lt;int&gt; RxAsync()
        /// {
        ///     var res = await Observable.Return(21).Delay(TimeSpan.FromSeconds(1));
        ///     return res * 2;
        /// }
        /// </code>
        /// </remarks>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        internal sealed class TaskObservable : IAsyncObservable<T>, INotifyCompletion
        {
            /// <summary>
            /// The underlying observable sequence to subscribe to in case the asynchronous method did not
            /// finish synchronously.
            /// </summary>
            private readonly AsyncAsyncSubject<T> _subject;

            /// <summary>
            /// The result returned by the asynchronous method in case the method finished synchronously.
            /// </summary>
            private readonly T _result;

            /// <summary>
            /// The exception thrown by the asynchronous method in case the method finished synchronously.
            /// </summary>
            private readonly Exception _exception;

            /// <summary>
            /// Creates a new <see cref="TaskObservable"/> for an asynchronous method that has not finished yet.
            /// </summary>
            public TaskObservable()
            {
                _subject = new SequentialAsyncAsyncSubject<T>();
            }

            /// <summary>
            /// Creates a new <see cref="TaskObservable"/> for an asynchronous method that synchronously returned
            /// the specified <paramref name="result"/> value.
            /// </summary>
            /// <param name="result">The result returned by the asynchronous method.</param>
            public TaskObservable(T result)
            {
                _result = result;
            }

            /// <summary>
            /// Creates a new <see cref="TaskObservable"/> for an asynchronous method that synchronously threw
            /// the specified <paramref name="exception"/>.
            /// </summary>
            /// <param name="exception">The exception thrown by the asynchronous method.</param>
            public TaskObservable(Exception exception)
            {
                _exception = exception;
            }

            /// <summary>
            /// Marks the observable as successfully completed.
            /// </summary>
            /// <param name="result">The result to use to complete the observable sequence.</param>
            /// <exception cref="InvalidOperationException">The observable has already completed.</exception>
            public async void SetResult(T result)
            {
                if (IsCompleted)
                    throw new InvalidOperationException();

                // REVIEW: Async void method.

                await _subject.OnNextAsync(result).ConfigureAwait(false);
                await _subject.OnCompletedAsync().ConfigureAwait(false);
            }

            /// <summary>
            /// Marks the observable as failed and binds the specified exception to the observable sequence.
            /// </summary>
            /// <param name="exception">The exception to bind to the observable sequence.</param>
            /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException">The observable has already completed.</exception>
            public void SetException(Exception exception)
            {
                if (IsCompleted)
                    throw new InvalidOperationException();

                _subject.OnErrorAsync(exception);
            }

            /// <summary>
            /// Subscribes the given observer to the observable sequence.
            /// </summary>
            /// <param name="observer">Observer that will receive notifications from the observable sequence.</param>
            /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
            public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
            {
                if (_subject != null)
                {
                    return _subject.SubscribeAsync(observer);
                }

                async Task<IAsyncDisposable> CoreAsync()
                {
                    if (_exception != null)
                    {
                        await observer.OnErrorAsync(_exception).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnNextAsync(_result).ConfigureAwait(false);
                    }

                    return AsyncDisposable.Nop;
                }

                return CoreAsync();
            }

            /// <summary>
            /// Gets an awaiter that can be used to await the eventual completion of the observable sequence.
            /// </summary>
            /// <returns>An awaiter that can be used to await the eventual completion of the observable sequence.</returns>
            public AsyncAsyncSubject<T> GetAwaiter() => _subject;

            /// <summary>
            /// Gets a Boolean indicating whether the observable sequence has completed.
            /// </summary>
            public bool IsCompleted => _subject?.IsCompleted ?? true;

            /// <summary>
            /// Gets the result produced by the observable sequence.
            /// </summary>
            /// <returns>The result produced by the observable sequence.</returns>
            public T GetResult()
            {
                if (_subject != null)
                {
                    return _subject.GetResult();
                }

                if (_exception != null)
                {
                    ExceptionDispatchInfo.Capture(_exception).Throw();
                }

                return _result;
            }

            /// <summary>
            /// Attaches the specified <paramref name="continuation"/> to the observable sequence.
            /// </summary>
            /// <param name="continuation">The continuation to attach.</param>
            public void OnCompleted(Action continuation)
            {
                if (_subject != null)
                {
                    _subject.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }
}
