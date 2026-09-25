// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="WindowClosingsSeq{T, TWindowClosing}"/> (built by <see cref="WindowExtensions.Window{T, TWindowClosing}(Seq{T}, Func{Seq{TWindowClosing}}, string)"/>) as the target's own <c>Window(windowClosingSelector)</c>.</summary>
    object WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq);

    /// <summary>Materializes a <see cref="WindowOpeningsSeq{T, TWindowOpening, TWindowClosing}"/> (built by <see cref="WindowExtensions.Window{T, TWindowOpening, TWindowClosing}(Seq{T}, Seq{TWindowOpening}, Func{TWindowOpening, Seq{TWindowClosing}}, string)"/>) as the target's own <c>Window(windowOpenings, windowClosingSelector)</c>.</summary>
    object WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq);

    /// <summary>Materializes a <see cref="WindowBoundariesSeq{T, TWindowBoundary}"/> (built by <see cref="WindowExtensions.Window{T, TWindowBoundary}(Seq{T}, Seq{TWindowBoundary})"/>) as the target's own <c>Window(windowBoundaries)</c>.</summary>
    object WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq);

    /// <summary>Materializes a <see cref="WindowCountSeq{T}"/> (built by <see cref="WindowExtensions.Window{T}(Seq{T}, int, int)"/>) as the target's own <c>Window(count, skip)</c>.</summary>
    object WindowCount<T>(WindowCountSeq<T> seq);

    /// <summary>Materializes a <see cref="WindowTimeSeq{T}"/> (built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, SchedulerRef)"/>) as the target's own <c>Window(timeSpan, scheduler)</c>.</summary>
    object WindowTime<T>(WindowTimeSeq<T> seq);

    /// <summary>Materializes a <see cref="WindowTimeShiftSeq{T}"/> (built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, TimeSpan, SchedulerRef)"/>) as the target's own <c>Window(timeSpan, timeShift, scheduler)</c>.</summary>
    object WindowTimeShift<T>(WindowTimeShiftSeq<T> seq);

    /// <summary>Materializes a <see cref="WindowTimeOrCountSeq{T}"/> (built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, int, SchedulerRef)"/>) as the target's own <c>Window(timeSpan, count, scheduler)</c>.</summary>
    object WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq);
}
