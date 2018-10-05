// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    // We can't make those based on the Strings_Core.resx file, because the ObsoleteAttribute needs a compile-time constant.

    internal static class Constants_Core
    {
        private const string ObsoleteRefactoring = "This property is no longer supported due to refactoring of the API surface and elimination of platform-specific dependencies.";

        public const string ObsoleteSchedulerNewthread = ObsoleteRefactoring + " Please use NewThreadScheduler.Default to obtain an instance of this scheduler type.";
        public const string ObsoleteSchedulerTaskpool = ObsoleteRefactoring + " Please use TaskPoolScheduler.Default to obtain an instance of this scheduler type.";
        public const string ObsoleteSchedulerThreadpool = ObsoleteRefactoring + " Consider using Scheduler.Default to obtain the platform's most appropriate pool-based scheduler. In order to access a specific pool-based scheduler, please add a reference to the System.Reactive.PlatformServices assembly for your target platform and use the appropriate scheduler in the System.Reactive.Concurrency namespace.";

        public const string ObsoleteSchedulerequired = "This instance property is no longer supported. Use CurrentThreadScheduler.IsScheduleRequired instead.";
    }

    // We can't make those based on the Strings_*.resx file, because the ObsoleteAttribute needs a compile-time constant.

    internal static class Constants_Linq
    {
#if PREFER_ASYNC
        public const string UseAsync = "This blocking operation is no longer supported. Instead, use the async version in combination with C# and Visual Basic async/await support. In case you need a blocking operation, use Wait or convert the resulting observable sequence to a Task object and block.";
        public const string UseTaskFromAsyncPattern = "This conversion is no longer supported. Replace use of the Begin/End asynchronous method pair with a new Task-based async method, and convert the result using ToObservable. If no Task-based async method is available, use Task.Factory.FromAsync to obtain a Task object.";
#endif
    }
}
