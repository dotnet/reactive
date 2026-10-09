// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Empty&lt;T&gt;()</c> or <c>Seq.Empty&lt;T&gt;(scheduler)</c>.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Empty{T}()"/>;
/// materialized by each target through <see cref="ISeqVisitor.Empty{T}(EmptySeq{T})"/>.
/// </remarks>
public sealed class EmptySeq<T>(SchedulerRef? scheduler = null) : Seq<T>
{
    /// <summary>Scheduler to send the termination call on.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Empty(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Empty<{typeof(T).Name}>()"
            : $"Seq.Empty<{typeof(T).Name}>({scheduler})";
}
