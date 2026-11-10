// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>FromAsyncPattern</c> overloads, which return a function that returns a description.
/// </summary>
/// <remarks>
/// As <c>Seq.ToAsync</c>: where an Rx.NET test writes <c>Observable.FromAsyncPattern(begin,
/// end)(2, 3)</c>, a shared scenario writes <c>Seq.FromAsyncPattern(begin, end)(2, 3)</c>. The
/// Begin delegate's state parameter is <c>object?</c>, as Rx.NET declares it.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 0 arguments.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc0Seq{TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc0{TResult}(FromAsyncPatternFunc0Seq{TResult})"/>.
    /// </returns>
    public static Func<Seq<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        () => new FromAsyncPatternFunc0Seq<TResult>(begin, end);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 0 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction0Seq"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction0(FromAsyncPatternAction0Seq)"/>.
    /// </returns>
    public static Func<Seq<Unit>> FromAsyncPattern(Func<AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        () => new FromAsyncPatternAction0Seq(begin, end);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 1 argument.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc1Seq{T1, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc1{T1, TResult}(FromAsyncPatternFunc1Seq{T1, TResult})"/>.
    /// </returns>
    public static Func<T1, Seq<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback,
            object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        arg1 => new FromAsyncPatternFunc1Seq<T1, TResult>(begin, end, arg1);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 1 argument whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction1Seq{T1}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction1{T1}(FromAsyncPatternAction1Seq{T1})"/>.
    /// </returns>
    public static Func<T1, Seq<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        arg1 => new FromAsyncPatternAction1Seq<T1>(begin, end, arg1);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 2 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc2Seq{T1, T2, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc2{T1, T2, TResult}(FromAsyncPatternFunc2Seq{T1, T2, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2) => new FromAsyncPatternFunc2Seq<T1, T2, TResult>(begin, end, arg1, arg2);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 2 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction2Seq{T1, T2}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction2{T1, T2}(FromAsyncPatternAction2Seq{T1, T2})"/>.
    /// </returns>
    public static Func<T1, T2, Seq<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback,
            object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2) => new FromAsyncPatternAction2Seq<T1, T2>(begin, end, arg1, arg2);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 3 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc3Seq{T1, T2, T3, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc3{T1, T2, T3, TResult}(FromAsyncPatternFunc3Seq{T1, T2, T3, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(Func<T1, T2,
            T3, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3) => new FromAsyncPatternFunc3Seq<T1, T2, T3, TResult>(begin, end, arg1,
                arg2, arg3);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 3 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction3Seq{T1, T2, T3}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction3{T1, T2, T3}(FromAsyncPatternAction3Seq{T1, T2, T3})"/>.
    /// </returns>
    public static Func<T1, T2, T3, Seq<Unit>> FromAsyncPattern<T1, T2, T3>(Func<T1, T2, T3,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3) => new FromAsyncPatternAction3Seq<T1, T2, T3>(begin, end, arg1, arg2,
                arg3);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 4 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc4Seq{T1, T2, T3, T4, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc4{T1, T2, T3, T4, TResult}(FromAsyncPatternFunc4Seq{T1, T2, T3, T4, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<TResult>> FromAsyncPattern<T1, T2, T3, T4, TResult>(Func<
            T1, T2, T3, T4, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult,
            TResult> end) =>
        (arg1, arg2, arg3, arg4) => new FromAsyncPatternFunc4Seq<T1, T2, T3, T4, TResult>(begin,
                end, arg1, arg2, arg3, arg4);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 4 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction4Seq{T1, T2, T3, T4}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction4{T1, T2, T3, T4}(FromAsyncPatternAction4Seq{T1, T2, T3, T4})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4>(Func<T1, T2, T3,
            T4, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4) => new FromAsyncPatternAction4Seq<T1, T2, T3, T4>(begin, end, arg1,
                arg2, arg3, arg4);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 5 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc5Seq{T1, T2, T3, T4, T5, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc5{T1, T2, T3, T4, T5, TResult}(FromAsyncPatternFunc5Seq{T1, T2, T3, T4, T5, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5,
            TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5) => new FromAsyncPatternFunc5Seq<T1, T2, T3, T4, T5, TResult>(
                begin, end, arg1, arg2, arg3, arg4, arg5);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 5 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction5Seq{T1, T2, T3, T4, T5}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction5{T1, T2, T3, T4, T5}(FromAsyncPatternAction5Seq{T1, T2, T3, T4, T5})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5>(Func<T1,
            T2, T3, T4, T5, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5) => new FromAsyncPatternAction5Seq<T1, T2, T3, T4, T5>(begin,
                end, arg1, arg2, arg3, arg4, arg5);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 6 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc6Seq{T1, T2, T3, T4, T5, T6, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc6{T1, T2, T3, T4, T5, T6, TResult}(FromAsyncPatternFunc6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5,
            T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?, IAsyncResult> begin,
            Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new FromAsyncPatternFunc6Seq<T1, T2, T3, T4, T5, T6,
                TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 6 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction6Seq{T1, T2, T3, T4, T5, T6}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction6{T1, T2, T3, T4, T5, T6}(FromAsyncPatternAction6Seq{T1, T2, T3, T4, T5, T6})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6>(
            Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6) => new FromAsyncPatternAction6Seq<T1, T2, T3, T4, T5,
                T6>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 7 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc7{T1, T2, T3, T4, T5, T6, T7, TResult}(FromAsyncPatternFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<TResult>> FromAsyncPattern<T1, T2, T3, T4,
            T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new FromAsyncPatternFunc7Seq<T1, T2, T3, T4,
                T5, T6, T7, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 7 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction7Seq{T1, T2, T3, T4, T5, T6, T7}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction7{T1, T2, T3, T4, T5, T6, T7}(FromAsyncPatternAction7Seq{T1, T2, T3, T4, T5, T6, T7})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5,
            T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object?, IAsyncResult> begin,
            Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => new FromAsyncPatternAction7Seq<T1, T2, T3, T4,
                T5, T6, T7>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 8 arguments.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(FromAsyncPatternFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<TResult>> FromAsyncPattern<T1, T2, T3,
            T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback,
            object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new FromAsyncPatternFunc8Seq<T1, T2, T3,
                T4, T5, T6, T7, T8, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 8 arguments whose
    /// End returns nothing.
    /// </summary>
    /// <typeparam name="T1">The type of the first argument passed to the function.</typeparam>
    /// <typeparam name="T2">The type of the second argument passed to the function.</typeparam>
    /// <typeparam name="T3">The type of the third argument passed to the function.</typeparam>
    /// <typeparam name="T4">The type of the fourth argument passed to the function.</typeparam>
    /// <typeparam name="T5">The type of the fifth argument passed to the function.</typeparam>
    /// <typeparam name="T6">The type of the sixth argument passed to the function.</typeparam>
    /// <typeparam name="T7">The type of the seventh argument passed to the function.</typeparam>
    /// <typeparam name="T8">The type of the eighth argument passed to the function.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction8{T1, T2, T3, T4, T5, T6, T7, T8}(FromAsyncPatternAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4,
            T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => new FromAsyncPatternAction8Seq<T1, T2,
                T3, T4, T5, T6, T7, T8>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 9 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc9{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}(FromAsyncPatternFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<TResult>> FromAsyncPattern<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new FromAsyncPatternFunc9Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5,
                arg6, arg7, arg8, arg9);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 9 arguments whose
    /// End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction9{T1, T2, T3, T4, T5, T6, T7, T8, T9}(FromAsyncPatternAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Seq<Unit>> FromAsyncPattern<T1, T2, T3,
            T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => new FromAsyncPatternAction9Seq<T1,
                T2, T3, T4, T5, T6, T7, T8, T9>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6,
                arg7, arg8, arg9);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 10 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}(FromAsyncPatternFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<TResult>> FromAsyncPattern<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10) => new FromAsyncPatternFunc10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 10 arguments
    /// whose End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(FromAsyncPatternAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Seq<Unit>> FromAsyncPattern<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10) => new FromAsyncPatternAction10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 11 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(FromAsyncPatternFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<TResult>> FromAsyncPattern<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult,
            TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new FromAsyncPatternFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 11 arguments
    /// whose End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(FromAsyncPatternAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Seq<Unit>> FromAsyncPattern<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, AsyncCallback, object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11) => new FromAsyncPatternAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 12 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}(FromAsyncPatternFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Seq<
            TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object?,
            IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new FromAsyncPatternFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 12 arguments
    /// whose End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(FromAsyncPatternAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11,
                arg12) => new FromAsyncPatternAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,
                arg11, arg12);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 13 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}(FromAsyncPatternFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Seq<
            TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback,
            object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new FromAsyncPatternFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 13 arguments
    /// whose End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(FromAsyncPatternAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object?,
            IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12,
                arg13) => new FromAsyncPatternAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9,
                arg10, arg11, arg12, arg13);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 14 arguments.
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
    /// <typeparam name="TResult">The type of the result returned by the End delegate.</typeparam>
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternFunc14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}(FromAsyncPatternFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Seq<
            TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            AsyncCallback, object?, IAsyncResult> begin, Func<IAsyncResult, TResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new FromAsyncPatternFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13, T14, TResult>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7,
                arg8, arg9, arg10, arg11, arg12, arg13, arg14);

    /// <summary>
    /// Describes <c>Seq.FromAsyncPattern(begin, end)</c> over a Begin/End pair of 14 arguments
    /// whose End returns nothing.
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
    /// <param name="begin">The delegate that begins the asynchronous operation.</param>
    /// <param name="end">The delegate that ends the asynchronous operation.</param>
    /// <returns>
    /// A function that, when called, builds a <see cref="FromAsyncPatternAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> holding its arguments,
    /// which each target materializes through
    /// <see cref="ISeqVisitor.FromAsyncPatternAction14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(FromAsyncPatternAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>.
    /// </returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            Seq<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback,
            object?, IAsyncResult> begin, Action<IAsyncResult> end) =>
        (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13,
                arg14) => new FromAsyncPatternAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
                T11, T12, T13, T14>(begin, end, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8,
                arg9, arg10, arg11, arg12, arg13, arg14);
}
