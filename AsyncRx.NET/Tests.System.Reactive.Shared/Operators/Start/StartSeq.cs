// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Start(function[, scheduler])</c> over a function, run at the call.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.Start</c> overloads in <c>Seq.Start.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.Start{TResult}(StartSeq{TResult})"/>.
/// </remarks>
public sealed class StartSeq<TResult>(Func<TResult> function, SchedulerRef? scheduler)
    : Seq<TResult>
{
    /// <summary>Function to run asynchronously.</summary>
    public Func<TResult> Function => function;

    /// <summary>Scheduler to run the function on. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Start(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.Start(function)"
            : $"Seq.Start(function, {scheduler})";
}
