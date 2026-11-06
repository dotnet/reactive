// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Replay</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// Sixteen overloads: with and without a selector, times the four replay-buffer shapes (unbounded,
/// by count, by time window, by both), each with and without a scheduler. The eight without a
/// selector return a <see cref="ConnectableSeq{T}"/>, which a scenario connects through
/// <see cref="ConnectableSeq{T}.ConnectAsync"/>; the eight with a selector hand the selector the
/// replayed sequence as a leaf and return an ordinary sequence. Each method builds one node of
/// the query description (one node type per overload, in this folder), and each target turns
/// that node into its own <c>Replay</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.Replay.cs</c>.
/// </remarks>
public static class ReplayExtensions
{
    /// <summary>Describes <c>source.Replay()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Replay{T}(ReplaySeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(this Seq<T> source) =>
        new ReplaySeq<T>(source);

    /// <summary>Describes <c>source.Replay(scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayScheduled{T}(ReplayScheduledSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(this Seq<T> source, SchedulerRef scheduler) =>
        new ReplayScheduledSeq<T>(source, scheduler);

    /// <summary>Describes <c>source.Replay(bufferSize)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayCount{T}(ReplayCountSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(this Seq<T> source, int bufferSize) =>
        new ReplayCountSeq<T>(source, bufferSize);

    /// <summary>Describes <c>source.Replay(bufferSize, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayCountScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayCountScheduled{T}(ReplayCountScheduledSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(
        this Seq<T> source,
        int bufferSize,
        SchedulerRef scheduler) =>
        new ReplayCountScheduledSeq<T>(source, bufferSize, scheduler);

    /// <summary>Describes <c>source.Replay(window)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayTime{T}(ReplayTimeSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(this Seq<T> source, TimeSpan window) =>
        new ReplayTimeSeq<T>(source, window);

    /// <summary>Describes <c>source.Replay(window, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayTimeScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayTimeScheduled{T}(ReplayTimeScheduledSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(
        this Seq<T> source,
        TimeSpan window,
        SchedulerRef scheduler) =>
        new ReplayTimeScheduledSeq<T>(source, window, scheduler);

    /// <summary>Describes <c>source.Replay(bufferSize, window)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayCountTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayCountTime{T}(ReplayCountTimeSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(
        this Seq<T> source,
        int bufferSize,
        TimeSpan window) =>
        new ReplayCountTimeSeq<T>(source, bufferSize, window);

    /// <summary>Describes <c>source.Replay(bufferSize, window, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
    /// <remarks>
    /// Builds a <see cref="ReplayCountTimeScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplayCountTimeScheduled{T}(ReplayCountTimeScheduledSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Replay<T>(
        this Seq<T> source,
        int bufferSize,
        TimeSpan window,
        SchedulerRef scheduler) =>
        new ReplayCountTimeScheduledSeq<T>(source, bufferSize, window, scheduler);

    /// <summary>Describes <c>source.Replay(selector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelector{T, TResult}(ReplaySelectorSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorSeq<T, TResult>(source, selector, text);

    /// <summary>Describes <c>source.Replay(selector, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="scheduler">
    /// Scheduler where connected observers within the selector function will be invoked on.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorScheduledSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorScheduled{T, TResult}(ReplaySelectorScheduledSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorScheduledSeq<T, TResult>(source, selector, scheduler, text);

    /// <summary>Describes <c>source.Replay(selector, bufferSize)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorCountSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorCount{T, TResult}(ReplaySelectorCountSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        int bufferSize,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorCountSeq<T, TResult>(source, selector, bufferSize, text);

    /// <summary>Describes <c>source.Replay(selector, bufferSize, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="scheduler">
    /// Scheduler where connected observers within the selector function will be invoked on.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorCountScheduledSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorCountScheduled{T, TResult}(ReplaySelectorCountScheduledSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        int bufferSize,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorCountScheduledSeq<T, TResult>(
            source,
            selector,
            bufferSize,
            scheduler,
            text);

    /// <summary>Describes <c>source.Replay(selector, window)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorTimeSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorTime{T, TResult}(ReplaySelectorTimeSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        TimeSpan window,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorTimeSeq<T, TResult>(source, selector, window, text);

    /// <summary>Describes <c>source.Replay(selector, window, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="scheduler">
    /// Scheduler where connected observers within the selector function will be invoked on.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorTimeScheduledSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorTimeScheduled{T, TResult}(ReplaySelectorTimeScheduledSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        TimeSpan window,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorTimeScheduledSeq<T, TResult>(source, selector, window, scheduler, text);

    /// <summary>Describes <c>source.Replay(selector, bufferSize, window)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorCountTimeSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorCountTime{T, TResult}(ReplaySelectorCountTimeSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        int bufferSize,
        TimeSpan window,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorCountTimeSeq<T, TResult>(source, selector, bufferSize, window, text);

    /// <summary>Describes <c>source.Replay(selector, bufferSize, window, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </param>
    /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
    /// <param name="window">Maximum time length of the replay buffer.</param>
    /// <param name="scheduler">
    /// Scheduler where connected observers within the selector function will be invoked on.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ReplaySelectorCountTimeScheduledSeq{T, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ReplaySelectorCountTimeScheduled{T, TResult}(ReplaySelectorCountTimeScheduledSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Replay<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        int bufferSize,
        TimeSpan window,
        SchedulerRef scheduler,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new ReplaySelectorCountTimeScheduledSeq<T, TResult>(
            source,
            selector,
            bufferSize,
            window,
            scheduler,
            text);
}
