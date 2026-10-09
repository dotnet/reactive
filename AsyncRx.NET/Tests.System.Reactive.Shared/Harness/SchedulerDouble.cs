// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A scheduler double written once, which each target adapts to its own interface.
/// </summary>
/// <remarks>
/// <para>
/// Rx.NET's operator tests declare a family of small schedulers (<c>ObserveOnTest</c>'s
/// <c>MyScheduler</c>, <c>DelayTest</c>'s <c>ImpulseScheduler</c>, the shared
/// <c>TestLongRunningScheduler</c>, and more), and each is the same thing underneath: a scheduler
/// whose immediate and delayed <c>Schedule</c> do something small and observable with the unit of
/// work they are handed. Only the interface differs between targets, so a double derives from
/// this class and overrides what it does with the work, and the target's one adapter
/// (<c>RxSchedulerDouble</c>, <c>AsyncRxSchedulerDouble</c>), built by
/// <see cref="IRxTarget.CreateScheduler"/>, presents it as an <c>IScheduler</c> or an
/// <c>IAsyncScheduler</c>. The double is itself the <see cref="SchedulerRef"/> a scenario passes
/// to an operator.
/// </para>
/// <para>
/// By default <see cref="Now"/> is the wall clock and delayed work is not supported, which is
/// what most of the originals do. A long-running double (<see cref="IsLongRunning"/>) receives
/// its work on Rx.NET only through <c>ISchedulerLongRunning.ScheduleLongRunning</c>, discovered
/// through <see cref="IServiceProvider"/>; the ordinary members throw, as they do in every
/// long-running double of Rx.NET's, so that a test proves the operator took that path. On
/// AsyncRx.NET, which has no such path, every unit of work arrives through
/// <see cref="ScheduleAsync(ScheduledWork, CancellationToken)"/>.
/// </para>
/// <para>
/// On Rx.NET the work completes synchronously, and so must the double's handling of it: the
/// adapter throws if the <see cref="ValueTask"/> it gets back is still pending.
/// </para>
/// </remarks>
public abstract class SchedulerDouble : SchedulerRef
{
    private readonly Realized<SchedulerDouble> _native;

    /// <summary>Creates the double and its adapter on the target under test.</summary>
    /// <param name="target">The target under test, which builds the adapter.</param>
    /// <param name="description">How this scheduler prints in the query text.</param>
    protected SchedulerDouble(IRxTarget target, string description)
        : base(description)
    {
        _native = target.CreateScheduler(this);
    }

    /// <inheritdoc/>
    public override Realized Native => _native;

    /// <summary>The scheduler's notion of the current time.</summary>
    public virtual DateTimeOffset Now => DateTimeOffset.Now;

    /// <summary>
    /// Whether, on Rx.NET, the double offers only the <c>ISchedulerLongRunning</c> path.
    /// </summary>
    public virtual bool IsLongRunning => false;

    /// <summary>Does whatever the double does with a unit of work due now.</summary>
    /// <param name="work">The unit of work.</param>
    /// <param name="cancellationToken">Cancelled when the work's subscription is disposed.</param>
    public abstract ValueTask ScheduleAsync(
        ScheduledWork work,
        CancellationToken cancellationToken);

    /// <summary>Does whatever the double does with a unit of work due after a delay.</summary>
    /// <param name="dueTime">The delay.</param>
    /// <param name="work">The unit of work.</param>
    /// <param name="cancellationToken">Cancelled when the work's subscription is disposed.</param>
    /// <remarks>Throws unless overridden, as the originals that take no delayed work do.</remarks>
    public virtual ValueTask ScheduleAsync(
        TimeSpan dueTime,
        ScheduledWork work,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
