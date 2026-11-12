// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="RefCountSeq{T}"/> as the target's own <c>RefCount()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T})"/>.</remarks>
    Realized<Seq<T>> RefCount<T>(RefCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountMinObserversSeq{T}"/> as the target's own
    /// <c>RefCount(minObservers)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountMinObservers<T>(RefCountMinObserversSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountDelaySeq{T}"/> as the target's own
    /// <c>RefCount(disconnectDelay)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountDelay<T>(RefCountDelaySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountDelayScheduledSeq{T}"/> as the target's own
    /// <c>RefCount(disconnectDelay, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountDelayScheduled<T>(RefCountDelayScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountMinObserversDelaySeq{T}"/> as the target's own
    /// <c>RefCount(minObservers, disconnectDelay)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountMinObserversDelay<T>(RefCountMinObserversDelaySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="RefCountMinObserversDelayScheduledSeq{T}"/> as the target's own
    /// <c>RefCount(minObservers, disconnectDelay, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> RefCountMinObserversDelayScheduled<T>(RefCountMinObserversDelayScheduledSeq<T> seq);
}
