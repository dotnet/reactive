// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromAsync(functionAsync[, scheduler])</c> over an asynchronous function that
/// takes a cancellation token.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
/// <remarks>
/// Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.FromAsyncCancel{TResult}(FromAsyncCancelSeq{TResult})"/>.
/// </remarks>
public sealed class FromAsyncCancelSeq<TResult>(Func<CancellationToken,
        Task<TResult>> functionAsync, SchedulerRef? scheduler)
    : Seq<TResult>
{
    /// <summary>Asynchronous function to convert, supporting cancellation.</summary>
    public Func<CancellationToken, Task<TResult>> FunctionAsync => functionAsync;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromAsyncCancel(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromAsync(functionAsync)"
            : $"Seq.FromAsync(functionAsync, {scheduler})";
}
