// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Disposables;

#if !NO_WEAKTABLE
using System.Runtime.CompilerServices;
#endif

namespace System.Reactive.Concurrency
{
    class CatchScheduler<TException> : SchedulerWrapper
        where TException : Exception
    {
        private readonly Func<TException, bool> _handler;

        public CatchScheduler(IScheduler scheduler, Func<TException, bool> handler)
            : base(scheduler)
        {
            _handler = handler;
        }

        protected override Func<IScheduler, TState, IDisposable> Wrap<TState>(Func<IScheduler, TState, IDisposable> action)
        {
            return (self, state) =>
            {
                try
                {
                    return action(GetRecursiveWrapper(self), state);
                }
                catch (TException exception)
                {
                    if (!_handler(exception))
                        throw;

                    return Disposable.Empty;
                }
            };
        }

#if !NO_WEAKTABLE
        public CatchScheduler(IScheduler scheduler, Func<TException, bool> handler, ConditionalWeakTable<IScheduler, IScheduler> cache)
            : base(scheduler, cache)
        {
            _handler = handler;
        }

        protected override SchedulerWrapper Clone(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache)
        {
            return new CatchScheduler<TException>(scheduler, _handler, cache);
        }
#else
        protected override SchedulerWrapper Clone(IScheduler scheduler)
        {
            return new CatchScheduler<TException>(scheduler, _handler);
        }
#endif

        protected override bool TryGetService(IServiceProvider provider, Type serviceType, out object service)
        {
            service = provider.GetService(serviceType);

            if (service != null)
            {
                if (serviceType == typeof(ISchedulerLongRunning))
                    service = new CatchSchedulerLongRunning((ISchedulerLongRunning)service, _handler);
                else if (serviceType == typeof(ISchedulerPeriodic))
                    service = new CatchSchedulerPeriodic((ISchedulerPeriodic)service, _handler);
            }

            return true;
        }

        class CatchSchedulerLongRunning : ISchedulerLongRunning
        {
            private readonly ISchedulerLongRunning _scheduler;
            private readonly Func<TException, bool> _handler;

            public CatchSchedulerLongRunning(ISchedulerLongRunning scheduler, Func<TException, bool> handler)
            {
                _scheduler = scheduler;
                _handler = handler;
            }

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                return _scheduler.ScheduleLongRunning(state, (state_, cancel) =>
                {
                    try
                    {
                        action(state_, cancel);
                    }
                    catch (TException exception)
                    {
                        if (!_handler(exception))
                            throw;
                    }
                });
            }
        }

        class CatchSchedulerPeriodic : ISchedulerPeriodic
        {
            private readonly ISchedulerPeriodic _scheduler;
            private readonly Func<TException, bool> _handler;

            public CatchSchedulerPeriodic(ISchedulerPeriodic scheduler, Func<TException, bool> handler)
            {
                _scheduler = scheduler;
                _handler = handler;
            }

            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                var failed = false;

                var d = new SingleAssignmentDisposable();

                d.Disposable = _scheduler.SchedulePeriodic(state, period, state_ =>
                {
                    //
                    // Cancellation may not be granted immediately; prevent from running user
                    // code in that case. Periodic schedulers are assumed to introduce some
                    // degree of concurrency, so we should return from the SchedulePeriodic
                    // call eventually, allowing the d.Dispose() call in the catch block to
                    // take effect.
                    //
                    if (failed)
                        return default(TState);

                    try
                    {
                        return action(state_);
                    }
                    catch (TException exception)
                    {
                        failed = true;

                        if (!_handler(exception))
                            throw;

                        d.Dispose();
                        return default(TState);
                    }
                });

                return d;
            }
        }
    }
}
