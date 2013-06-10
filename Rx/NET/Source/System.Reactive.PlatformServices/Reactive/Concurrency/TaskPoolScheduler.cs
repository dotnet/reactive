// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the Task Parallel Library (TPL) task pool.
    /// </summary>
    /// <seealso cref="TaskPoolScheduler.Default">Instance of this type using the default TaskScheduler to schedule work on the TPL task pool.</seealso>
    public sealed class TaskPoolScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
    {
        private static readonly TaskPoolScheduler s_instance = new TaskPoolScheduler(new TaskFactory(TaskScheduler.Default));
        private readonly TaskFactory taskFactory;

        /// <summary>
        /// Creates an object that schedules units of work using the provided TaskFactory.
        /// </summary>
        /// <param name="taskFactory">Task factory used to create tasks to run units of work.</param>
        /// <exception cref="ArgumentNullException"><paramref name="taskFactory"/> is null.</exception>
        public TaskPoolScheduler(TaskFactory taskFactory)
        {
            if (taskFactory == null)
                throw new ArgumentNullException("taskFactory");

            this.taskFactory = taskFactory;
        }

        /// <summary>
        /// Gets an instance of this scheduler that uses the default TaskScheduler.
        /// </summary>
        public static TaskPoolScheduler Default
        {
            get
            {
                return s_instance;
            }
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var d = new SerialDisposable();
            var cancelable = new CancellationDisposable();
            d.Disposable = cancelable;
            taskFactory.StartNew(() =>
            {
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
                d.Disposable = action(this, state);
            }, cancelable.Token);
            return d;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
                return Schedule(state, action);

#if !NO_TASK_DELAY
            var d = new MultipleAssignmentDisposable();

            var ct = new CancellationDisposable();
            d.Disposable = ct;

#if USE_TASKEX
            TaskEx.Delay(dueTime, ct.Token).ContinueWith(_ =>
#else
            Task.Delay(dueTime, ct.Token).ContinueWith(_ =>
#endif
            {
                if (!d.IsDisposed)
                    d.Disposable = action(this, state);
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, taskFactory.Scheduler);

            return d;
#else
            return DefaultScheduler.Instance.Schedule(state, dt, (_, state1) => Schedule(state1, action));
#endif
        }

        /// <summary>
        /// Schedules a long-running task by creating a new task using TaskCreationOptions.LongRunning. Cancellation happens through polling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            var d = new BooleanDisposable();

            taskFactory.StartNew(() =>
            {
                //
                // Notice we don't check d.IsDisposed. The contract for ISchedulerLongRunning
                // requires us to ensure the scheduled work gets an opportunity to observe
                // the cancellation request.
                //
                action(state, d);
            }, TaskCreationOptions.LongRunning);

            return d;
        }

#if !NO_STOPWATCH
        /// <summary>
        /// Gets a new stopwatch ob ject.
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
#endif

        /// <summary>
        /// Schedules a periodic piece of work by running a platform-specific timer to create tasks periodically.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

#if !NO_TASK_DELAY
            var cancel = new CancellationDisposable();

            var state1 = state;
            var gate = new AsyncLock();

            var moveNext = default(Action);
            moveNext = () =>
            {
#if USE_TASKEX
                TaskEx.Delay(period, cancel.Token).ContinueWith(
#else
                Task.Delay(period, cancel.Token).ContinueWith(
#endif
                    _ =>
                    {
                        moveNext();

                        gate.Wait(() =>
                        {
                            state1 = action(state1);
                        });
                    },
                    CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, taskFactory.Scheduler
                );
            };

            moveNext();

            return new CompositeDisposable(cancel, gate);
#else
            var state1 = state;
            var gate = new AsyncLock();

            var timer = ConcurrencyAbstractionLayer.Current.StartPeriodicTimer(() =>
            {
                taskFactory.StartNew(() =>
                {
                    gate.Wait(() =>
                    {
                        state1 = action(state1);
                    });
                });
            }, period);

            return new CompositeDisposable(timer, gate);
#endif
        }
    }
}
#endif
