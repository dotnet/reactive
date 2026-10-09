// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A scheduler argument in a description.</summary>
/// <param name="native">The target's own scheduler, held as a realization.</param>
/// <param name="description">How this scheduler prints in the query text.</param>
/// <remarks>
/// The target's own scheduler, carried as a <see cref="Realized"/>. The test's
/// <see cref="TestSchedulerRef"/> is one; the result of its <c>DisableOptimizations()</c> is
/// another; a <see cref="SchedulerDouble"/> is one that supplies its own <see cref="Native"/>.
/// </remarks>
public class SchedulerRef(Realized native, string description)
{
    /// <summary>Creates a reference whose derived type supplies <see cref="Native"/>.</summary>
    /// <param name="description">How this scheduler prints in the query text.</param>
    protected SchedulerRef(string description)
        : this(null!, description)
    {
    }

    /// <summary>
    /// The target's own scheduler: an <c>IScheduler</c> on Rx.NET, an <c>IAsyncScheduler</c> on
    /// AsyncRx.NET.
    /// </summary>
    /// <remarks>Only the target gets it out, through <see cref="Realized.Get{TS}"/>.</remarks>
    public virtual Realized Native => native;

    /// <summary>
    /// How this scheduler prints in the query text (<c>Scheduler</c>, or
    /// <c>Scheduler.DisableOptimizations()</c>).
    /// </summary>
    public override string ToString() => description;
}
