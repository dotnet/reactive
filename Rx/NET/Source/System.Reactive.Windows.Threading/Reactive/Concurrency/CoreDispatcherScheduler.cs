// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if WINDOWS
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Runtime.ExceptionServices;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on a Windows.UI.Core.CoreDispatcher.
    /// </summary>
    /// <remarks>
    /// This scheduler type is typically used indirectly through the <see cref="System.Reactive.Linq.DispatcherObservable.ObserveOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> and <see cref="System.Reactive.Linq.DispatcherObservable.SubscribeOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> methods that use the current Dispatcher.
    /// </remarks>
    public sealed class CoreDispatcherScheduler : LocalScheduler, ISchedulerPeriodic
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly CoreDispatcherPriority _priority;

        /// <summary>
        /// Constructs a CoreDispatcherScheduler that schedules units of work on the given Windows.UI.Core.CoreDispatcher.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is null.</exception>
        public CoreDispatcherScheduler(CoreDispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            _dispatcher = dispatcher;
            _priority = CoreDispatcherPriority.Normal;
        }

        /// <summary>
        /// Constructs a CoreDispatcherScheduler that schedules units of work on the given Windows.UI.Core.CoreDispatcher with the given priority.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to schedule work on.</param>
        /// <param name="priority">Priority for scheduled units of work.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is null.</exception>
        public CoreDispatcherScheduler(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            _dispatcher = dispatcher;
            _priority = priority;
        }

        /// <summary>
        /// Gets the scheduler that schedules work on the Windows.UI.Core.CoreDispatcher associated with the current Window.
        /// </summary>
        public static CoreDispatcherScheduler Current
        {
            get
            {
                var window = Window.Current;
                if (window == null)
                    throw new InvalidOperationException(Strings_WindowsThreading.NO_WINDOW_CURRENT);

                return new CoreDispatcherScheduler(window.Dispatcher);
            }
        }

        /// <summary>
        /// Gets the Windows.UI.Core.CoreDispatcher associated with the CoreDispatcherScheduler.
        /// </summary>
        public CoreDispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        /// <summary>
        /// Gets the priority at which work is scheduled.
        /// </summary>
        public CoreDispatcherPriority Priority
        {
            get { return _priority; }
        }

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

            var res = _dispatcher.RunAsync(_priority, () =>
            {
                if (!d.IsDisposed)
                {
                    try
                    {
                        d.Disposable = action(this, state);
                    }
                    catch (Exception ex)
                    {
                        //
                        // Work-around for the behavior of throwing from RunAsync not propagating
                        // the exception to the Application.UnhandledException event (as of W8RP)
                        // as our users have come to expect from previous XAML stacks using Rx.
                        //
                        // If we wouldn't do this, there'd be an observable behavioral difference
                        // between scheduling with TimeSpan.Zero or using this overload.
                        //
                        // For scheduler implementation guidance rules, see TaskPoolScheduler.cs
                        // in System.Reactive.PlatformServices\Reactive\Concurrency.
                        //
                        var timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.Zero;
                        timer.Tick += (o, e) =>
                        {
                            timer.Stop();
                            ExceptionDispatchInfo.Capture(ex).Throw();
                        };

                        timer.Start();
                    }
                }
            });

            return new CompositeDisposable(
                d,
                Disposable.Create(res.Cancel)
            );
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime on the dispatcher, using a Windows.UI.Xaml.DispatcherTimer object.
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

            var timer = new DispatcherTimer();

            timer.Tick += (o, e) =>
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
        /// Schedules a periodic piece of work on the dispatcher, using a Windows.UI.Xaml.DispatcherTimer object.
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
            //
            // According to MSDN documentation, the default is TimeSpan.Zero, so that's definitely valid.
            // Empirical observation - negative values seem to be normalized to TimeSpan.Zero, but let's not go there.
            //
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            var timer = new DispatcherTimer();

            var state1 = state;

            timer.Tick += (o, e) =>
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