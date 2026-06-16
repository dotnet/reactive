// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Marker interface for schedulers that support periodic scheduling, but which can't do so
    /// with sub-millisecond precision.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Scheduler.SchedulePeriodic_{TState}(IScheduler, TState, TimeSpan, Func{TState, TState})"/>
    /// incorporates a workaround for schedulers that do not support sub-millisecond precision. It used
    /// to provide this workaround for all Windows-specific targets. (Specifically, Rx 6 did it for
    /// <c>uap10.0.18362</c> and <c>net6.0-windows10.0.19041</c>.) However, since we've removed the
    /// <c>uap10.0.18362</c> target, old-style UWP apps will now end up with the <c>netstandard2.0</c>
    /// Rx.NET, so that also needs to be able to apply the workaround when required.
    /// </para>
    /// <para>
    /// Since the <c>netstandard2.0</c> is used in scenarios where the workaround definitely isn't required
    /// (any scenario other than legacy UWP in fact), it can't apply it indiscriminately: it needs to detect
    /// at runtime whether the workaround is necessary. We don't want to use reflection to achieve this because
    /// it would impose a performance penalty on all periodic scheduling operations, just to enable a workaround
    /// that's only needed on legacy UWP apps. Also, reflection is problematic in trimming and AOT scenarios.
    /// So we use this marker interface to enable efficient detection of whether the workaround is needed.
    /// It's internal because this workaround was only ever designed for internal purposes. We use
    /// InternalVisibleTo to enable the components that now contain the legacy code that needs the workaround
    /// to apply it to the relevant schedulers.
    /// </para>
    /// </remarks>
    internal interface ISchedulerPeriodNoSubMs : ISchedulerPeriodic
    {
    }
}
