// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

namespace System.Reactive.Concurrency
{
    internal sealed class CatchScheduler<TException> : SchedulerWrapper
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
                catch (TException exception) when (_handler(exception))
                {
                    return Disposable.Empty;
                }
            };
        }

        public CatchScheduler(IScheduler scheduler, Func<TException, bool> handler, ConditionalWeakTable<IScheduler, IScheduler> cache)
            : base(scheduler, cache)
        {
            _handler = handler;
        }

        protected override SchedulerWrapper Clone(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache)
        {
            return new CatchScheduler<TException>(scheduler, _handler, cache);
        }

        protected override bool TryGetService(IServiceProvider provider, Type serviceType, out object service)
        {
            service = provider.GetService(serviceType);

            if (service != null)
            {
                if (serviceType == typeof(ISchedulerLongRunning))
                {
                    service = new CatchSchedulerLongRunning((ISchedulerLongRunning)service, _handler);
                }
                else if (serviceType == typeof(ISchedulerPeriodic))
                {
                    service = new CatchSchedulerPeriodic((ISchedulerPeriodic)service, _handler);
                }
            }

            return true;
        }

        private class CatchSchedulerLongRunning : ISchedulerLongRunning
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
                // Note that avoiding closure allocation here would introduce infinite generic recursion over the TState argument
                
                return _scheduler.ScheduleLongRunning(
                    state,
                    (state1, cancel) =>
                    {
                        try
                        {
                            action(state1, cancel);
                        }
                        catch (TException exception) when (_handler(exception))
                        {
                        }
                    });
            }
        }

        private sealed class CatchSchedulerPeriodic : ISchedulerPeriodic
        {
            private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
            {
                private IDisposable _cancel;
                private bool _failed;

                private readonly Func<TState, TState> _action;
                private readonly CatchSchedulerPeriodic _catchScheduler;

                public PeriodicallyScheduledWorkItem(CatchSchedulerPeriodic scheduler, TState state, TimeSpan period, Func<TState, TState> action)
                {
                    _catchScheduler = scheduler;
                    _action = action;

                    Disposable.SetSingle(ref _cancel, scheduler._scheduler.SchedulePeriodic((@this: this, state), period, tuple => tuple.@this?.Tick(tuple.state) ?? default));
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref _cancel);
                }

                private (PeriodicallyScheduledWorkItem<TState> @this, TState state) Tick(TState state)
                {
                    //
                    // Cancellation may not be granted immediately; prevent from running user
                    // code in that case. Periodic schedulers are assumed to introduce some
                    // degree of concurrency, so we should return from the SchedulePeriodic
                    // call eventually, allowing the d.Dispose() call in the catch block to
                    // take effect.
                    //
                    if (_failed)
                    {
                        return default;
                    }

                    try
                    {
                        return (this, _action(state));
                    }
                    catch (TException exception)
                    {
                        _failed = true;

                        if (!_catchScheduler._handler(exception))
                        {
                            throw;
                        }

                        Disposable.TryDispose(ref _cancel);
                        return default;
                    }
                }
            }

            private readonly ISchedulerPeriodic _scheduler;
            private readonly Func<TException, bool> _handler;

            public CatchSchedulerPeriodic(ISchedulerPeriodic scheduler, Func<TException, bool> handler)
            {
                _scheduler = scheduler;
                _handler = handler;
            }

            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                return new PeriodicallyScheduledWorkItem<TState>(this, state, period, action);
            }
        }
    }
}
