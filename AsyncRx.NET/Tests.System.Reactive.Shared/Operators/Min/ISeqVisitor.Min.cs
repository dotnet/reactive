// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MinInt32Seq"/> as the target's own <c>Min()</c> over <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{int})"/>.</remarks>
    Realized<Seq<int>> MinInt32(MinInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="MinInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, int}, string)"/>.
    /// </remarks>
    Realized<Seq<int>> MinInt32Selector<TSource>(
        MinInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinInt64Seq"/> as the target's own <c>Min()</c> over <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{long})"/>.</remarks>
    Realized<Seq<long>> MinInt64(MinInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="MinInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, long}, string)"/>.
    /// </remarks>
    Realized<Seq<long>> MinInt64Selector<TSource>(
        MinInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinSingleSeq"/> as the target's own <c>Min()</c> over <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{float})"/>.</remarks>
    Realized<Seq<float>> MinSingle(MinSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>.
    /// </remarks>
    Realized<Seq<float>> MinSingleSelector<TSource>(
        MinSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinDoubleSeq"/> as the target's own <c>Min()</c> over <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{double})"/>.</remarks>
    Realized<Seq<double>> MinDouble(MinDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, double}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> MinDoubleSelector<TSource>(
        MinDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinDecimalSeq"/> as the target's own <c>Min()</c> over <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{decimal})"/>.</remarks>
    Realized<Seq<decimal>> MinDecimal(MinDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, decimal}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal>> MinDecimalSelector<TSource>(
        MinDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableInt32Seq"/> as the target's own <c>Min()</c> over nullable <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{int?})"/>.</remarks>
    Realized<Seq<int?>> MinNullableInt32(MinNullableInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, int?}, string)"/>.
    /// </remarks>
    Realized<Seq<int?>> MinNullableInt32Selector<TSource>(
        MinNullableInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableInt64Seq"/> as the target's own <c>Min()</c> over nullable <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{long?})"/>.</remarks>
    Realized<Seq<long?>> MinNullableInt64(MinNullableInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, long?}, string)"/>.
    /// </remarks>
    Realized<Seq<long?>> MinNullableInt64Selector<TSource>(
        MinNullableInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableSingleSeq"/> as the target's own <c>Min()</c> over nullable <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{float?})"/>.</remarks>
    Realized<Seq<float?>> MinNullableSingle(MinNullableSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, float?}, string)"/>.
    /// </remarks>
    Realized<Seq<float?>> MinNullableSingleSelector<TSource>(
        MinNullableSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableDoubleSeq"/> as the target's own <c>Min()</c> over nullable <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{double?})"/>.</remarks>
    Realized<Seq<double?>> MinNullableDouble(MinNullableDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, double?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> MinNullableDoubleSelector<TSource>(
        MinNullableDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableDecimalSeq"/> as the target's own <c>Min()</c> over nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min(Seq{decimal?})"/>.</remarks>
    Realized<Seq<decimal?>> MinNullableDecimal(MinNullableDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="MinNullableDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Min(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Min{TSource}(Seq{TSource}, Func{TSource, decimal?}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal?>> MinNullableDecimalSelector<TSource>(
        MinNullableDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="MinSeq{T}"/> as the target's own <c>Min()</c> over elements of
    /// any type.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Min<T>(MinSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MinComparerSeq{T}"/> as the target's own <c>Min(comparer)</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Min{T}(Seq{T}, IComparer{T})"/>.</remarks>
    Realized<Seq<T>> MinComparer<T>(MinComparerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="MinSelectorSeq{TSource, TResult}"/> as the target's own
    /// <c>Min(selector)</c> with a selector of any result type.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Min{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> MinSelector<TSource, TResult>(MinSelectorSeq<TSource, TResult> seq);

    /// <summary>
    /// Materializes a <see cref="MinSelectorComparerSeq{TSource, TResult}"/> as the target's own
    /// <c>Min(selector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Min{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, IComparer{TResult}, string)"/>.
    /// </remarks>
    Realized<Seq<TResult>> MinSelectorComparer<TSource, TResult>(
        MinSelectorComparerSeq<TSource, TResult> seq);
}
