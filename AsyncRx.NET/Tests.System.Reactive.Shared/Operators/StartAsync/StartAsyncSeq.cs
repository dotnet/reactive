// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.StartAsync(functionAsync[, scheduler])</c> over an asynchronous function, run
/// at the call.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.StartAsync{TResult}(StartAsyncSeq{TResult})"/>.
/// </remarks>
public sealed class StartAsyncSeq<TResult>(Func<Task<TResult>> functionAsync,
        SchedulerRef? scheduler)
    : Seq<TResult>
{
    /// <summary>Asynchronous function to run.</summary>
    public Func<Task<TResult>> FunctionAsync => functionAsync;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.StartAsync(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.StartAsync(functionAsync)"
            : $"Seq.StartAsync(functionAsync, {scheduler})";
}
