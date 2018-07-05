// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        private sealed class AsyncInvocation<TState> : IDisposable
        {
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();
            private IDisposable _run;

            public IDisposable Run(IScheduler self, TState s, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
            {
                if (_cts.IsCancellationRequested)
                    return Disposable.Empty;

                action(new CancelableScheduler(self, _cts.Token), s, _cts.Token).ContinueWith(
                    (t, thisObject) =>
                    {
                        var @this = (AsyncInvocation<TState>)thisObject;

                        t.Exception?.Handle(e => e is OperationCanceledException);

                        Disposable.SetSingle(ref @this._run, t.Result);
                    },
                    this,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.NotOnCanceled);

                return this;
            }

            public void Dispose()
            {
                _cts.Cancel();
                Disposable.TryDispose(ref _run);
            }
        }

        /// <summary>
        /// Yields execution of the current work item on the scheduler to another work item on the scheduler.
        /// The caller should await the result of calling Yield to schedule the remainder of the current work item (known as the continuation).
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Yield(this IScheduler scheduler)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(a), scheduler.GetCancellationToken());
        }

        /// <summary>
        /// Yields execution of the current work item on the scheduler to another work item on the scheduler.
        /// The caller should await the result of calling Yield to schedule the remainder of the current work item (known as the continuation).
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the continuation to run.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Yield(this IScheduler scheduler, CancellationToken cancellationToken)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(a), cancellationToken);
        }

        /// <summary>
        /// Suspends execution of the current work item on the scheduler for the specified duration.
        /// The caller should await the result of calling Sleep to schedule the remainder of the current work item (known as the continuation) after the specified duration.
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <param name="dueTime">Time when the continuation should run.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Sleep(this IScheduler scheduler, TimeSpan dueTime)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(dueTime, a), scheduler.GetCancellationToken());
        }

        /// <summary>
        /// Suspends execution of the current work item on the scheduler for the specified duration.
        /// The caller should await the result of calling Sleep to schedule the remainder of the current work item (known as the continuation) after the specified duration.
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <param name="dueTime">Time when the continuation should run.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the continuation to run.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Sleep(this IScheduler scheduler, TimeSpan dueTime, CancellationToken cancellationToken)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(dueTime, a), cancellationToken);
        }

        /// <summary>
        /// Suspends execution of the current work item on the scheduler until the specified due time.
        /// The caller should await the result of calling Sleep to schedule the remainder of the current work item (known as the continuation) at the specified due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <param name="dueTime">Time when the continuation should run.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Sleep(this IScheduler scheduler, DateTimeOffset dueTime)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(dueTime, a), scheduler.GetCancellationToken());
        }

        /// <summary>
        /// Suspends execution of the current work item on the scheduler until the specified due time.
        /// The caller should await the result of calling Sleep to schedule the remainder of the current work item (known as the continuation) at the specified due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to yield work on.</param>
        /// <param name="dueTime">Time when the continuation should run.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the continuation to run.</param>
        /// <returns>Scheduler operation object to await in order to schedule the continuation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is <c>null</c>.</exception>
        public static SchedulerOperation Sleep(this IScheduler scheduler, DateTimeOffset dueTime, CancellationToken cancellationToken)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new SchedulerOperation(a => scheduler.Schedule(dueTime, a), cancellationToken);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, Func<IScheduler, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, (self, closureAction, ct) => closureAction(self, ct));
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, Func<IScheduler, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, (self, closureAction, ct) => closureAction(self, ct));
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, dueTime, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, dueTime, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, TimeSpan dueTime, Func<IScheduler, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, dueTime, (self, closureAction, ct) => closureAction(self, ct));
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, TimeSpan dueTime, Func<IScheduler, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, dueTime, (self, closureAction, ct) => closureAction(self, ct));
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, dueTime, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="state">State to pass to the asynchronous method.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, state, dueTime, action);
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, DateTimeOffset dueTime, Func<IScheduler, CancellationToken, Task> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, dueTime, (self, closureAction, ct) => closureAction(self, ct));
        }

        /// <summary>
        /// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
        /// </summary>
        /// <param name="scheduler">Scheduler to schedule work on.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <param name="action">Asynchronous method to run the work, using Yield and Sleep operations for cooperative scheduling and injection of cancellation points.</param>
        /// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleAsync(this IScheduler scheduler, DateTimeOffset dueTime, Func<IScheduler, CancellationToken, Task<IDisposable>> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ScheduleAsync_(scheduler, action, dueTime, (self, closureAction, ct) => closureAction(self, ct));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            return scheduler.Schedule((state, action), (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            return scheduler.Schedule((state, action), (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, TimeSpan dueTime, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            return scheduler.Schedule((state, action), dueTime, (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, TimeSpan dueTime, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            return scheduler.Schedule((state, action), dueTime, (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, DateTimeOffset dueTime, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            return scheduler.Schedule((state, action), dueTime, (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable ScheduleAsync_<TState>(IScheduler scheduler, TState state, DateTimeOffset dueTime, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            return scheduler.Schedule((state, action), dueTime, (self, t) => InvokeAsync(self, t.state, t.action));
        }

        private static IDisposable InvokeAsync<TState>(IScheduler self, TState s, Func<IScheduler, TState, CancellationToken, Task<IDisposable>> action)
        {
            return new AsyncInvocation<TState>().Run(self, s, action);
        }

        private static IDisposable InvokeAsync<TState>(IScheduler self, TState s, Func<IScheduler, TState, CancellationToken, Task> action)
        {
            return InvokeAsync(self, (action, state: s), (self_, t, ct) => t.action(self_, t.state, ct).ContinueWith(_ => Disposable.Empty));
        }

        private static CancellationToken GetCancellationToken(this IScheduler scheduler)
        {
            return scheduler is CancelableScheduler cs ? cs.Token : CancellationToken.None;
        }

        private sealed class CancelableScheduler : IScheduler
        {
            private readonly IScheduler _scheduler;
            private readonly CancellationToken _cancellationToken;

            public CancelableScheduler(IScheduler scheduler, CancellationToken cancellationToken)
            {
                _scheduler = scheduler;
                _cancellationToken = cancellationToken;
            }

            public CancellationToken Token
            {
                get { return _cancellationToken; }
            }

            public DateTimeOffset Now => _scheduler.Now;

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule(state, action);
            }

            public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule(state, dueTime, action);
            }

            public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule(state, dueTime, action);
            }
        }
    }
}
