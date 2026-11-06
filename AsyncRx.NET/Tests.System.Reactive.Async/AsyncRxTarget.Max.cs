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
    Realized<Seq<int>> ISeqVisitor.MaxInt32(MaxInt32Seq seq) =>
        Realized.Of<Seq<int>>(Materialize(seq.Source).Max());

    Realized<Seq<int>> ISeqVisitor.MaxInt32Selector<TSource>(
        MaxInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int>>(
            MaxInt32SelectorImpl<TSource>, MaxInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int> MaxInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Max(selector);

    private static IAsyncObservable<int> MaxInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int>> selector) =>
        source.Max(selector);

    Realized<Seq<long>> ISeqVisitor.MaxInt64(MaxInt64Seq seq) =>
        Realized.Of<Seq<long>>(Materialize(seq.Source).Max());

    Realized<Seq<long>> ISeqVisitor.MaxInt64Selector<TSource>(
        MaxInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long>>(
            MaxInt64SelectorImpl<TSource>, MaxInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long> MaxInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Max(selector);

    private static IAsyncObservable<long> MaxInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long>> selector) =>
        source.Max(selector);

    Realized<Seq<float>> ISeqVisitor.MaxSingle(MaxSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Max());

    Realized<Seq<float>> ISeqVisitor.MaxSingleSelector<TSource>(
        MaxSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float>>(
            MaxSingleSelectorImpl<TSource>, MaxSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> MaxSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Max(selector);

    private static IAsyncObservable<float> MaxSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float>> selector) =>
        source.Max(selector);

    Realized<Seq<double>> ISeqVisitor.MaxDouble(MaxDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Max());

    Realized<Seq<double>> ISeqVisitor.MaxDoubleSelector<TSource>(
        MaxDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            MaxDoubleSelectorImpl<TSource>, MaxDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> MaxDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Max(selector);

    private static IAsyncObservable<double> MaxDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double>> selector) =>
        source.Max(selector);

    Realized<Seq<decimal>> ISeqVisitor.MaxDecimal(MaxDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Max());

    Realized<Seq<decimal>> ISeqVisitor.MaxDecimalSelector<TSource>(
        MaxDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal>>(
            MaxDecimalSelectorImpl<TSource>, MaxDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> MaxDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Max(selector);

    private static IAsyncObservable<decimal> MaxDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal>> selector) =>
        source.Max(selector);

    Realized<Seq<int?>> ISeqVisitor.MaxNullableInt32(MaxNullableInt32Seq seq) =>
        Realized.Of<Seq<int?>>(Materialize(seq.Source).Max());

    Realized<Seq<int?>> ISeqVisitor.MaxNullableInt32Selector<TSource>(
        MaxNullableInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<int?>>(
            MaxNullableInt32SelectorImpl<TSource>, MaxNullableInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int?> MaxNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Max(selector);

    private static IAsyncObservable<int?> MaxNullableInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int?>> selector) =>
        source.Max(selector);

    Realized<Seq<long?>> ISeqVisitor.MaxNullableInt64(MaxNullableInt64Seq seq) =>
        Realized.Of<Seq<long?>>(Materialize(seq.Source).Max());

    Realized<Seq<long?>> ISeqVisitor.MaxNullableInt64Selector<TSource>(
        MaxNullableInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<long?>>(
            MaxNullableInt64SelectorImpl<TSource>, MaxNullableInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long?> MaxNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Max(selector);

    private static IAsyncObservable<long?> MaxNullableInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long?>> selector) =>
        source.Max(selector);

    Realized<Seq<float?>> ISeqVisitor.MaxNullableSingle(MaxNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Max());

    Realized<Seq<float?>> ISeqVisitor.MaxNullableSingleSelector<TSource>(
        MaxNullableSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float?>>(
            MaxNullableSingleSelectorImpl<TSource>, MaxNullableSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float?> MaxNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Max(selector);

    private static IAsyncObservable<float?> MaxNullableSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float?>> selector) =>
        source.Max(selector);

    Realized<Seq<double?>> ISeqVisitor.MaxNullableDouble(MaxNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Max());

    Realized<Seq<double?>> ISeqVisitor.MaxNullableDoubleSelector<TSource>(
        MaxNullableDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            MaxNullableDoubleSelectorImpl<TSource>, MaxNullableDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> MaxNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Max(selector);

    private static IAsyncObservable<double?> MaxNullableDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double?>> selector) =>
        source.Max(selector);

    Realized<Seq<decimal?>> ISeqVisitor.MaxNullableDecimal(MaxNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Max());

    Realized<Seq<decimal?>> ISeqVisitor.MaxNullableDecimalSelector<TSource>(
        MaxNullableDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal?>>(
            MaxNullableDecimalSelectorImpl<TSource>, MaxNullableDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal?> MaxNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Max(selector);

    private static IAsyncObservable<decimal?> MaxNullableDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal?>> selector) =>
        source.Max(selector);

    Realized<Seq<T>> ISeqVisitor.Max<T>(MaxSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MaxImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.MaxComparer<T>(MaxComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MaxComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<TResult>> ISeqVisitor.MaxSelector<TSource, TResult>(
        MaxSelectorSeq<TSource, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            MaxSelectorImpl<TSource, TResult>, MaxSelectorAsyncImpl<TSource, TResult>, seq.Source,
            seq.Selector);

    Realized<Seq<TResult>> ISeqVisitor.MaxSelectorComparer<TSource, TResult>(
        MaxSelectorComparerSeq<TSource, TResult> seq) =>
        RunByForm<Seq<TResult>>(
            MaxSelectorComparerImpl<TSource, TResult>,
            MaxSelectorComparerAsyncImpl<TSource, TResult>, seq.Source, seq.Selector, seq.Comparer);

    private static IAsyncObservable<T> MaxImpl<T>(IAsyncObservable<T> source) => source.Max();

    private static IAsyncObservable<T> MaxComparerImpl<T>(
        IAsyncObservable<T> source,
        IComparer<T> comparer) =>
        source.Max(comparer);

    private static IAsyncObservable<TResult> MaxSelectorImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector) =>
        source.Max(selector);

    private static IAsyncObservable<TResult> MaxSelectorAsyncImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TResult>> selector) =>
        source.Max(selector);

    private static IAsyncObservable<TResult> MaxSelectorComparerImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector,
        IComparer<TResult> comparer) =>
        source.Max(selector, comparer);

    private static IAsyncObservable<TResult> MaxSelectorComparerAsyncImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TResult>> selector,
        IComparer<TResult> comparer) =>
        source.Max(selector, comparer);
}
