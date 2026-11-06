// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>
    /// Describes <c>Seq.StartAsync(functionAsync)</c> over an asynchronous function, run at the
    /// call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to run.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsync{TResult}(StartAsyncSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> StartAsync<TResult>(Func<Task<TResult>> functionAsync) =>
        new StartAsyncSeq<TResult>(functionAsync, null);

    /// <summary>
    /// Describes <c>Seq.StartAsync(functionAsync, scheduler)</c> over an asynchronous function, run
    /// at the call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to run.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsync{TResult}(StartAsyncSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> StartAsync<TResult>(Func<Task<TResult>> functionAsync,
            SchedulerRef scheduler) =>
        new StartAsyncSeq<TResult>(functionAsync, scheduler);

    /// <summary>
    /// Describes <c>Seq.StartAsync(functionAsync)</c> over an asynchronous function that takes a
    /// cancellation token, run at the call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to run, supporting cancellation.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncCancelSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncCancel{TResult}(StartAsyncCancelSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> StartAsync<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync) =>
        new StartAsyncCancelSeq<TResult>(functionAsync, null);

    /// <summary>
    /// Describes <c>Seq.StartAsync(functionAsync, scheduler)</c> over an asynchronous function that
    /// takes a cancellation token, run at the call.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="functionAsync">Asynchronous function to run, supporting cancellation.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncCancelSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncCancel{TResult}(StartAsyncCancelSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> StartAsync<TResult>(Func<CancellationToken,
            Task<TResult>> functionAsync, SchedulerRef scheduler) =>
        new StartAsyncCancelSeq<TResult>(functionAsync, scheduler);

    /// <summary>
    /// Describes <c>Seq.StartAsync(actionAsync)</c> over an asynchronous action, run at the call.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to run.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncAction(StartAsyncActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> StartAsync(Func<Task> actionAsync) =>
        new StartAsyncActionSeq(actionAsync, null);

    /// <summary>
    /// Describes <c>Seq.StartAsync(actionAsync, scheduler)</c> over an asynchronous action, run at
    /// the call.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to run.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncActionSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncAction(StartAsyncActionSeq)"/>.
    /// </remarks>
    public static Seq<Unit> StartAsync(Func<Task> actionAsync, SchedulerRef scheduler) =>
        new StartAsyncActionSeq(actionAsync, scheduler);

    /// <summary>
    /// Describes <c>Seq.StartAsync(actionAsync)</c> over an asynchronous action that takes a
    /// cancellation token, run at the call.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to run, supporting cancellation.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncActionCancelSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncActionCancel(StartAsyncActionCancelSeq)"/>.
    /// </remarks>
    public static Seq<Unit> StartAsync(Func<CancellationToken, Task> actionAsync) =>
        new StartAsyncActionCancelSeq(actionAsync, null);

    /// <summary>
    /// Describes <c>Seq.StartAsync(actionAsync, scheduler)</c> over an asynchronous action that
    /// takes a cancellation token, run at the call.
    /// </summary>
    /// <param name="actionAsync">Asynchronous action to run, supporting cancellation.</param>
    /// <param name="scheduler">Scheduler on which to notify observers.</param>
    /// <remarks>
    /// Builds a <see cref="StartAsyncActionCancelSeq"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartAsyncActionCancel(StartAsyncActionCancelSeq)"/>.
    /// </remarks>
    public static Seq<Unit> StartAsync(Func<CancellationToken, Task> actionAsync,
            SchedulerRef scheduler) =>
        new StartAsyncActionCancelSeq(actionAsync, scheduler);
}
