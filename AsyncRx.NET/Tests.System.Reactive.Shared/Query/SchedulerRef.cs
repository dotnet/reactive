// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A scheduler argument in a description.</summary>
/// <param name="native">The target's own scheduler.</param>
/// <param name="description">How this scheduler prints in the query text.</param>
/// <remarks>
/// The target's own scheduler, carried as <see cref="object"/>. The test's
/// <see cref="TestSchedulerRef"/> is one; the result of its <c>DisableOptimizations()</c> is
/// another.
/// </remarks>
public class SchedulerRef(object native, string description)
{
    /// <summary>
    /// The target's own scheduler: an <c>IScheduler</c> on Rx.NET, an <c>IAsyncScheduler</c> on
    /// AsyncRx.NET.
    /// </summary>
    /// <remarks>Only the target casts it.</remarks>
    public object Native => native;

    /// <summary>
    /// How this scheduler prints in the query text (<c>Scheduler</c>, or
    /// <c>Scheduler.DisableOptimizations()</c>).
    /// </summary>
    public override string ToString() => description;
}
