// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Throw&lt;T&gt;(error)</c> or
/// <c>Seq.Throw&lt;T&gt;(error, scheduler)</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Throw{T}(Exception)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Throw{T}(ThrowSeq{T})"/>.
/// </remarks>
public sealed class ThrowSeq<T>(Exception error, SchedulerRef? scheduler) : Seq<T>
{
    /// <summary>Exception object used for the sequence's termination.</summary>
    public Exception Error => error;

    /// <summary>Scheduler to send the exceptional termination call on.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    public override RSeq<T> Accept(ISeqVisitor visitor) => visitor.Throw(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? $"Seq.Throw<{typeof(T).Name}>({error.GetType().Name})" : $"Seq.Throw<{typeof(T).Name}>({error.GetType().Name}, {scheduler})";
}
