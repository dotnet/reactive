// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    // We can't make those based on the Strings_Core.resx file, because the ObsoleteAttribute needs a compile-time constant.

    class Constants_Core
    {
        private const string OBSOLETE_REFACTORING = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies.";

        public const string OBSOLETE_SCHEDULER_NEWTHREAD  = OBSOLETE_REFACTORING + " Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use NewThreadScheduler.Default to obtain an instance of this scheduler type. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
        public const string OBSOLETE_SCHEDULER_TASKPOOL   = OBSOLETE_REFACTORING + " Please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use TaskPoolScheduler.Default to obtain an instance of this scheduler type. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
        public const string OBSOLETE_SCHEDULER_THREADPOOL = OBSOLETE_REFACTORING + " Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";

        public const string OBSOLETE_SCHEDULEREQUIRED     = "This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead. See http://go.microsoft.com/fwlink/?LinkID=260866 for more information.";
    }
}
