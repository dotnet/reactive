// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SeqPlan1{T1, TResult}"/> as the target's own plan over one
    /// sequence, <c>source.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="ThenExtensions.Then{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then1<T1, TResult>(SeqPlan1<T1, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan2{T1, T2, TResult}"/> as the target's own plan over two
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern2{T1, T2}.Then{TResult}(Func{T1, T2, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then2<T1, T2, TResult>(SeqPlan2<T1, T2, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan3{T1, T2, T3, TResult}"/> as the target's own plan over three
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern3{T1, T2, T3}.Then{TResult}(Func{T1, T2, T3, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then3<T1, T2, T3, TResult>(SeqPlan3<T1, T2, T3, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan4{T1, T2, T3, T4, TResult}"/> as the target's own plan over four
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern4{T1, T2, T3, T4}.Then{TResult}(Func{T1, T2, T3, T4, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then4<T1, T2, T3, T4, TResult>(SeqPlan4<T1, T2, T3, T4,
            TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan5{T1, T2, T3, T4, T5, TResult}"/> as the target's own plan over five
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern5{T1, T2, T3, T4, T5}.Then{TResult}(Func{T1, T2, T3, T4, T5, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then5<T1, T2, T3, T4, T5, TResult>(SeqPlan5<T1, T2, T3, T4, T5,
            TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan6{T1, T2, T3, T4, T5, T6, TResult}"/> as the target's own plan over six
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern6{T1, T2, T3, T4, T5, T6}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then6<T1, T2, T3, T4, T5, T6, TResult>(SeqPlan6<T1, T2, T3, T4, T5,
            T6, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan7{T1, T2, T3, T4, T5, T6, T7, TResult}"/> as the target's own plan over seven
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern7{T1, T2, T3, T4, T5, T6, T7}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then7<T1, T2, T3, T4, T5, T6, T7, TResult>(SeqPlan7<T1, T2, T3, T4,
            T5, T6, T7, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/> as the target's own plan over eight
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern8{T1, T2, T3, T4, T5, T6, T7, T8}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(SeqPlan8<T1, T2, T3,
            T4, T5, T6, T7, T8, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan9{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}"/> as the target's own plan over nine
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern9{T1, T2, T3, T4, T5, T6, T7, T8, T9}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then9<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(SeqPlan9<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}"/> as the target's own plan over ten
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(SeqPlan10<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/> as the target's own plan over eleven
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            SeqPlan11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}"/> as the target's own plan over twelve
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            SeqPlan12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}"/> as the target's own plan over thirteen
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult>(SeqPlan13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/> as the target's own plan over fourteen
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            TResult>(SeqPlan14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}"/> as the target's own plan over fifteen
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15, TResult>(SeqPlan15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15, TResult> plan);

    /// <summary>
    /// Materializes a <see cref="SeqPlan16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}"/> as the target's own plan over sixteen
    /// sequences, <c>pattern.Then(selector)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}, string)"/>.</remarks>
    Realized<SeqPlan<TResult>> Then16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,
            T15, T16, TResult>(SeqPlan16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14, T15, T16, TResult> plan);
}
