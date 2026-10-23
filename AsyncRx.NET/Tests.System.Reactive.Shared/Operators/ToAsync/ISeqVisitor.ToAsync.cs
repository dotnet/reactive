// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc0Seq{TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 0 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc0<TResult>(ToAsyncFunc0Seq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction0Seq"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 0 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction0(ToAsyncAction0Seq seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc1Seq{T1, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 1 argument.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc1<T1, TResult>(ToAsyncFunc1Seq<T1, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction1Seq{T1}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 1 argument.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction1<T1>(ToAsyncAction1Seq<T1> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc2Seq{T1, T2, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 2 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc2<T1, T2, TResult>(ToAsyncFunc2Seq<T1, T2, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction2Seq{T1, T2}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 2 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction2<T1, T2>(ToAsyncAction2Seq<T1, T2> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc3Seq{T1, T2, T3, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 3 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc3<T1, T2, T3, TResult>(ToAsyncFunc3Seq<T1, T2, T3,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction3Seq{T1, T2, T3}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 3 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction3<T1, T2, T3>(ToAsyncAction3Seq<T1, T2, T3> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc4Seq{T1, T2, T3, T4, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 4 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc4<T1, T2, T3, T4, TResult>(ToAsyncFunc4Seq<T1, T2, T3, T4,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction4Seq{T1, T2, T3, T4}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 4 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction4<T1, T2, T3, T4>(ToAsyncAction4Seq<T1, T2, T3, T4> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc5Seq{T1, T2, T3, T4, T5, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 5 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc5<T1, T2, T3, T4, T5, TResult>(ToAsyncFunc5Seq<T1, T2, T3, T4,
            T5, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction5Seq{T1, T2, T3, T4, T5}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 5 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction5<T1, T2, T3, T4, T5>(ToAsyncAction5Seq<T1, T2, T3, T4,
            T5> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc6Seq{T1, T2, T3, T4, T5, T6, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 6 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc6<T1, T2, T3, T4, T5, T6, TResult>(ToAsyncFunc6Seq<T1, T2, T3,
            T4, T5, T6, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction6Seq{T1, T2, T3, T4, T5, T6}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 6 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction6<T1, T2, T3, T4, T5, T6>(ToAsyncAction6Seq<T1, T2, T3, T4, T5,
            T6> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 7 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc7<T1, T2, T3, T4, T5, T6, T7, TResult>(ToAsyncFunc7Seq<T1, T2,
            T3, T4, T5, T6, T7, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction7Seq{T1, T2, T3, T4, T5, T6, T7}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 7 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction7<T1, T2, T3, T4, T5, T6, T7>(ToAsyncAction7Seq<T1, T2, T3, T4,
            T5, T6, T7> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 8 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(ToAsyncFunc8Seq<T1,
            T2, T3, T4, T5, T6, T7, T8, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 8 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction8<T1, T2, T3, T4, T5, T6, T7, T8>(ToAsyncAction8Seq<T1, T2, T3,
            T4, T5, T6, T7, T8> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 9 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            ToAsyncFunc9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 9 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ToAsyncAction9Seq<T1, T2,
            T3, T4, T5, T6, T7, T8, T9> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 10
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            ToAsyncFunc10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 10 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ToAsyncAction10Seq<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 11
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            ToAsyncFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 11 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            ToAsyncAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 12
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(ToAsyncFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 12 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            ToAsyncAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 13
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult>(ToAsyncFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 13 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            ToAsyncAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 14
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, TResult>(ToAsyncFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 14 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14>(ToAsyncAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 15
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15, TResult>(ToAsyncFunc15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, T15, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction15Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 15 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15>(ToAsyncAction15Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncFunc16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/>: calls the target's own <c>ToAsync(function[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over a function of 16
    /// arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> ToAsyncFunc16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15, T16, TResult>(ToAsyncFunc16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, T14, T15, T16, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="ToAsyncAction16Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/>: calls the target's own <c>ToAsync(action[, scheduler])</c>
    /// and then the function it returns, with the node's arguments, over an action of 16 arguments.
    /// </summary>
    /// <remarks>Built by the <c>Seq.ToAsync</c> overloads in <c>Seq.ToAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> ToAsyncAction16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15, T16>(ToAsyncAction16Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15, T16> seq);
}
