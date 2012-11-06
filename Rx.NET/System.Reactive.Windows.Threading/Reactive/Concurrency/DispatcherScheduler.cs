// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !WINDOWS
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on a <see cref="System.Windows.Threading.Dispatcher"/>.
    /// </summary>
    /// <remarks>
    /// This scheduler type is typically used indirectly through the <see cref="System.Reactive.Linq.DispatcherObservable.ObserveOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> and <see cref="System.Reactive.Linq.DispatcherObservable.SubscribeOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> methods that use the Dispatcher on the calling thread.
    /// </remarks>
    public class DispatcherScheduler : LocalScheduler, ISchedulerPeriodic
    {
        /// <summary>
        /// Gets the scheduler that schedules work on the current <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        [Obsolete(Constants_WindowsThreading.OBSOLETE_INSTANCE_PROPERTY)]
        public static DispatcherScheduler Instance
        {
            get
            {
                return new DispatcherScheduler(
#if USE_SL_DISPATCHER
                    System.Windows.Deployment.Current.Dispatcher
#else
                    System.Windows.Threading.Dispatcher.CurrentDispatcher
#endif
                );
            }
        }

        /// <summary>
        /// Gets the scheduler that schedules work on the <see cref="System.Windows.Threading.Dispatcher"/> for the current thread.
        /// </summary>
        public static DispatcherScheduler Current
        {
            get
            {
#if USE_SL_DISPATCHER
                return new DispatcherScheduler(System.Windows.Deployment.Current.Dispatcher);
#else
                var dispatcher = System.Windows.Threading.Dispatcher.FromThread(Thread.CurrentThread);
                if (dispatcher == null)
                    throw new InvalidOperationException(Strings_WindowsThreading.NO_DISPATCHER_CURRENT_THREAD);

                return new DispatcherScheduler(dispatcher);
#endif
            }
        }

        System.Windows.Threading.Dispatcher _dispatcher;

#if HAS_DISPATCHER_PRIORITY
        System.Windows.Threading.DispatcherPriority _priority;
#endif

        /// <summary>
        /// Constructs a DispatcherScheduler that schedules units of work on the given <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is null.</exception>
        public DispatcherScheduler(System.Windows.Threading.Dispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            _dispatcher = dispatcher;
#if HAS_DISPATCHER_PRIORITY
            _priority = Windows.Threading.DispatcherPriority.Normal;
#endif
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Constructs a DispatcherScheduler that schedules units of work on the given <see cref="System.Windows.Threading.Dispatcher"/> at the given priority.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <param name="priority">Priority at which units of work are scheduled.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is null.</exception>
        public DispatcherScheduler(System.Windows.Threading.Dispatcher dispatcher, System.Windows.Threading.DispatcherPriority priority)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            _dispatcher = dispatcher;
            _priority = priority;
        }
#endif

        /// <summary>
        /// Gets the <see cref="System.Windows.Threading.Dispatcher"/> associated with the DispatcherScheduler.
        /// </summary>
        public System.Windows.Threading.Dispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Gets the priority at which work items will be dispatched.
        /// </summary>
        public System.Windows.Threading.DispatcherPriority Priority
        {
            get { return _priority; }
        }
#endif

        /// <summary>
        /// Schedules an action to be executed on the dispatcher.
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

            var d = new SingleAssignmentDisposable();

            _dispatcher.BeginInvoke(
                new Action(() =>
                {
                    if (!d.IsDisposed)
                        d.Disposable = action(this, state);
                })
#if HAS_DISPATCHER_PRIORITY
                , _priority
#endif
            );

            return d;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime on the dispatcher, using a <see cref="System.Windows.Threading.DispatcherTimer"/> object.
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

            var d = new MultipleAssignmentDisposable();

            var timer = new System.Windows.Threading.DispatcherTimer(
#if HAS_DISPATCHER_PRIORITY
                _priority, _dispatcher
#elif DESKTOPCLR40 // BACKWARDS COMPATIBILITY with v1.x
                System.Windows.Threading.DispatcherPriority.Background, _dispatcher
#endif
            );

            timer.Tick += (s, e) =>
            {
                var t = Interlocked.Exchange(ref timer, null);
                if (t != null)
                {
                    try
                    {
                        d.Disposable = action(this, state);
                    }
                    finally
                    {
                        t.Stop();
                        action = null;
                    }
                }
            };

            timer.Interval = dt;
            timer.Start();

            d.Disposable = Disposable.Create(() =>
            {
                var t = Interlocked.Exchange(ref timer, null);
                if (t != null)
                {
                    t.Stop();
                    action = (_, __) => Disposable.Empty;
                }
            });

            return d;
        }

        /// <summary>
        /// Schedules a periodic piece of work on the dispatcher, using a <see cref="System.Windows.Threading.DispatcherTimer"/> object.
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

            var timer = new System.Windows.Threading.DispatcherTimer(
#if HAS_DISPATCHER_PRIORITY
                _priority, _dispatcher
#elif DESKTOPCLR40 // BACKWARDS COMPATIBILITY with v1.x
                System.Windows.Threading.DispatcherPriority.Background, _dispatcher
#endif
            );

            var state1 = state;

            timer.Tick += (s, e) =>
            {
                state1 = action(state1);
            };

            timer.Interval = period;
            timer.Start();

            return Disposable.Create(() =>
            {
                var t = Interlocked.Exchange(ref timer, null);
                if (t != null)
                {
                    t.Stop();
                    action = _ => _;
                }
            });
        }
    }
}
#endif