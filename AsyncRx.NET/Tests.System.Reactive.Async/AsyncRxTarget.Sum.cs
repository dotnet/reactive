// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    // The typed source forms materialize their source directly: a closed element type cannot be
    // a nested description, so there is nothing for the bridge to instantiate.
    Realized<Seq<int>> ISeqVisitor.SumInt32(SumInt32Seq seq) =>
        Realized.Of<Seq<int>>(Materialize(seq.Source).Sum());

    Realized<Seq<int>> ISeqVisitor.SumInt32Selector<TSource>(
        SumInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int>>(
            SumInt32SelectorImpl<TSource>, SumInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int> SumInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<int> SumInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int>> selector) =>
        source.Sum(selector);

    Realized<Seq<long>> ISeqVisitor.SumInt64(SumInt64Seq seq) =>
        Realized.Of<Seq<long>>(Materialize(seq.Source).Sum());

    Realized<Seq<long>> ISeqVisitor.SumInt64Selector<TSource>(
        SumInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long>>(
            SumInt64SelectorImpl<TSource>, SumInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long> SumInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<long> SumInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long>> selector) =>
        source.Sum(selector);

    Realized<Seq<float>> ISeqVisitor.SumSingle(SumSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Sum());

    Realized<Seq<float>> ISeqVisitor.SumSingleSelector<TSource>(
        SumSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float>>(
            SumSingleSelectorImpl<TSource>, SumSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> SumSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<float> SumSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float>> selector) =>
        source.Sum(selector);

    Realized<Seq<double>> ISeqVisitor.SumDouble(SumDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Sum());

    Realized<Seq<double>> ISeqVisitor.SumDoubleSelector<TSource>(
        SumDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            SumDoubleSelectorImpl<TSource>, SumDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> SumDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<double> SumDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double>> selector) =>
        source.Sum(selector);

    Realized<Seq<decimal>> ISeqVisitor.SumDecimal(SumDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Sum());

    Realized<Seq<decimal>> ISeqVisitor.SumDecimalSelector<TSource>(
        SumDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal>>(
            SumDecimalSelectorImpl<TSource>, SumDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> SumDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<decimal> SumDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal>> selector) =>
        source.Sum(selector);

    Realized<Seq<int?>> ISeqVisitor.SumNullableInt32(SumNullableInt32Seq seq) =>
        Realized.Of<Seq<int?>>(Materialize(seq.Source).Sum());

    Realized<Seq<int?>> ISeqVisitor.SumNullableInt32Selector<TSource>(
        SumNullableInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int?>>(
            SumNullableInt32SelectorImpl<TSource>, SumNullableInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int?> SumNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<int?> SumNullableInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int?>> selector) =>
        source.Sum(selector);

    Realized<Seq<long?>> ISeqVisitor.SumNullableInt64(SumNullableInt64Seq seq) =>
        Realized.Of<Seq<long?>>(Materialize(seq.Source).Sum());

    Realized<Seq<long?>> ISeqVisitor.SumNullableInt64Selector<TSource>(
        SumNullableInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long?>>(
            SumNullableInt64SelectorImpl<TSource>, SumNullableInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long?> SumNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<long?> SumNullableInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long?>> selector) =>
        source.Sum(selector);

    Realized<Seq<float?>> ISeqVisitor.SumNullableSingle(SumNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Sum());

    Realized<Seq<float?>> ISeqVisitor.SumNullableSingleSelector<TSource>(
        SumNullableSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float?>>(
            SumNullableSingleSelectorImpl<TSource>, SumNullableSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float?> SumNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<float?> SumNullableSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float?>> selector) =>
        source.Sum(selector);

    Realized<Seq<double?>> ISeqVisitor.SumNullableDouble(SumNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Sum());

    Realized<Seq<double?>> ISeqVisitor.SumNullableDoubleSelector<TSource>(
        SumNullableDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            SumNullableDoubleSelectorImpl<TSource>, SumNullableDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> SumNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<double?> SumNullableDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double?>> selector) =>
        source.Sum(selector);

    Realized<Seq<decimal?>> ISeqVisitor.SumNullableDecimal(SumNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Sum());

    Realized<Seq<decimal?>> ISeqVisitor.SumNullableDecimalSelector<TSource>(
        SumNullableDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal?>>(
            SumNullableDecimalSelectorImpl<TSource>, SumNullableDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal?> SumNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Sum(selector);

    private static IAsyncObservable<decimal?> SumNullableDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal?>> selector) =>
        source.Sum(selector);
}
