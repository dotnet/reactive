// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace System.Reactive.Concurrency
{
    internal abstract class SchedulerWrapper : IScheduler, IServiceProvider
    {
        protected readonly IScheduler _scheduler;
        private readonly ConditionalWeakTable<IScheduler, IScheduler> _cache;

        protected SchedulerWrapper(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _cache = new ConditionalWeakTable<IScheduler, IScheduler>();
        }

        protected SchedulerWrapper(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache)
        {
            _scheduler = scheduler;
            _cache = cache;
        }

        public DateTimeOffset Now => _scheduler.Now;

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return _scheduler.Schedule(state, Wrap(action));
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return _scheduler.Schedule(state, dueTime, Wrap(action));
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return _scheduler.Schedule(state, dueTime, Wrap(action));
        }

        protected virtual Func<IScheduler, TState, IDisposable> Wrap<TState>(Func<IScheduler, TState, IDisposable> action)
        {
            return (self, state) => action(GetRecursiveWrapper(self), state);
        }

        protected IScheduler GetRecursiveWrapper(IScheduler scheduler)
        {
            return _cache.GetValue(scheduler, s => Clone(s, _cache));
        }

        protected abstract SchedulerWrapper Clone(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache);

        public object GetService(Type serviceType)
        {
            if (!(_scheduler is IServiceProvider serviceProvider))
            {
                return null;
            }

            if (TryGetService(serviceProvider, serviceType, out var result))
            {
                return result;
            }

            return serviceProvider.GetService(serviceType);
        }

        protected abstract bool TryGetService(IServiceProvider provider, Type serviceType, out object service);
    }
}
