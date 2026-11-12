// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DelaySubscriptionTimeSeq{T}"/> as the target's own
    /// <c>DelaySubscription(dueTime[, scheduler])</c> with a relative due time, on the
    /// target's default scheduler when the node names none.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.DelaySubscription{T}(Seq{T}, TimeSpan, SchedulerRef)"/> and
    /// <see cref="Seq.DelaySubscription{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> DelaySubscriptionTime<T>(DelaySubscriptionTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DelaySubscriptionAbsoluteSeq{T}"/> as the target's own
    /// <c>DelaySubscription(dueTime[, scheduler])</c> with an absolute due time, on the
    /// target's default scheduler when the node names none.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.DelaySubscription{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
    /// and <see cref="Seq.DelaySubscription{T}(Seq{T}, DateTimeOffset)"/>.
    /// </remarks>
    Realized<Seq<T>> DelaySubscriptionAbsolute<T>(DelaySubscriptionAbsoluteSeq<T> seq);
}
