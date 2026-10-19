// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(function[, scheduler])(arg1)</c>: the call of the function
/// <c>ToAsync</c> returns, over a function of 1 argument.
/// </summary>
/// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncFunc1{T1, TResult}(ToAsyncFunc1Seq{T1, TResult})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncFunc1Seq<T1, TResult>(Func<T1, TResult> function,
        SchedulerRef? scheduler, T1 arg1)
    : Seq<TResult>
{
    /// <summary>Function to convert to an asynchronous function.</summary>
    public Func<T1, TResult> Function => function;

    /// <summary>Scheduler to invoke the function on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <summary>The first argument the function was called with.</summary>
    public T1 Arg1 => arg1;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncFunc1(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(function)(" : $"Seq.ToAsync(function, {scheduler})(")
        + string.Join(", ", new object?[] { arg1 })
        + ")";
}
