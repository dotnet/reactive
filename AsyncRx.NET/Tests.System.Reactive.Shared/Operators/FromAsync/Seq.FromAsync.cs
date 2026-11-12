// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>
    /// Describes <c>Seq.FromAsync(functionAsync)</c> over an asynchronous function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to convert.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsync{TResult}(FromAsyncSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync) =>
        new FromAsyncSeq<TResult>(functionAsync, null);

    /// <summary>
    /// Describes <c>Seq.FromAsync(functionAsync, scheduler)</c> over an asynchronous function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to convert.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsync{TResult}(FromAsyncSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync,
            SchedulerRef scheduler) =>
        new FromAsyncSeq<TResult>(functionAsync, scheduler);

    /// <summary>
    /// Describes <c>Seq.FromAsync(functionAsync)</c> over an asynchronous function that takes a
    /// cancellation token.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">
    /// Asynchronous function to convert, supporting cancellation.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncCancelSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncCancel{TResult}(FromAsyncCancelSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> FromAsync<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        new FromAsyncCancelSeq<TResult>(functionAsync, null);

    /// <summary>
    /// Describes <c>Seq.FromAsync(functionAsync, scheduler)</c> over an asynchronous function that
    /// takes a cancellation token.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">
    /// Asynchronous function to convert, supporting cancellation.
    /// </param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncCancelSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncCancel{TResult}(FromAsyncCancelSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> FromAsync<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, SchedulerRef scheduler) =>
        new FromAsyncCancelSeq<TResult>(functionAsync, scheduler);

    /// <summary>Describes <c>Seq.FromAsync(actionAsync)</c> over an asynchronous action.</summary>
    /// <param name="actionAsync">Asynchronous action to convert.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncAction(FromAsyncActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> FromAsync(Func<Task> actionAsync) =>
        new FromAsyncActionSeq(actionAsync, null);

    /// <summary>
    /// Describes <c>Seq.FromAsync(actionAsync, scheduler)</c> over an asynchronous action.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to convert.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncAction(FromAsyncActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> FromAsync(Func<Task> actionAsync, SchedulerRef scheduler) =>
        new FromAsyncActionSeq(actionAsync, scheduler);

    /// <summary>
    /// Describes <c>Seq.FromAsync(actionAsync)</c> over an asynchronous action that takes a
    /// cancellation token.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to convert, supporting cancellation.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncActionCancelSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncActionCancel(FromAsyncActionCancelSeq)"/>.
    /// </remarks>
    public static Seq<Unit> FromAsync(Func<CancellationToken, Task> actionAsync) =>
        new FromAsyncActionCancelSeq(actionAsync, null);

    /// <summary>
    /// Describes <c>Seq.FromAsync(actionAsync, scheduler)</c> over an asynchronous action that
    /// takes a cancellation token.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to convert, supporting cancellation.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="FromAsyncActionCancelSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncActionCancel(FromAsyncActionCancelSeq)"/>.
    /// </remarks>
    public static Seq<Unit> FromAsync(Func<CancellationToken, Task> actionAsync,
            SchedulerRef scheduler) =>
        new FromAsyncActionCancelSeq(actionAsync, scheduler);
}
