// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the Task Parallel Library (TPL) task pool.
    /// </summary>
    /// <seealso cref="Default">Instance of this type using the default TaskScheduler to schedule work on the TPL task pool.</seealso>
    public sealed class TaskPoolScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
    {
        private sealed class ScheduledWorkItem<TState> : IDisposable
        {
            private readonly TState _state;
            private readonly TaskPoolScheduler _scheduler;
            private readonly Func<IScheduler, TState, IDisposable> _action;

            private IDisposable _cancel;

            public ScheduledWorkItem(TaskPoolScheduler scheduler, TState state, Func<IScheduler, TState, IDisposable> action)
            {
                _state = state;
                _action = action;
                _scheduler = scheduler;

                var cancelable = new CancellationDisposable();

                Disposable.SetSingle(ref _cancel, cancelable);

                scheduler._taskFactory.StartNew(
                    thisObject =>
                    {
                        var @this = (ScheduledWorkItem<TState>)thisObject;
                        //
                        // BREAKING CHANGE v2.0 > v1.x - No longer escalating exceptions using a throwing
                        //                               helper thread.
                        //
                        // Our manual escalation based on the creation of a throwing thread was merely to
                        // expedite the process of throwing the exception that would otherwise occur on the
                        // finalizer thread at a later point during the app's lifetime.
                        //
                        // However, it also prevented applications from observing the exception through
                        // the TaskScheduler.UnobservedTaskException static event. Also, starting form .NET
                        // 4.5, the default behavior of the task pool is not to take down the application
                        // when an exception goes unobserved (done as part of the async/await work). It'd
                        // be weird for Rx not to follow the platform defaults.
                        //
                        // General implementation guidelines for schedulers (in order of importance):
                        //
                        //    1. Always thunk through to the underlying infrastructure with a wrapper that's as tiny as possible.
                        //    2. Global exception notification/handling mechanisms shouldn't be bypassed.
                        //    3. Escalation behavior for exceptions is left to the underlying infrastructure.
                        //
                        // The Catch extension method for IScheduler (added earlier) allows to re-route
                        // exceptions at stage 2. If the exception isn't handled at the Rx level, it
                        // propagates by means of a rethrow, falling back to behavior in 3.
                        //
                        Disposable.TrySetSerial(ref @this._cancel, @this._action(@this._scheduler, @this._state));
                    },
                    this,
                    cancelable.Token);
            }

            public void Dispose()
            {
                Disposable.TryDispose(ref _cancel);
            }
        }

        private sealed class SlowlyScheduledWorkItem<TState> : IDisposable
        {
            private readonly TState _state;
            private readonly TaskPoolScheduler _scheduler;
            private readonly Func<IScheduler, TState, IDisposable> _action;

            private IDisposable _cancel;

            public SlowlyScheduledWorkItem(TaskPoolScheduler scheduler, TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                _state = state;
                _action = action;
                _scheduler = scheduler;

                var ct = new CancellationDisposable();
                Disposable.SetSingle(ref _cancel, ct);

                TaskHelpers.Delay(dueTime, ct.Token).ContinueWith(
                    (_, thisObject) =>
                    {
                        var @this = (SlowlyScheduledWorkItem<TState>)thisObject;

                        if (!Disposable.GetIsDisposed(ref @this._cancel))
                        {
                            Disposable.TrySetMultiple(ref @this._cancel, @this._action(@this._scheduler, @this._state));
                        }
                    },
                    this,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    scheduler._taskFactory.Scheduler);
            }

            public void Dispose()
            {
                Disposable.TryDispose(ref _cancel);
            }
        }

        private sealed class LongScheduledWorkItem<TState> : ICancelable
        {
            private readonly TState _state;
            private readonly Action<TState, ICancelable> _action;

            private IDisposable _cancel;

            public LongScheduledWorkItem(TaskPoolScheduler scheduler, TState state, Action<TState, ICancelable> action)
            {
                _state = state;
                _action = action;

                scheduler._taskFactory.StartNew(
                    thisObject =>
                    {
                        var @this = (LongScheduledWorkItem<TState>)thisObject;

                        //
                        // Notice we don't check _cancel.IsDisposed. The contract for ISchedulerLongRunning
                        // requires us to ensure the scheduled work gets an opportunity to observe
                        // the cancellation request.
                        //
                        @this._action(@this._state, @this);
                    },
                    this,
                    TaskCreationOptions.LongRunning);
            }

            public void Dispose()
            {
                Disposable.TryDispose(ref _cancel);
            }

            public bool IsDisposed => Disposable.GetIsDisposed(ref _cancel);
        }

        private static readonly Lazy<TaskPoolScheduler> LazyInstance = new Lazy<TaskPoolScheduler>(() => new TaskPoolScheduler(new TaskFactory(TaskScheduler.Default)));
        private readonly TaskFactory _taskFactory;

        /// <summary>
        /// Creates an object that schedules units of work using the provided <see cref="TaskFactory"/>.
        /// </summary>
        /// <param name="taskFactory">Task factory used to create tasks to run units of work.</param>
        /// <exception cref="ArgumentNullException"><paramref name="taskFactory"/> is <c>null</c>.</exception>
        public TaskPoolScheduler(TaskFactory taskFactory)
        {
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
        }

        /// <summary>
        /// Gets an instance of this scheduler that uses the default <see cref="TaskScheduler"/>.
        /// </summary>
        public static TaskPoolScheduler Default => LazyInstance.Value;

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new ScheduledWorkItem<TState>(this, state, action);
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
            {
                return Schedule(state, action);
            }

            return ScheduleSlow(state, dt, action);
        }

        private IDisposable ScheduleSlow<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            return new SlowlyScheduledWorkItem<TState>(this, state, dueTime, action);
        }

        /// <summary>
        /// Schedules a long-running task by creating a new task using TaskCreationOptions.LongRunning. Cancellation happens through polling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            return new LongScheduledWorkItem<TState>(this, state, action);
        }

        /// <summary>
        /// Gets a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        public override IStopwatch StartStopwatch()
        {
            //
            // Strictly speaking, this explicit override is not necessary because the base implementation calls into
            // the enlightenment module to obtain the CAL, which would circle back to System.Reactive.PlatformServices
            // where we're currently running. This is merely a short-circuit to avoid the additional roundtrip.
            //
            return new StopwatchImpl();
        }

        /// <summary>
        /// Schedules a periodic piece of work by running a platform-specific timer to create tasks periodically.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new PeriodicallyScheduledWorkItem<TState>(state, period, action, _taskFactory);
        }

        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private TState _state;

            private readonly TimeSpan _period;
            private readonly TaskFactory _taskFactory;
            private readonly Func<TState, TState> _action;
            private readonly AsyncLock _gate = new AsyncLock();
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();

            public PeriodicallyScheduledWorkItem(TState state, TimeSpan period, Func<TState, TState> action, TaskFactory taskFactory)
            {
                _state = state;
                _period = period;
                _action = action;
                _taskFactory = taskFactory;

                MoveNext();
            }

            public void Dispose()
            {
                _cts.Cancel();
                _gate.Dispose();
            }

            private void MoveNext()
            {
                TaskHelpers.Delay(_period, _cts.Token).ContinueWith(
                    (_, thisObject) =>
                    {
                        var @this = (PeriodicallyScheduledWorkItem<TState>)thisObject;

                        @this.MoveNext();

                        @this._gate.Wait(
                            @this,
                            closureThis => closureThis._state = closureThis._action(closureThis._state));
                    },
                    this,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    _taskFactory.Scheduler
                );
            }
        }
    }
}
