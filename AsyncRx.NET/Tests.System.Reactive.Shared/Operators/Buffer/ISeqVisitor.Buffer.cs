// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="BufferClosingsSeq{T, TBufferClosing}"/> as the target's own
    /// <c>Buffer(bufferClosingSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="BufferExtensions.Buffer{T, TBufferClosing}(Seq{T}, Func{Seq{TBufferClosing}}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferClosings<T, TBufferClosing>(
        BufferClosingsSeq<T, TBufferClosing> seq);

    /// <summary>
    /// Materializes a <see cref="BufferOpeningsSeq{T, TBufferOpening, TBufferClosing}"/> as the
    /// target's own <c>Buffer(bufferOpenings, bufferClosingSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="BufferExtensions.Buffer{T, TBufferOpening, TBufferClosing}(Seq{T}, Seq{TBufferOpening}, Func{TBufferOpening, Seq{TBufferClosing}}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferOpenings<T, TBufferOpening, TBufferClosing>(
        BufferOpeningsSeq<T, TBufferOpening, TBufferClosing> seq);

    /// <summary>
    /// Materializes a <see cref="BufferBoundariesSeq{T, TBufferBoundary}"/> as the target's own
    /// <c>Buffer(bufferBoundaries)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="BufferExtensions.Buffer{T, TBufferBoundary}(Seq{T}, Seq{TBufferBoundary})"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferBoundaries<T, TBufferBoundary>(
        BufferBoundariesSeq<T, TBufferBoundary> seq);

    /// <summary>
    /// Materializes a <see cref="BufferCountSeq{T}"/> as the target's own
    /// <c>Buffer(count, skip)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, int, int)"/>.</remarks>
    Realized<Seq<IList<T>>> BufferCount<T>(BufferCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferTime<T>(BufferTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeShiftSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan, timeShift, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferTimeShift<T>(BufferTimeShiftSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeOrCountSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan, count, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, int, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferTimeOrCount<T>(BufferTimeOrCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferCountOnlySeq{T}"/> as the target's own <c>Buffer(count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<IList<T>>> BufferCountOnly<T>(BufferCountOnlySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeDefaultSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan)"/>.</remarks>
    Realized<Seq<IList<T>>> BufferTimeDefault<T>(BufferTimeDefaultSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeShiftDefaultSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan, timeShift)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferTimeShiftDefault<T>(BufferTimeShiftDefaultSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="BufferTimeOrCountDefaultSeq{T}"/> as the target's own
    /// <c>Buffer(timeSpan, count)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, int)"/>.
    /// </remarks>
    Realized<Seq<IList<T>>> BufferTimeOrCountDefault<T>(BufferTimeOrCountDefaultSeq<T> seq);
}
