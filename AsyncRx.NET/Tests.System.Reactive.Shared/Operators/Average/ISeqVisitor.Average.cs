// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="AverageInt32Seq"/> as the target's own <c>Average()</c> over <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{int})"/>.</remarks>
    Realized<Seq<double>> AverageInt32(AverageInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="AverageInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, int}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> AverageInt32Selector<TSource>(
        AverageInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageInt64Seq"/> as the target's own <c>Average()</c> over <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{long})"/>.</remarks>
    Realized<Seq<double>> AverageInt64(AverageInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="AverageInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, long}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> AverageInt64Selector<TSource>(
        AverageInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageSingleSeq"/> as the target's own <c>Average()</c> over <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{float})"/>.</remarks>
    Realized<Seq<float>> AverageSingle(AverageSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, float}, string)"/>.
    /// </remarks>
    Realized<Seq<float>> AverageSingleSelector<TSource>(
        AverageSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageDoubleSeq"/> as the target's own <c>Average()</c> over <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{double})"/>.</remarks>
    Realized<Seq<double>> AverageDouble(AverageDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, double}, string)"/>.
    /// </remarks>
    Realized<Seq<double>> AverageDoubleSelector<TSource>(
        AverageDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageDecimalSeq"/> as the target's own <c>Average()</c> over <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{decimal})"/>.</remarks>
    Realized<Seq<decimal>> AverageDecimal(AverageDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, decimal}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal>> AverageDecimalSelector<TSource>(
        AverageDecimalSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableInt32Seq"/> as the target's own <c>Average()</c> over nullable <c>int</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{int?})"/>.</remarks>
    Realized<Seq<double?>> AverageNullableInt32(AverageNullableInt32Seq seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableInt32SelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning nullable <c>int</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, int?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> AverageNullableInt32Selector<TSource>(
        AverageNullableInt32SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableInt64Seq"/> as the target's own <c>Average()</c> over nullable <c>long</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{long?})"/>.</remarks>
    Realized<Seq<double?>> AverageNullableInt64(AverageNullableInt64Seq seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableInt64SelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning nullable <c>long</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, long?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> AverageNullableInt64Selector<TSource>(
        AverageNullableInt64SelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableSingleSeq"/> as the target's own <c>Average()</c> over nullable <c>float</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{float?})"/>.</remarks>
    Realized<Seq<float?>> AverageNullableSingle(AverageNullableSingleSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableSingleSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning nullable <c>float</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, float?}, string)"/>.
    /// </remarks>
    Realized<Seq<float?>> AverageNullableSingleSelector<TSource>(
        AverageNullableSingleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableDoubleSeq"/> as the target's own <c>Average()</c> over nullable <c>double</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{double?})"/>.</remarks>
    Realized<Seq<double?>> AverageNullableDouble(AverageNullableDoubleSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableDoubleSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning nullable <c>double</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, double?}, string)"/>.
    /// </remarks>
    Realized<Seq<double?>> AverageNullableDoubleSelector<TSource>(
        AverageNullableDoubleSelectorSeq<TSource> seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableDecimalSeq"/> as the target's own <c>Average()</c> over nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>Built by <see cref="Seq.Average(Seq{decimal?})"/>.</remarks>
    Realized<Seq<decimal?>> AverageNullableDecimal(AverageNullableDecimalSeq seq);

    /// <summary>
    /// Materializes a <see cref="AverageNullableDecimalSelectorSeq{TSource}"/> as the target's own
    /// <c>Average(selector)</c> with a selector returning nullable <c>decimal</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Average{TSource}(Seq{TSource}, Func{TSource, decimal?}, string)"/>.
    /// </remarks>
    Realized<Seq<decimal?>> AverageNullableDecimalSelector<TSource>(
        AverageNullableDecimalSelectorSeq<TSource> seq);
}
