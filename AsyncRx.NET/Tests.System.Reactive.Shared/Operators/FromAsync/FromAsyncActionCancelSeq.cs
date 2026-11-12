// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsync(actionAsync[, scheduler])</c> over an asynchronous action that takes
/// a cancellation token.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.FromAsyncActionCancel(FromAsyncActionCancelSeq)"/>.
/// </remarks>
public sealed class FromAsyncActionCancelSeq(Func<CancellationToken, Task> actionAsync,
        SchedulerRef? scheduler)
    : Seq<Unit>
{
    /// <summary>Asynchronous action to convert, supporting cancellation.</summary>
    public Func<CancellationToken, Task> ActionAsync => actionAsync;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncActionCancel(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromAsync(actionAsync)"
            : $"Seq.FromAsync(actionAsync, {scheduler})";
}
