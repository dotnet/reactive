// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TimeoutTimeSeq{T}"/> as the target's own
    /// <c>Timeout(dueTime[, scheduler])</c> (relative).
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
    /// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutTime<T>(TimeoutTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutTimeOtherSeq{T}"/> as the target's own
    /// <c>Timeout(dueTime, other[, scheduler])</c> (relative).
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, Seq{T}, SchedulerRef)"/> or
    /// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, Seq{T})"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutTimeOther<T>(TimeoutTimeOtherSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutAbsoluteSeq{T}"/> as the target's own
    /// <c>Timeout(dueTime[, scheduler])</c> (absolute).
    /// </summary>
    /// <remarks>
    /// Built by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
    /// or <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutAbsolute<T>(TimeoutAbsoluteSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutAbsoluteOtherSeq{T}"/> as the target's own
    /// <c>Timeout(dueTime, other[, scheduler])</c> (absolute).
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset, Seq{T}, SchedulerRef)"/> or
    /// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset, Seq{T})"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutAbsoluteOther<T>(TimeoutAbsoluteOtherSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutDurationSeq{T, TTimeout}"/> as the target's own
    /// <c>Timeout(timeoutDurationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Func{T, Seq{TTimeout}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutDuration<T, TTimeout>(TimeoutDurationSeq<T, TTimeout> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutDurationOtherSeq{T, TTimeout}"/> as the target's own
    /// <c>Timeout(timeoutDurationSelector, other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Func{T, Seq{TTimeout}}, Seq{T}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutDurationOther<T, TTimeout>(TimeoutDurationOtherSeq<T, TTimeout> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutFirstDurationSeq{T, TTimeout}"/> as the target's own
    /// <c>Timeout(firstTimeout, timeoutDurationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Seq{TTimeout}, Func{T, Seq{TTimeout}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutFirstDuration<T, TTimeout>(TimeoutFirstDurationSeq<T, TTimeout> seq);

    /// <summary>
    /// Materializes a <see cref="TimeoutFirstDurationOtherSeq{T, TTimeout}"/> as the target's
    /// own <c>Timeout(firstTimeout, timeoutDurationSelector, other)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Seq{TTimeout}, Func{T, Seq{TTimeout}}, Seq{T}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> TimeoutFirstDurationOther<T, TTimeout>(
        TimeoutFirstDurationOtherSeq<T, TTimeout> seq);
}
