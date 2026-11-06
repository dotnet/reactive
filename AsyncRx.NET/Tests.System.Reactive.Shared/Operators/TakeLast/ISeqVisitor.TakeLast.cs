// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TakeLastCountSeq{T}"/> as the target's own
    /// <c>TakeLast(count[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, int, SchedulerRef)"/> or
    /// <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, int)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeLastCount<T>(TakeLastCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeLastTimeSeq{T}"/> as the target's own
    /// <c>TakeLast(duration[, scheduler])</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeLastTime<T>(TakeLastTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TakeLastTimeLoopSeq{T}"/> as the target's own
    /// <c>TakeLast(duration, timerScheduler, loopScheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan, SchedulerRef, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<T>> TakeLastTimeLoop<T>(TakeLastTimeLoopSeq<T> seq);
}
