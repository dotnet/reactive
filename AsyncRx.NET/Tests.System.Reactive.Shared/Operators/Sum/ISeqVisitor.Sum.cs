// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SumInt32Seq"/> as the target's own <c>Sum()</c> over <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{int})"/>.</remarks>
    Realized<Seq<int>> SumInt32(SumInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="SumInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, int}, string)"/>.
    /// </remarks>
    Realized<Seq<int>> SumInt32Selector<TSource>(
        SumInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumInt64Seq"/> as the target's own <c>Sum()</c> over <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{long})"/>.</remarks>
    Realized<Seq<long>> SumInt64(SumInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="SumInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, long}, string)"/>.
    /// </remarks>
    Realized<Seq<long>> SumInt64Selector<TSource>(
        SumInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumSingleSeq"/> as the target's own <c>Sum()</c> over <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{float})"/>.</remarks>
    Realized<Seq<float>> SumSingle(SumSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>.
    /// </remarks>
    Realized<Seq<float>> SumSingleSelector<TSource>(
        SumSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumDoubleSeq"/> as the target's own <c>Sum()</c> over <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{double})"/>.</remarks>
    Realized<Seq<double>> SumDouble(SumDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, double}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> SumDoubleSelector<TSource>(
        SumDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumDecimalSeq"/> as the target's own <c>Sum()</c> over <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{decimal})"/>.</remarks>
    Realized<Seq<decimal>> SumDecimal(SumDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, decimal}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal>> SumDecimalSelector<TSource>(
        SumDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableInt32Seq"/> as the target's own <c>Sum()</c> over nullable <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{int?})"/>.</remarks>
    Realized<Seq<int?>> SumNullableInt32(SumNullableInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, int?}, string)"/>.
    /// </remarks>
    Realized<Seq<int?>> SumNullableInt32Selector<TSource>(
        SumNullableInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableInt64Seq"/> as the target's own <c>Sum()</c> over nullable <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{long?})"/>.</remarks>
    Realized<Seq<long?>> SumNullableInt64(SumNullableInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, long?}, string)"/>.
    /// </remarks>
    Realized<Seq<long?>> SumNullableInt64Selector<TSource>(
        SumNullableInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableSingleSeq"/> as the target's own <c>Sum()</c> over nullable <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{float?})"/>.</remarks>
    Realized<Seq<float?>> SumNullableSingle(SumNullableSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, float?}, string)"/>.
    /// </remarks>
    Realized<Seq<float?>> SumNullableSingleSelector<TSource>(
        SumNullableSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableDoubleSeq"/> as the target's own <c>Sum()</c> over nullable <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{double?})"/>.</remarks>
    Realized<Seq<double?>> SumNullableDouble(SumNullableDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, double?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> SumNullableDoubleSelector<TSource>(
        SumNullableDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableDecimalSeq"/> as the target's own <c>Sum()</c> over nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Sum(Seq{decimal?})"/>.</remarks>
    Realized<Seq<decimal?>> SumNullableDecimal(SumNullableDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="SumNullableDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Sum(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Sum{TSource}(Seq{TSource}, Func{TSource, decimal?}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal?>> SumNullableDecimalSelector<TSource>(
        SumNullableDecimalSelectorSeq<TSource> seq);
}
