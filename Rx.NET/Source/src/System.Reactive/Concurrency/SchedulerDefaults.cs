// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    internal static class SchedulerDefaults
    {
        internal static IScheduler ConstantTimeOperations => ImmediateScheduler.Instance;
        internal static IScheduler TailRecursion => ImmediateScheduler.Instance;
        internal static IScheduler Iteration => CurrentThreadScheduler.Instance;
        internal static IScheduler TimeBasedOperations => DefaultScheduler.Instance;
        internal static IScheduler AsyncConversions => DefaultScheduler.Instance;
    }
}
