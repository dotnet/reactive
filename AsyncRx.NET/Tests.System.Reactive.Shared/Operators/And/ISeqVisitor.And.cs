// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SeqPattern2{T1, T2}"/> as the target's own pattern over two
    /// sequences, <c>left.And(right)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="AndExtensions.And{T1, T2}(Seq{T1}, Seq{T2})"/>.</remarks>
    Realized<SeqPattern2<T1, T2>> And2<T1, T2>(SeqPattern2<T1, T2> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern3{T1, T2, T3}"/> as the target's own pattern over three
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern2{T1, T2}.And{T3}(Seq{T3})"/>.</remarks>
    Realized<SeqPattern3<T1, T2, T3>> And3<T1, T2, T3>(SeqPattern3<T1, T2, T3> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern4{T1, T2, T3, T4}"/> as the target's own pattern over four
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern3{T1, T2, T3}.And{T4}(Seq{T4})"/>.</remarks>
    Realized<SeqPattern4<T1, T2, T3, T4>> And4<T1, T2, T3, T4>(SeqPattern4<T1, T2, T3, T4> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern5{T1, T2, T3, T4, T5}"/> as the target's own pattern over five
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern4{T1, T2, T3, T4}.And{T5}(Seq{T5})"/>.</remarks>
    Realized<SeqPattern5<T1, T2, T3, T4, T5>> And5<T1, T2, T3, T4, T5>(SeqPattern5<T1, T2, T3, T4,
            T5> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern6{T1, T2, T3, T4, T5, T6}"/> as the target's own pattern over six
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern5{T1, T2, T3, T4, T5}.And{T6}(Seq{T6})"/>.</remarks>
    Realized<SeqPattern6<T1, T2, T3, T4, T5, T6>> And6<T1, T2, T3, T4, T5, T6>(SeqPattern6<T1, T2,
            T3, T4, T5, T6> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern7{T1, T2, T3, T4, T5, T6, T7}"/> as the target's own pattern over seven
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern6{T1, T2, T3, T4, T5, T6}.And{T7}(Seq{T7})"/>.</remarks>
    Realized<SeqPattern7<T1, T2, T3, T4, T5, T6, T7>> And7<T1, T2, T3, T4, T5, T6, T7>(SeqPattern7<
            T1, T2, T3, T4, T5, T6, T7> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern8{T1, T2, T3, T4, T5, T6, T7, T8}"/> as the target's own pattern over eight
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern7{T1, T2, T3, T4, T5, T6, T7}.And{T8}(Seq{T8})"/>.</remarks>
    Realized<SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>> And8<T1, T2, T3, T4, T5, T6, T7, T8>(
            SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern9{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> as the target's own pattern over nine
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern8{T1, T2, T3, T4, T5, T6, T7, T8}.And{T9}(Seq{T9})"/>.</remarks>
    Realized<SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9>> And9<T1, T2, T3, T4, T5, T6, T7, T8,
            T9>(SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> as the target's own pattern over ten
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern9{T1, T2, T3, T4, T5, T6, T7, T8, T9}.And{T10}(Seq{T10})"/>.</remarks>
    Realized<SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> And10<T1, T2, T3, T4, T5, T6,
            T7, T8, T9, T10>(SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> as the target's own pattern over eleven
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}.And{T11}(Seq{T11})"/>.</remarks>
    Realized<SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> And11<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, T11>(SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> as the target's own pattern over twelve
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}.And{T12}(Seq{T12})"/>.</remarks>
    Realized<SeqPattern12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> And12<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12>(SeqPattern12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11, T12> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> as the target's own pattern over thirteen
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}.And{T13}(Seq{T13})"/>.</remarks>
    Realized<SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> And13<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11, T12, T13> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> as the target's own pattern over fourteen
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}.And{T14}(Seq{T14})"/>.</remarks>
    Realized<SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> And14<T1,
            T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(SeqPattern14<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> as the target's own pattern over fifteen
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}.And{T15}(Seq{T15})"/>.</remarks>
    Realized<SeqPattern15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> And15<
            T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(SeqPattern15<T1, T2,
            T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> pattern);

    /// <summary>
    /// Materializes a <see cref="SeqPattern16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> as the target's own pattern over sixteen
    /// sequences, <c>pattern.And(other)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="SeqPattern15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}.And{T16}(Seq{T16})"/>.</remarks>
    Realized<SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16>> And16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> pattern);
}
