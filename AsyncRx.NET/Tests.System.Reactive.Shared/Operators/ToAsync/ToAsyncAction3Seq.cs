// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(action[, scheduler])(arg1, arg2, arg3)</c>: the call of the function
/// <c>ToAsync</c> returns, over an action of 3 arguments.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
/// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncAction3{T1, T2, T3}(ToAsyncAction3Seq{T1, T2, T3})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncAction3Seq<T1, T2, T3>(Action<T1, T2, T3> action,
        SchedulerRef? scheduler, T1 arg1, T2 arg2, T3 arg3)
    : Seq<Unit>
{
    /// <summary>Action to convert to an asynchronous function.</summary>
    public Action<T1, T2, T3> Action => action;

    /// <summary>Scheduler to invoke the action on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <summary>The second argument the function was called with.</summary>
    public T2 Arg2 => arg2;

    /// <summary>The third argument the function was called with.</summary>
    public T3 Arg3 => arg3;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncAction3(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(action)(" : $"Seq.ToAsync(action, {scheduler})(")
        + string.Join(
            ", ",
            new object?[]
            {
                arg1, arg2, arg3,
            })
        + ")";
}
