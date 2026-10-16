// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ReplaySeq{T}"/> as the target's own
    /// <c>Replay()</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T})"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> Replay<T>(ReplaySeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayScheduledSeq{T}"/> as the target's own
    /// <c>Replay(scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, SchedulerRef)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayScheduled<T>(ReplayScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayCountSeq{T}"/> as the target's own
    /// <c>Replay(bufferSize)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayCount<T>(ReplayCountSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayCountScheduledSeq{T}"/> as the target's own
    /// <c>Replay(bufferSize, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int, SchedulerRef)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayCountScheduled<T>(ReplayCountScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayTimeSeq{T}"/> as the target's own
    /// <c>Replay(window)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, TimeSpan)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayTime<T>(ReplayTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayTimeScheduledSeq{T}"/> as the target's own
    /// <c>Replay(window, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, TimeSpan, SchedulerRef)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayTimeScheduled<T>(ReplayTimeScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayCountTimeSeq{T}"/> as the target's own
    /// <c>Replay(bufferSize, window)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int, TimeSpan)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayCountTime<T>(ReplayCountTimeSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplayCountTimeScheduledSeq{T}"/> as the target's own
    /// <c>Replay(bufferSize, window, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int, TimeSpan, SchedulerRef)"/>.
    /// The realization is the target's connectable observable.
    /// </remarks>
    Realized<Seq<T>> ReplayCountTimeScheduled<T>(ReplayCountTimeScheduledSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelector<T, TResult>(ReplaySelectorSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorScheduledSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, SchedulerRef, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorScheduled<T, TResult>(
        ReplaySelectorScheduledSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorCountSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, bufferSize)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, int, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorCount<T, TResult>(ReplaySelectorCountSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorCountScheduledSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, bufferSize, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, int, SchedulerRef, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorCountScheduled<T, TResult>(
        ReplaySelectorCountScheduledSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorTimeSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, window)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, TimeSpan, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorTime<T, TResult>(ReplaySelectorTimeSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorTimeScheduledSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, window, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, TimeSpan, SchedulerRef, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorTimeScheduled<T, TResult>(
        ReplaySelectorTimeScheduledSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorCountTimeSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, bufferSize, window)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, int, TimeSpan, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorCountTime<T, TResult>(
        ReplaySelectorCountTimeSeq<T, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ReplaySelectorCountTimeScheduledSeq{T, TResult}"/> as the target's own
    /// <c>Replay(selector, bufferSize, window, scheduler)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, int, TimeSpan, SchedulerRef, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> ReplaySelectorCountTimeScheduled<T, TResult>(
        ReplaySelectorCountTimeScheduledSeq<T, TResult> seq);
}
