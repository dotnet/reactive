// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Reactive.Concurrency
{
    internal sealed class DisableOptimizationsScheduler : SchedulerWrapper
    {
        private readonly Type[] _optimizationInterfaces;

        public DisableOptimizationsScheduler(IScheduler scheduler)
            : base(scheduler)
        {
            _optimizationInterfaces = Scheduler.Optimizations;
        }

        public DisableOptimizationsScheduler(IScheduler scheduler, Type[] optimizationInterfaces)
            : base(scheduler)
        {
            _optimizationInterfaces = optimizationInterfaces;
        }

        public DisableOptimizationsScheduler(IScheduler scheduler, Type[] optimizationInterfaces, ConditionalWeakTable<IScheduler, IScheduler> cache)
            : base(scheduler, cache)
        {
            _optimizationInterfaces = optimizationInterfaces;
        }

        protected override SchedulerWrapper Clone(IScheduler scheduler, ConditionalWeakTable<IScheduler, IScheduler> cache)
        {
            return new DisableOptimizationsScheduler(scheduler, _optimizationInterfaces, cache);
        }

        protected override bool TryGetService(IServiceProvider provider, Type serviceType, out object service)
        {
            service = null;
            return _optimizationInterfaces.Contains(serviceType);
        }
    }
}
