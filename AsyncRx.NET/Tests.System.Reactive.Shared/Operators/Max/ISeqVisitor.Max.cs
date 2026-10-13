// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MaxInt32Seq"/> as the target's own <c>Max()</c> over <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{int})"/>.</remarks>
    Realized<Seq<int>> MaxInt32(MaxInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="MaxInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, int}, string)"/>.
    /// </remarks>
    Realized<Seq<int>> MaxInt32Selector<TSource>(
        MaxInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxInt64Seq"/> as the target's own <c>Max()</c> over <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{long})"/>.</remarks>
    Realized<Seq<long>> MaxInt64(MaxInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="MaxInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, long}, string)"/>.
    /// </remarks>
    Realized<Seq<long>> MaxInt64Selector<TSource>(
        MaxInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxSingleSeq"/> as the target's own <c>Max()</c> over <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{float})"/>.</remarks>
    Realized<Seq<float>> MaxSingle(MaxSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>.
    /// </remarks>
    Realized<Seq<float>> MaxSingleSelector<TSource>(
        MaxSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxDoubleSeq"/> as the target's own <c>Max()</c> over <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{double})"/>.</remarks>
    Realized<Seq<double>> MaxDouble(MaxDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, double}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> MaxDoubleSelector<TSource>(
        MaxDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxDecimalSeq"/> as the target's own <c>Max()</c> over <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{decimal})"/>.</remarks>
    Realized<Seq<decimal>> MaxDecimal(MaxDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, decimal}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal>> MaxDecimalSelector<TSource>(
        MaxDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableInt32Seq"/> as the target's own <c>Max()</c> over nullable <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{int?})"/>.</remarks>
    Realized<Seq<int?>> MaxNullableInt32(MaxNullableInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, int?}, string)"/>.
    /// </remarks>
    Realized<Seq<int?>> MaxNullableInt32Selector<TSource>(
        MaxNullableInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableInt64Seq"/> as the target's own <c>Max()</c> over nullable <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{long?})"/>.</remarks>
    Realized<Seq<long?>> MaxNullableInt64(MaxNullableInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, long?}, string)"/>.
    /// </remarks>
    Realized<Seq<long?>> MaxNullableInt64Selector<TSource>(
        MaxNullableInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableSingleSeq"/> as the target's own <c>Max()</c> over nullable <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{float?})"/>.</remarks>
    Realized<Seq<float?>> MaxNullableSingle(MaxNullableSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, float?}, string)"/>.
    /// </remarks>
    Realized<Seq<float?>> MaxNullableSingleSelector<TSource>(
        MaxNullableSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableDoubleSeq"/> as the target's own <c>Max()</c> over nullable <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{double?})"/>.</remarks>
    Realized<Seq<double?>> MaxNullableDouble(MaxNullableDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, double?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> MaxNullableDoubleSelector<TSource>(
        MaxNullableDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableDecimalSeq"/> as the target's own <c>Max()</c> over nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max(Seq{decimal?})"/>.</remarks>
    Realized<Seq<decimal?>> MaxNullableDecimal(MaxNullableDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="MaxNullableDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Max(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Max{TSource}(Seq{TSource}, Func{TSource, decimal?}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal?>> MaxNullableDecimalSelector<TSource>(
        MaxNullableDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MaxSeq{T}"/> as the target's own <c>Max()</c> over elements of
    /// any type.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Max<T>(MaxSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MaxComparerSeq{T}"/> as the target's own <c>Max(comparer)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Max{T}(Seq{T}, IComparer{T})"/>.</remarks>
    Realized<Seq<T>> MaxComparer<T>(MaxComparerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MaxSelectorSeq{TSource, TResult}"/> as the target's own
    /// <c>Max(selector)</c> with a selector of any result type.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Max{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> MaxSelector<TSource, TResult>(MaxSelectorSeq<TSource, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="MaxSelectorComparerSeq{TSource, TResult}"/> as the target's own
    /// <c>Max(selector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Max{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, IComparer{TResult}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> MaxSelectorComparer<TSource, TResult>(
        MaxSelectorComparerSeq<TSource, TResult> seq);
}
