// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc0Seq{TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 0 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc0<TResult>(FromAsyncPatternFunc0Seq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction0Seq"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 0 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction0(FromAsyncPatternAction0Seq seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc1Seq{T1, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 1 argument.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc1<T1, TResult>(FromAsyncPatternFunc1Seq<T1,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction1Seq{T1}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 1 argument whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction1<T1>(FromAsyncPatternAction1Seq<T1> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc2Seq{T1, T2, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 2 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc2<T1, T2, TResult>(FromAsyncPatternFunc2Seq<T1, T2,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction2Seq{T1, T2}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 2 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction2<T1, T2>(FromAsyncPatternAction2Seq<T1, T2> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc3Seq{T1, T2, T3, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 3 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc3<T1, T2, T3, TResult>(FromAsyncPatternFunc3Seq<T1,
            T2, T3, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction3Seq{T1, T2, T3}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 3 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction3<T1, T2, T3>(FromAsyncPatternAction3Seq<T1, T2,
            T3> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc4Seq{T1, T2, T3, T4, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 4 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc4<T1, T2, T3, T4, TResult>(FromAsyncPatternFunc4Seq<
            T1, T2, T3, T4, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction4Seq{T1, T2, T3, T4}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 4 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction4<T1, T2, T3, T4>(FromAsyncPatternAction4Seq<T1, T2,
            T3, T4> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc5Seq{T1, T2, T3, T4, T5, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 5 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc5<T1, T2, T3, T4, T5, TResult>(
            FromAsyncPatternFunc5Seq<T1, T2, T3, T4, T5, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction5Seq{T1, T2, T3, T4, T5}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 5 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction5<T1, T2, T3, T4, T5>(FromAsyncPatternAction5Seq<T1,
            T2, T3, T4, T5> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc6Seq{T1, T2, T3, T4, T5, T6, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 6 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc6<T1, T2, T3, T4, T5, T6, TResult>(
            FromAsyncPatternFunc6Seq<T1, T2, T3, T4, T5, T6, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction6Seq{T1, T2, T3, T4, T5, T6}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 6 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction6<T1, T2, T3, T4, T5, T6>(FromAsyncPatternAction6Seq<
            T1, T2, T3, T4, T5, T6> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc7Seq{T1, T2, T3, T4, T5, T6, T7, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 7 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc7<T1, T2, T3, T4, T5, T6, T7, TResult>(
            FromAsyncPatternFunc7Seq<T1, T2, T3, T4, T5, T6, T7, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction7Seq{T1, T2, T3, T4, T5, T6, T7}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 7 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction7<T1, T2, T3, T4, T5, T6, T7>(
            FromAsyncPatternAction7Seq<T1, T2, T3, T4, T5, T6, T7> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc8Seq{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 8 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            FromAsyncPatternFunc8Seq<T1, T2, T3, T4, T5, T6, T7, T8, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction8Seq{T1, T2, T3, T4, T5, T6, T7, T8}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 8 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction8<T1, T2, T3, T4, T5, T6, T7, T8>(
            FromAsyncPatternAction8Seq<T1, T2, T3, T4, T5, T6, T7, T8> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 9 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            FromAsyncPatternFunc9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction9Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 9 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            FromAsyncPatternAction9Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 10 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            FromAsyncPatternFunc10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction10Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 10 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            FromAsyncPatternAction10Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 11 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult>(FromAsyncPatternFunc11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 11 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            FromAsyncPatternAction11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 12 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult>(FromAsyncPatternFunc12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction12Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 12 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            FromAsyncPatternAction12Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 13 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, TResult>(FromAsyncPatternFunc13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,
            T12, T13, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction13Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 13 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13>(FromAsyncPatternAction13Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternFunc14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 14 arguments.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<TResult>> FromAsyncPatternFunc14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14, TResult>(FromAsyncPatternFunc14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12, T13, T14, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncPatternAction14Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/>: calls the target's own
    /// <c>FromAsyncPattern(begin, end)</c> and then the function it returns, with the node's
    /// arguments, over a Begin/End pair of 14 arguments whose End returns nothing.
    /// </summary>
    /// <remarks>
    /// Built by the <c>Seq.FromAsyncPattern</c> overloads in <c>Seq.FromAsyncPattern.cs</c>.
    /// </remarks>
    Realized<Seq<Unit>> FromAsyncPatternAction14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14>(FromAsyncPatternAction14Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,
            T13, T14> seq);
}
