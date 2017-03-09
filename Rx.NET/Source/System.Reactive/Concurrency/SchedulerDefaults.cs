// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;

namespace System.Reactive.Concurrency
{
    internal static class SchedulerDefaults
    {
        internal static IScheduler ConstantTimeOperations { get { return ImmediateScheduler.Instance; } }
        internal static IScheduler TailRecursion { get { return ImmediateScheduler.Instance; } }
        internal static IScheduler Iteration { get { return CurrentThreadScheduler.Instance; } }
        internal static IScheduler TimeBasedOperations { get { return DefaultScheduler.Instance; } }
        internal static IScheduler AsyncConversions { get { return DefaultScheduler.Instance; } }
    }
}
