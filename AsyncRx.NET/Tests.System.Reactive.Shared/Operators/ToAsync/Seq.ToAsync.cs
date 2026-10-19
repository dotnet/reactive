// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>ToAsync</c> overloads, which return a function that returns a description.
/// </summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observable.ToAsync(f)(1, 2)</c>, a shared scenario writes
/// <c>Seq.ToAsync(f)(1, 2)</c>: each overload returns an ordinary function that builds the
/// node for its arguments when called, so the call reads as the original and the target makes
/// the library call, and then the call of what it returned, when the node is realized. One node
/// type per arity and kind (function or action), as the library has.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 0 arguments.</summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc0Seq{TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc0{TResult}(ToAsyncFunc0Seq{TResult})"/>.
    /// </returns>
    public static Func<Seq<TResult>> ToAsync<TResult>(Func<TResult> function) =>
        () => new ToAsyncFunc0Seq<TResult>(function, null);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 0 arguments.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc0Seq{TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc0{TResult}(ToAsyncFunc0Seq{TResult})"/>.
    /// </returns>
    public static Func<Seq<TResult>> ToAsync<TResult>(Func<TResult> function,
            SchedulerRef scheduler) =>
        () => new ToAsyncFunc0Seq<TResult>(function, scheduler);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 0 arguments.</summary>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction0Seq"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction0(ToAsyncAction0Seq)"/>.
    /// </returns>
    public static Func<Seq<Unit>> ToAsync(Action action) =>
        () => new ToAsyncAction0Seq(action, null);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 0 arguments.
    /// </summary>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction0Seq"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction0(ToAsyncAction0Seq)"/>.
    /// </returns>
    public static Func<Seq<Unit>> ToAsync(Action action, SchedulerRef scheduler) =>
        () => new ToAsyncAction0Seq(action, scheduler);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 1 argument.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc1Seq{T1, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc1{T1, TResult}(ToAsyncFunc1Seq{T1, TResult})"/>.
    /// </returns>
    public static Func<T1, Seq<TResult>> ToAsync<T1, TResult>(Func<T1, TResult> function) =>
        arg1 => new ToAsyncFunc1Seq<T1, TResult>(function, null, arg1);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 1 argument.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc1Seq{T1, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc1{T1, TResult}(ToAsyncFunc1Seq{T1, TResult})"/>.
    /// </returns>
    public static Func<T1, Seq<TResult>> ToAsync<T1, TResult>(Func<T1, TResult> function,
            SchedulerRef scheduler) =>
        arg1 => new ToAsyncFunc1Seq<T1, TResult>(function, scheduler, arg1);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 1 argument.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction1Seq{T1}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction1{T1}(ToAsyncAction1Seq{T1})"/>.
    /// </returns>
    public static Func<T1, Seq<Unit>> ToAsync<T1>(Action<T1> action) =>
        arg1 => new ToAsyncAction1Seq<T1>(action, null, arg1);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 1 argument.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction1Seq{T1}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction1{T1}(ToAsyncAction1Seq{T1})"/>.
    /// </returns>
    public static Func<T1, Seq<Unit>> ToAsync<T1>(Action<T1> action, SchedulerRef scheduler) =>
        arg1 => new ToAsyncAction1Seq<T1>(action, scheduler, arg1);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 2 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc2Seq{T1, T2, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc2{T1, T2, TResult}(ToAsyncFunc2Seq{T1, T2, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2,
            TResult> function) =>
        (arg1, arg2) => new ToAsyncFunc2Seq<T1, T2, TResult>(function, null, arg1, arg2);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 2 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc2Seq{T1, T2, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc2{T1, T2, TResult}(ToAsyncFunc2Seq{T1, T2, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2,
            TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2) => new ToAsyncFunc2Seq<T1, T2, TResult>(function, scheduler, arg1, arg2);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 2 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction2Seq{T1, T2}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction2{T1, T2}(ToAsyncAction2Seq{T1, T2})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<Unit>> ToAsync<T1, T2>(Action<T1, T2> action) =>
        (arg1, arg2) => new ToAsyncAction2Seq<T1, T2>(action, null, arg1, arg2);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 2 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction2Seq{T1, T2}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction2{T1, T2}(ToAsyncAction2Seq{T1, T2})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<Unit>> ToAsync<T1, T2>(Action<T1, T2> action,
            SchedulerRef scheduler) =>
        (arg1, arg2) => new ToAsyncAction2Seq<T1, T2>(action, scheduler, arg1, arg2);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 3 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc3Seq{T1, T2, T3, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc3{T1, T2, T3, TResult}(ToAsyncFunc3Seq{T1, T2, T3, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3,
            TResult> function) =>
        (arg1, arg2, arg3) => new ToAsyncFunc3Seq<T1, T2, T3, TResult>(function, null, arg1, arg2,
                arg3);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 3 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc3Seq{T1, T2, T3, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc3{T1, T2, T3, TResult}(ToAsyncFunc3Seq{T1, T2, T3, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3,
            TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3) => new ToAsyncFunc3Seq<T1, T2, T3, TResult>(function, scheduler, arg1,
                arg2, arg3);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 3 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction3Seq{T1, T2, T3}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction3{T1, T2, T3}(ToAsyncAction3Seq{T1, T2, T3})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action) =>
        (arg1, arg2, arg3) => new ToAsyncAction3Seq<T1, T2, T3>(action, null, arg1, arg2, arg3);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 3 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction3Seq{T1, T2, T3}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction3{T1, T2, T3}(ToAsyncAction3Seq{T1, T2, T3})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3) => new ToAsyncAction3Seq<T1, T2, T3>(action, scheduler, arg1, arg2,
                arg3);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 4 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc4Seq{T1, T2, T3, T4, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc4{T1, T2, T3, T4, TResult}(ToAsyncFunc4Seq{T1, T2, T3, T4, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2,
            T3, T4, TResult> function) =>
        (arg1, arg2, arg3, arg4) => new ToAsyncFunc4Seq<T1, T2, T3, T4, TResult>(function, null,
                arg1, arg2, arg3, arg4);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 4 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc4Seq{T1, T2, T3, T4, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc4{T1, T2, T3, T4, TResult}(ToAsyncFunc4Seq{T1, T2, T3, T4, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2,
            T3, T4, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4) => new ToAsyncFunc4Seq<T1, T2, T3, T4, TResult>(function,
                scheduler, arg1, arg2, arg3, arg4);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 4 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction4Seq{T1, T2, T3, T4}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction4{T1, T2, T3, T4}(ToAsyncAction4Seq{T1, T2, T3, T4})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3,
            T4> action) =>
        (arg1, arg2, arg3, arg4) => new ToAsyncAction4Seq<T1, T2, T3, T4>(action, null, arg1, arg2,
                arg3, arg4);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 4 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction4Seq{T1, T2, T3, T4}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction4{T1, T2, T3, T4}(ToAsyncAction4Seq{T1, T2, T3, T4})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3,
            T4> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4) => new ToAsyncAction4Seq<T1, T2, T3, T4>(action, scheduler, arg1,
                arg2, arg3, arg4);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 5 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc5Seq{T1, T2, T3, T4, T5, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc5{T1, T2, T3, T4, T5, TResult}(ToAsyncFunc5Seq{T1, T2, T3, T4, T5, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<
            T1, T2, T3, T4, T5, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5) => new ToAsyncFunc5Seq<T1, T2, T3, T4, T5, TResult>(function,
                null, arg1, arg2, arg3, arg4, arg5);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 5 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc5Seq{T1, T2, T3, T4, T5, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc5{T1, T2, T3, T4, T5, TResult}(ToAsyncFunc5Seq{T1, T2, T3, T4, T5, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<
            T1, T2, T3, T4, T5, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5) => new ToAsyncFunc5Seq<T1, T2, T3, T4, T5, TResult>(function,
                scheduler, arg1, arg2, arg3, arg4, arg5);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 5 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction5Seq{T1, T2, T3, T4, T5}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction5{T1, T2, T3, T4, T5}(ToAsyncAction5Seq{T1, T2, T3, T4, T5})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3,
            T4, T5> action) =>
        (arg1, arg2, arg3, arg4, arg5) => new ToAsyncAction5Seq<T1, T2, T3, T4, T5>(action, null,
                arg1, arg2, arg3, arg4, arg5);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 5 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction5Seq{T1, T2, T3, T4, T5}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction5{T1, T2, T3, T4, T5}(ToAsyncAction5Seq{T1, T2, T3, T4, T5})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3,
            T4, T5> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5) => new ToAsyncAction5Seq<T1, T2, T3, T4, T5>(action,
                scheduler, arg1, arg2, arg3, arg4, arg5);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 6 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc6Seq{T1, T2, T3, T4, T5, T6, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc6{T1, T2, T3, T4, T5, T6, TResult}(ToAsyncFunc6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6,
            TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new ToAsyncFunc6Seq<T1, T2, T3, T4, T5, T6,
                TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 6 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc6Seq{T1, T2, T3, T4, T5, T6, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc6{T1, T2, T3, T4, T5, T6, TResult}(ToAsyncFunc6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6,
            TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new ToAsyncFunc6Seq<T1, T2, T3, T4, T5, T6,
                TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 6 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction6Seq{T1, T2, T3, T4, T5, T6}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction6{T1, T2, T3, T4, T5, T6}(ToAsyncAction6Seq{T1, T2, T3, T4, T5, T6})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1,
            T2, T3, T4, T5, T6> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new ToAsyncAction6Seq<T1, T2, T3, T4, T5, T6>(
                action, null, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 6 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction6Seq{T1, T2, T3, T4, T5, T6}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction6{T1, T2, T3, T4, T5, T6}(ToAsyncAction6Seq{T1, T2, T3, T4, T5, T6})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1,
            T2, T3, T4, T5, T6> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new ToAsyncAction6Seq<T1, T2, T3, T4, T5, T6>(
                action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 7 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc7{T1, T2, T3, T4, T5, T6, T7, TResult}(ToAsyncFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new ToAsyncFunc7Seq<T1, T2, T3, T4, T5, T6,
                T7, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 7 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc7{T1, T2, T3, T4, T5, T6, T7, TResult}(ToAsyncFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new ToAsyncFunc7Seq<T1, T2, T3, T4, T5, T6,
                T7, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 7 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction7Seq{T1, T2, T3, T4, T5, T6, T7}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction7{T1, T2, T3, T4, T5, T6, T7}(ToAsyncAction7Seq{T1, T2, T3, T4, T5, T6, T7})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            Action<T1, T2, T3, T4, T5, T6, T7> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new ToAsyncAction7Seq<T1, T2, T3, T4, T5, T6,
                T7>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 7 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction7Seq{T1, T2, T3, T4, T5, T6, T7}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction7{T1, T2, T3, T4, T5, T6, T7}(ToAsyncAction7Seq{T1, T2, T3, T4, T5, T6, T7})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(
            Action<T1, T2, T3, T4, T5, T6, T7> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new ToAsyncAction7Seq<T1, T2, T3, T4, T5, T6,
                T7>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 8 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(ToAsyncFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new ToAsyncFunc8Seq<T1, T2, T3, T4, T5,
                T6, T7, T8, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 8 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(ToAsyncFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new ToAsyncFunc8Seq<T1, T2, T3, T4, T5,
                T6, T7, T8, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 8 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction8{T1, T2, T3, T4, T5, T6, T7, T8}(ToAsyncAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new ToAsyncAction8Seq<T1, T2, T3, T4,
                T5, T6, T7, T8>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 8 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction8{T1, T2, T3, T4, T5, T6, T7, T8}(ToAsyncAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6,
            T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new ToAsyncAction8Seq<T1, T2, T3, T4,
                T5, T6, T7, T8>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 9 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc9{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}(ToAsyncFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new ToAsyncFunc9Seq<T1, T2, T3,
                T4, T5, T6, T7, T8, T9, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6,
                arg7, arg8, arg9);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 9 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc9{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}(ToAsyncFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<TResult>> ToAsync<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new ToAsyncFunc9Seq<T1, T2, T3,
                T4, T5, T6, T7, T8, T9, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5,
                arg6, arg7, arg8, arg9);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 9 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction9{T1, T2, T3, T4, T5, T6, T7, T8, T9}(ToAsyncAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5,
            T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new ToAsyncAction9Seq<T1, T2, T3,
                T4, T5, T6, T7, T8, T9>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8, arg9);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 9 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction9{T1, T2, T3, T4, T5, T6, T7, T8, T9}(ToAsyncAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<Unit>> ToAsync<T1, T2, T3, T4, T5,
            T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new ToAsyncAction9Seq<T1, T2, T3,
                T4, T5, T6, T7, T8, T9>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8, arg9);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 10 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}(ToAsyncFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<TResult>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new ToAsyncFunc10Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(function, null, arg1, arg2, arg3,
                arg4, arg5, arg6, arg7, arg8, arg9, arg10);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 10 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}(ToAsyncFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<TResult>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new ToAsyncFunc10Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(function, scheduler, arg1, arg2, arg3,
                arg4, arg5, arg6, arg7, arg8, arg9, arg10);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 10 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(ToAsyncAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<Unit>> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new ToAsyncAction10Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, null, arg1, arg2, arg3, arg4, arg5,
                arg6, arg7, arg8, arg9, arg10);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 10 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(ToAsyncAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<Unit>> ToAsync<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => new ToAsyncAction10Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, scheduler, arg1, arg2, arg3, arg4,
                arg5, arg6, arg7, arg8, arg9, arg10);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 11 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(ToAsyncFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<TResult>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new ToAsyncFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 11 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(ToAsyncFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<TResult>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new ToAsyncFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 11 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(ToAsyncAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<Unit>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new ToAsyncAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 11 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(ToAsyncAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<Unit>> ToAsync<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new ToAsyncAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 12 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}(ToAsyncFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Seq<TResult>> ToAsync<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new ToAsyncFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 12 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}(ToAsyncFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Seq<TResult>> ToAsync<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new ToAsyncFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 12 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(ToAsyncAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Seq<Unit>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new ToAsyncAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 12 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(ToAsyncAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Seq<Unit>> ToAsync<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11, T12> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new ToAsyncAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11, arg12);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 13 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}(ToAsyncFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new ToAsyncFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12, arg13);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 13 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}(ToAsyncFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new ToAsyncFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 13 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Seq<Unit>> ToAsync<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new ToAsyncAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11, arg12, arg13);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 13 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Seq<Unit>> ToAsync<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10, T11, T12, T13> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new ToAsyncAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11, arg12, arg13);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 14 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}(ToAsyncFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new ToAsyncFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13, arg14);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 14 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}(ToAsyncFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new ToAsyncFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8, arg9, arg10, arg11, arg12, arg13, arg14);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 14 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(ToAsyncAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new ToAsyncAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11, arg12, arg13, arg14);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 14 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(ToAsyncAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new ToAsyncAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12, arg13, arg14);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 15 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}(ToAsyncFunc15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15) => new ToAsyncFunc15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15, TResult>(function, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 15 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}(ToAsyncFunc15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15) => new ToAsyncFunc15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5, arg6,
                arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 15 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}(ToAsyncAction15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15) => new ToAsyncAction15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15>(action, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12, arg13, arg14, arg15);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 15 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}(ToAsyncAction15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15) => new ToAsyncAction15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
                T13, T14, T15>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13, arg14, arg15);

    /// <summary>Describes <c>Seq.ToAsync(function)</c> over a function of 16 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}(ToAsyncFunc16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult> function) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16) => new ToAsyncFunc16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                T12, T13, T14, T15, T16, TResult>(function, null, arg1, arg2, arg3, arg4, arg5,
                arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);

    /// <summary>
    /// Describes <c>Seq.ToAsync(function, scheduler)</c> over a function of 16 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the function.</typeparam>
    /// <param name="function">Function to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the function on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncFunc16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncFunc16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}(ToAsyncFunc16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Seq<
            TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult> function, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16) => new ToAsyncFunc16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
                T12, T13, T14, T15, T16, TResult>(function, scheduler, arg1, arg2, arg3, arg4, arg5,
                arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);

    /// <summary>Describes <c>Seq.ToAsync(action)</c> over an action of 16 arguments.</summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}(ToAsyncAction16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> action) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16) => new ToAsyncAction16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13, T14, T15, T16>(action, null, arg1, arg2, arg3, arg4, arg5, arg6,
                arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);

    /// <summary>
    /// Describes <c>Seq.ToAsync(action, scheduler)</c> over an action of 16 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="T9">The type of the ninth argument passed to the function.</typeparam>
    /// <typeparam name="T10">The type of the tenth argument passed to the function.</typeparam>
    /// <typeparam name="T11">The type of the eleventh argument passed to the function.</typeparam>
    /// <typeparam name="T12">The type of the twelfth argument passed to the function.</typeparam>
    /// <typeparam name="T13">
    /// The type of the thirteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T14">
    /// The type of the fourteenth argument passed to the function.
    /// </typeparam>
    /// <typeparam name="T15">The type of the fifteenth argument passed to the function.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth argument passed to the function.</typeparam>
    /// <param name="action">Action to convert to an asynchronous function.</param>
    /// <param name="scheduler">Scheduler to invoke the action on.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="ToAsyncAction16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.ToAsyncAction16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}(ToAsyncAction16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            Seq<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> action, SchedulerRef scheduler) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14,
                arg15, arg16) => new ToAsyncAction16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13, T14, T15, T16>(action, scheduler, arg1, arg2, arg3, arg4, arg5, arg6,
                arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
}
