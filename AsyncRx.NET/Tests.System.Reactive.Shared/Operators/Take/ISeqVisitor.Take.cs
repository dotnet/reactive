// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TakeSeq{T}"/> as the target's own <c>Take(count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="TakeExtensions.Take{T}(Seq{T}, int)"/>.</remarks>
    object Take<T>(TakeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeScheduledSeq{T}"/> as the target's own
    /// <c>Take(count, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, int, SchedulerRef)"/>.
    /// </remarks>
    object TakeScheduled<T>(TakeScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeTimeSeq{T}"/> as the target's own
    /// <c>Take(duration, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    object TakeTime<T>(TakeTimeSeq<T> seq);
}
