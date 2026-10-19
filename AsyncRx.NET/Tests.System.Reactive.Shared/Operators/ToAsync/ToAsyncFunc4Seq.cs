// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(function[, scheduler])(arg1, arg2, arg3, arg4)</c>: the call of the
/// function
/// <c>ToAsync</c> returns, over a function of 4 arguments.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
/// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
/// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncFunc4{T1, T2, T3, T4, TResult}(ToAsyncFunc4Seq{T1, T2, T3, T4, TResult})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncFunc4Seq<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function,
        SchedulerRef? scheduler, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    : Seq<TResult>
{
    /// <summary>Function to convert to an asynchronous function.</summary>
    public Func<T1, T2, T3, T4, TResult> Function => function;

    /// <summary>Scheduler to invoke the function on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <summary>The second argument the function was called with.</summary>
    public T2 Arg2 => arg2;

    /// <summary>The third argument the function was called with.</summary>
    public T3 Arg3 => arg3;

    /// <summary>The fourth argument the function was called with.</summary>
    public T4 Arg4 => arg4;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncFunc4(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(function)(" : $"Seq.ToAsync(function, {scheduler})(")
        + string.Join(
            ", ",
            new object?[]
            {
                arg1, arg2, arg3, arg4,
            })
        + ")";
}
