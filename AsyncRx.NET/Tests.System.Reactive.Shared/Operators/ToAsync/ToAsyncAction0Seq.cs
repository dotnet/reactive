// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(action[, scheduler])()</c>: the call of the function
/// <c>ToAsync</c> returns, over an action of 0 arguments.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncAction0(ToAsyncAction0Seq)"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncAction0Seq(Action action, SchedulerRef? scheduler)
    : Seq<Unit>
{
    /// <summary>Action to convert to an asynchronous function.</summary>
    public Action Action => action;

    /// <summary>Scheduler to invoke the action on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncAction0(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(action)(" : $"Seq.ToAsync(action, {scheduler})(")
        + ""
        + ")";
}
