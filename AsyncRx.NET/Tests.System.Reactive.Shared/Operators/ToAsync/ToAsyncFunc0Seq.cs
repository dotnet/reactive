// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.ToAsync(function[, scheduler])()</c>: the call of the function
/// <c>ToAsync</c> returns, over a function of 0 arguments.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>, which return a function
/// that builds this node when called; materialized by each target through
/// <see cref="ISeqVisitor.ToAsyncFunc0{TResult}(ToAsyncFunc0Seq{TResult})"/>, which calls the target's own returned
/// function with the arguments held here.
/// </remarks>
public sealed class ToAsyncFunc0Seq<TResult>(Func<TResult> function, SchedulerRef? scheduler)
    : Seq<TResult>
{
    /// <summary>Function to convert to an asynchronous function.</summary>
    public Func<TResult> Function => function;

    /// <summary>Scheduler to invoke the function on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToAsyncFunc0(this);

    /// <inheritdoc/>
    public override string ToString() =>
        (scheduler is null ? "Seq.ToAsync(function)(" : $"Seq.ToAsync(function, {scheduler})(")
        + ""
        + ")";
}
