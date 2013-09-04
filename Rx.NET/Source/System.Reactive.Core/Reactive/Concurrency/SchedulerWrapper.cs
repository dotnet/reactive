// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

#if !NO_WEAKTABLE
using System.Runtime.CompilerServices;
#endif

namespace System.Reactive.Concurrency
{
    internal abstract class SchedulerWrapper : IScheduler, IServiceProvider
    {
        protected readonly IScheduler _scheduler;

        public SchedulerWrapper(IScheduler scheduler)
        {
            _scheduler = scheduler;

#if !NO_WEAKTABLE
            _cache = new ConditionalWeakTable<IScheduler, IScheduler>();
#endif
        }

        public DateTimeOffset Now
        {
            get { return _scheduler.Now; }
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return _scheduler.Schedule(state, Wrap(action));
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return _scheduler.Schedule(state, dueTime, Wrap(action));
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return _scheduler.Schedule(state, dueTime, Wrap(action));
        }

        protected virtual Func<IScheduler, TState, IDisposable> Wrap<TState>(Func<IScheduler, TState, IDisposable> action)
        {
            return (self, state) => action(GetRecursiveWrapper(self), state);
        }

#if !NO_WEAKTABLE
        private readonly ConditionalWeakTable<IScheduler, IScheduler> _cache;

        public SchedulerWrapper(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache)
        {
            _scheduler = scheduler;
            _cache = cache;
        }

        protected IScheduler GetRecursiveWrapper(IScheduler scheduler)
        {
            return _cache.GetValue(scheduler, s => Clone(s, _cache));
        }

        protected abstract SchedulerWrapper Clone(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache);
#else
            private readonly object _gate = new object();
            private IScheduler _recursiveOriginal;
            private IScheduler _recursiveWrapper;

            protected IScheduler GetRecursiveWrapper(IScheduler scheduler)
            {
                var recursiveWrapper = default(IScheduler);

                lock (_gate)
                {
                    //
                    // Chances are the recursive scheduler will remain the same. In practice, this
                    // single-shot caching scheme works out quite well. Notice we propagate our
                    // mini-cache to recursive raw scheduler wrappers too.
                    //
                    if (!object.ReferenceEquals(scheduler, _recursiveOriginal))
                    {
                        _recursiveOriginal = scheduler;

                        var wrapper = Clone(scheduler);
                        wrapper._recursiveOriginal = scheduler;
                        wrapper._recursiveWrapper = wrapper;

                        _recursiveWrapper = wrapper;
                    }

                    recursiveWrapper = _recursiveWrapper;
                }

                return recursiveWrapper;
            }

            protected abstract SchedulerWrapper Clone(IScheduler scheduler);
#endif

        public object GetService(Type serviceType)
        {
            var serviceProvider = _scheduler as IServiceProvider;
            if (serviceProvider == null)
                return null;

            var result = default(object);
            if (TryGetService(serviceProvider, serviceType, out result))
                return result;

            return serviceProvider.GetService(serviceType);
        }

        protected abstract bool TryGetService(IServiceProvider provider, Type serviceType, out object service);
    }
}
