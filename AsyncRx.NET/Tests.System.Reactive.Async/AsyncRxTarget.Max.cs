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
        _bridge.Run<Seq<int>>(
            MaxInt32SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int> MaxInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Max(selector);

    Realized<Seq<long>> ISeqVisitor.MaxInt64(MaxInt64Seq seq) =>
        Realized.Of<Seq<long>>(Materialize(seq.Source).Max());

    Realized<Seq<long>> ISeqVisitor.MaxInt64Selector<TSource>(
        MaxInt64SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<long>>(
            MaxInt64SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long> MaxInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Max(selector);

    Realized<Seq<float>> ISeqVisitor.MaxSingle(MaxSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Max());

    Realized<Seq<float>> ISeqVisitor.MaxSingleSelector<TSource>(
        MaxSingleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<float>>(
            MaxSingleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> MaxSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Max(selector);

    Realized<Seq<double>> ISeqVisitor.MaxDouble(MaxDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Max());

    Realized<Seq<double>> ISeqVisitor.MaxDoubleSelector<TSource>(
        MaxDoubleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double>>(
            MaxDoubleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> MaxDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Max(selector);

    Realized<Seq<decimal>> ISeqVisitor.MaxDecimal(MaxDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Max());

    Realized<Seq<decimal>> ISeqVisitor.MaxDecimalSelector<TSource>(
        MaxDecimalSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<decimal>>(
            MaxDecimalSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> MaxDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Max(selector);

    Realized<Seq<int?>> ISeqVisitor.MaxNullableInt32(MaxNullableInt32Seq seq) =>
        Realized.Of<Seq<int?>>(Materialize(seq.Source).Max());

    Realized<Seq<int?>> ISeqVisitor.MaxNullableInt32Selector<TSource>(
        MaxNullableInt32SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<int?>>(
            MaxNullableInt32SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<int?> MaxNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Max(selector);

    Realized<Seq<long?>> ISeqVisitor.MaxNullableInt64(MaxNullableInt64Seq seq) =>
        Realized.Of<Seq<long?>>(Materialize(seq.Source).Max());

    Realized<Seq<long?>> ISeqVisitor.MaxNullableInt64Selector<TSource>(
        MaxNullableInt64SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<long?>>(
            MaxNullableInt64SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<long?> MaxNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Max(selector);

    Realized<Seq<float?>> ISeqVisitor.MaxNullableSingle(MaxNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Max());

    Realized<Seq<float?>> ISeqVisitor.MaxNullableSingleSelector<TSource>(
        MaxNullableSingleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<float?>>(
            MaxNullableSingleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float?> MaxNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Max(selector);

    Realized<Seq<double?>> ISeqVisitor.MaxNullableDouble(MaxNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Max());

    Realized<Seq<double?>> ISeqVisitor.MaxNullableDoubleSelector<TSource>(
        MaxNullableDoubleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double?>>(
            MaxNullableDoubleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> MaxNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Max(selector);

    Realized<Seq<decimal?>> ISeqVisitor.MaxNullableDecimal(MaxNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Max());

    Realized<Seq<decimal?>> ISeqVisitor.MaxNullableDecimalSelector<TSource>(
        MaxNullableDecimalSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<decimal?>>(
            MaxNullableDecimalSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal?> MaxNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Max(selector);

    Realized<Seq<T>> ISeqVisitor.Max<T>(MaxSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MaxImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.MaxComparer<T>(MaxComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MaxComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<TResult>> ISeqVisitor.MaxSelector<TSource, TResult>(
        MaxSelectorSeq<TSource, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(MaxSelectorImpl<TSource, TResult>, seq.Source, seq.Selector);

    Realized<Seq<TResult>> ISeqVisitor.MaxSelectorComparer<TSource, TResult>(
        MaxSelectorComparerSeq<TSource, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            MaxSelectorComparerImpl<TSource, TResult>,
            seq.Source,
            seq.Selector,
            seq.Comparer);

    private static IAsyncObservable<T> MaxImpl<T>(IAsyncObservable<T> source) => source.Max();

    private static IAsyncObservable<T> MaxComparerImpl<T>(
        IAsyncObservable<T> source,
        IComparer<T> comparer) =>
        source.Max(comparer);

    private static IAsyncObservable<TResult> MaxSelectorImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector) =>
        source.Max(selector);

    private static IAsyncObservable<TResult> MaxSelectorComparerImpl<TSource, TResult>(
        IAsyncObservable<TSource> source,
        Func<TSource, TResult> selector,
        IComparer<TResult> comparer) =>
        source.Max(selector, comparer);
}
