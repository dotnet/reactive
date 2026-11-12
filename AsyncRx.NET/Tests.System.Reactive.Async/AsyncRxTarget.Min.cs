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
    Realized<Seq<int>> ISeqVisitor.MinInt32(MinInt32Seq seq) =>
        Realized.Of<Seq<int>>(Materialize(seq.Source).Min());

    Realized<Seq<int>> ISeqVisitor.MinInt32Selector<TSource>(
        MinInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int>>(
            MinInt32SelectorImpl<TSource>, MinInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int> MinInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Min(selector);

    private static IAsyncObservable<int> MinInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int>> selector) =>
        source.Min(selector);

    Realized<Seq<long>> ISeqVisitor.MinInt64(MinInt64Seq seq) =>
        Realized.Of<Seq<long>>(Materialize(seq.Source).Min());

    Realized<Seq<long>> ISeqVisitor.MinInt64Selector<TSource>(
        MinInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long>>(
            MinInt64SelectorImpl<TSource>, MinInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long> MinInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Min(selector);

    private static IAsyncObservable<long> MinInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long>> selector) =>
        source.Min(selector);

    Realized<Seq<float>> ISeqVisitor.MinSingle(MinSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Min());

    Realized<Seq<float>> ISeqVisitor.MinSingleSelector<TSource>(
        MinSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float>>(
            MinSingleSelectorImpl<TSource>, MinSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> MinSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Min(selector);

    private static IAsyncObservable<float> MinSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float>> selector) =>
        source.Min(selector);

    Realized<Seq<double>> ISeqVisitor.MinDouble(MinDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Min());

    Realized<Seq<double>> ISeqVisitor.MinDoubleSelector<TSource>(
        MinDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            MinDoubleSelectorImpl<TSource>, MinDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> MinDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Min(selector);

    private static IAsyncObservable<double> MinDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double>> selector) =>
        source.Min(selector);

    Realized<Seq<decimal>> ISeqVisitor.MinDecimal(MinDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Min());

    Realized<Seq<decimal>> ISeqVisitor.MinDecimalSelector<TSource>(
        MinDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal>>(
            MinDecimalSelectorImpl<TSource>, MinDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> MinDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Min(selector);

    private static IAsyncObservable<decimal> MinDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal>> selector) =>
        source.Min(selector);

    Realized<Seq<int?>> ISeqVisitor.MinNullableInt32(MinNullableInt32Seq seq) =>
        Realized.Of<Seq<int?>>(Materialize(seq.Source).Min());

    Realized<Seq<int?>> ISeqVisitor.MinNullableInt32Selector<TSource>(
        MinNullableInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int?>>(
            MinNullableInt32SelectorImpl<TSource>, MinNullableInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int?> MinNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Min(selector);

    private static IAsyncObservable<int?> MinNullableInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int?>> selector) =>
        source.Min(selector);

    Realized<Seq<long?>> ISeqVisitor.MinNullableInt64(MinNullableInt64Seq seq) =>
        Realized.Of<Seq<long?>>(Materialize(seq.Source).Min());

    Realized<Seq<long?>> ISeqVisitor.MinNullableInt64Selector<TSource>(
        MinNullableInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long?>>(
            MinNullableInt64SelectorImpl<TSource>, MinNullableInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long?> MinNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Min(selector);

    private static IAsyncObservable<long?> MinNullableInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long?>> selector) =>
        source.Min(selector);

    Realized<Seq<float?>> ISeqVisitor.MinNullableSingle(MinNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Min());

    Realized<Seq<float?>> ISeqVisitor.MinNullableSingleSelector<TSource>(
        MinNullableSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float?>>(
            MinNullableSingleSelectorImpl<TSource>, MinNullableSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float?> MinNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Min(selector);

    private static IAsyncObservable<float?> MinNullableSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float?>> selector) =>
        source.Min(selector);

    Realized<Seq<double?>> ISeqVisitor.MinNullableDouble(MinNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Min());

    Realized<Seq<double?>> ISeqVisitor.MinNullableDoubleSelector<TSource>(
        MinNullableDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            MinNullableDoubleSelectorImpl<TSource>, MinNullableDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> MinNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Min(selector);

    private static IAsyncObservable<double?> MinNullableDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double?>> selector) =>
        source.Min(selector);

    Realized<Seq<decimal?>> ISeqVisitor.MinNullableDecimal(MinNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Min());

    Realized<Seq<decimal?>> ISeqVisitor.MinNullableDecimalSelector<TSource>(
        MinNullableDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal?>>(
            MinNullableDecimalSelectorImpl<TSource>, MinNullableDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal?> MinNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Min(selector);

    private static IAsyncObservable<decimal?> MinNullableDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal?>> selector) =>
        source.Min(selector);

    Realized<Seq<T>> ISeqVisitor.Min<T>(MinSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MinImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.MinComparer<T>(MinComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MinComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<TResult>> ISeqVisitor.MinSelector<TSource, TResult>(
        MinSelectorSeq<TSource, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            MinSelectorImpl<TSource, TResult>, MinSelectorAsyncImpl<TSource, TResult>, seq.Source,
            seq.Selector);

    Realized<Seq<TResult>> ISeqVisitor.MinSelectorComparer<TSource, TResult>(
        MinSelectorComparerSeq<TSource, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            MinSelectorComparerImpl<TSource, TResult>,
            MinSelectorComparerAsyncImpl<TSource, TResult>, seq.Source, seq.Selector, seq.Comparer);

    private static IAsyncObservable<T> MinImpl<T>(IAsyncObservable<T> source) => source.Min();

    private static IAsyncObservable<T> MinComparerImpl<T>(
        IAsyncObservable<T> source,
        IComparer<T> comparer) =>
        source.Min(comparer);

    private static IAsyncObservable<TResult> MinSelectorImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector) =>
        source.Min(selector);

    private static IAsyncObservable<TResult> MinSelectorAsyncImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TResult>> selector) =>
        source.Min(selector);

    private static IAsyncObservable<TResult> MinSelectorComparerImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector,
        IComparer<TResult> comparer) =>
        source.Min(selector, comparer);

    private static IAsyncObservable<TResult> MinSelectorComparerAsyncImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TResult>> selector,
        IComparer<TResult> comparer) =>
        source.Min(selector, comparer);
}
