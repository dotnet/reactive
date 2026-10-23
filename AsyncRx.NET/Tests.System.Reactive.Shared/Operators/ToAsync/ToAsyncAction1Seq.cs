// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(action[, scheduler])(arg1)</c>: the call of the function
/// <c>ToAsync</c> returns, over an action of 1 argument.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncAction1{T1}(ToAsyncAction1Seq{T1})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncAction1Seq<T1>(Action<T1> action, SchedulerRef? scheduler, T1 arg1)
    : Seq<Unit>
{
    /// <summary>Action to convert to an asynchronous function.</summary>
    public Action<T1> Action => action;

    /// <summary>Scheduler to invoke the action on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncAction1(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(action)(" : $"Seq.ToAsync(action, {scheduler})(")
        + string.Join(", ", new object?[] { arg1 })
        + ")";
}
