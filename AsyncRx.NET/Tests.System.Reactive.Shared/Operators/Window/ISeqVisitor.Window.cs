// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="WindowClosingsSeq{T, TWindowClosing}"/> as the target's own
    /// <c>Window(windowClosingSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="WindowExtensions.Window{T, TWindowClosing}(Seq{T}, Func{Seq{TWindowClosing}}, string)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq);

    /// <summary>
    /// Materializes a <see cref="WindowOpeningsSeq{T, TWindowOpening, TWindowClosing}"/> as the
    /// target's own <c>Window(windowOpenings, windowClosingSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="WindowExtensions.Window{T, TWindowOpening, TWindowClosing}(Seq{T}, Seq{TWindowOpening}, Func{TWindowOpening, Seq{TWindowClosing}}, string)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq);

    /// <summary>
    /// Materializes a <see cref="WindowBoundariesSeq{T, TWindowBoundary}"/> as the target's own
    /// <c>Window(windowBoundaries)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="WindowExtensions.Window{T, TWindowBoundary}(Seq{T}, Seq{TWindowBoundary})"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq);

    /// <summary>
    /// Materializes a <see cref="WindowCountSeq{T}"/> as the target's own
    /// <c>Window(count, skip)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="WindowExtensions.Window{T}(Seq{T}, int, int)"/>.</remarks>
    Realized<Seq<Seq<T>>> WindowCount<T>(WindowCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeSeq{T}"/> as the target's own
    /// <c>Window(timeSpan, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowTime<T>(WindowTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeShiftSeq{T}"/> as the target's own
    /// <c>Window(timeSpan, timeShift, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, TimeSpan, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowTimeShift<T>(WindowTimeShiftSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeOrCountSeq{T}"/> as the target's own
    /// <c>Window(timeSpan, count, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, int, SchedulerRef)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowCountOnlySeq{T}"/> as the target's own <c>Window(count)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="WindowExtensions.Window{T}(Seq{T}, int)"/>.</remarks>
    Realized<Seq<Seq<T>>> WindowCountOnly<T>(WindowCountOnlySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeDefaultSeq{T}"/> as the target's own
    /// <c>Window(timeSpan)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan)"/>.</remarks>
    Realized<Seq<Seq<T>>> WindowTimeDefault<T>(WindowTimeDefaultSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeShiftDefaultSeq{T}"/> as the target's own
    /// <c>Window(timeSpan, timeShift)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, TimeSpan)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowTimeShiftDefault<T>(WindowTimeShiftDefaultSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="WindowTimeOrCountDefaultSeq{T}"/> as the target's own
    /// <c>Window(timeSpan, count)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, int)"/>.
    /// </remarks>
    Realized<Seq<Seq<T>>> WindowTimeOrCountDefault<T>(WindowTimeOrCountDefaultSeq<T> seq);
}
