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
    Realized<Seq<double>> ISeqVisitor.AverageInt32(AverageInt32Seq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Average());

    Realized<Seq<double>> ISeqVisitor.AverageInt32Selector<TSource>(
        AverageInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            AverageInt32SelectorImpl<TSource>, AverageInt32SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double> AverageInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int>> selector) =>
        source.Average(selector);

    Realized<Seq<double>> ISeqVisitor.AverageInt64(AverageInt64Seq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Average());

    Realized<Seq<double>> ISeqVisitor.AverageInt64Selector<TSource>(
        AverageInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            AverageInt64SelectorImpl<TSource>, AverageInt64SelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double> AverageInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long>> selector) =>
        source.Average(selector);

    Realized<Seq<float>> ISeqVisitor.AverageSingle(AverageSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Average());

    Realized<Seq<float>> ISeqVisitor.AverageSingleSelector<TSource>(
        AverageSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float>>(
            AverageSingleSelectorImpl<TSource>, AverageSingleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> AverageSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Average(selector);

    private static IAsyncObservable<float> AverageSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float>> selector) =>
        source.Average(selector);

    Realized<Seq<double>> ISeqVisitor.AverageDouble(AverageDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Average());

    Realized<Seq<double>> ISeqVisitor.AverageDoubleSelector<TSource>(
        AverageDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double>>(
            AverageDoubleSelectorImpl<TSource>, AverageDoubleSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double> AverageDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double>> selector) =>
        source.Average(selector);

    Realized<Seq<decimal>> ISeqVisitor.AverageDecimal(AverageDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Average());

    Realized<Seq<decimal>> ISeqVisitor.AverageDecimalSelector<TSource>(
        AverageDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal>>(
            AverageDecimalSelectorImpl<TSource>, AverageDecimalSelectorAsyncImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> AverageDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Average(selector);

    private static IAsyncObservable<decimal> AverageDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal>> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt32(AverageNullableInt32Seq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt32Selector<TSource>(
        AverageNullableInt32SelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            AverageNullableInt32SelectorImpl<TSource>,
            AverageNullableInt32SelectorAsyncImpl<TSource>, seq.Source, seq.Selector);

    private static IAsyncObservable<double?> AverageNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double?> AverageNullableInt32SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<int?>> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt64(AverageNullableInt64Seq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt64Selector<TSource>(
        AverageNullableInt64SelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            AverageNullableInt64SelectorImpl<TSource>,
            AverageNullableInt64SelectorAsyncImpl<TSource>, seq.Source, seq.Selector);

    private static IAsyncObservable<double?> AverageNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double?> AverageNullableInt64SelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<long?>> selector) =>
        source.Average(selector);

    Realized<Seq<float?>> ISeqVisitor.AverageNullableSingle(AverageNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Average());

    Realized<Seq<float?>> ISeqVisitor.AverageNullableSingleSelector<TSource>(
        AverageNullableSingleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<float?>>(
            AverageNullableSingleSelectorImpl<TSource>,
            AverageNullableSingleSelectorAsyncImpl<TSource>, seq.Source, seq.Selector);

    private static IAsyncObservable<float?> AverageNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Average(selector);

    private static IAsyncObservable<float?> AverageNullableSingleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<float?>> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableDouble(AverageNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableDoubleSelector<TSource>(
        AverageNullableDoubleSelectorSeq<TSource> seq) =>
        RunByForm<Seq<double?>>(
            AverageNullableDoubleSelectorImpl<TSource>,
            AverageNullableDoubleSelectorAsyncImpl<TSource>, seq.Source, seq.Selector);

    private static IAsyncObservable<double?> AverageNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Average(selector);

    private static IAsyncObservable<double?> AverageNullableDoubleSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<double?>> selector) =>
        source.Average(selector);

    Realized<Seq<decimal?>> ISeqVisitor.AverageNullableDecimal(AverageNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Average());

    Realized<Seq<decimal?>> ISeqVisitor.AverageNullableDecimalSelector<TSource>(
        AverageNullableDecimalSelectorSeq<TSource> seq) =>
        RunByForm<Seq<decimal?>>(
            AverageNullableDecimalSelectorImpl<TSource>,
            AverageNullableDecimalSelectorAsyncImpl<TSource>, seq.Source, seq.Selector);

    private static IAsyncObservable<decimal?> AverageNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Average(selector);

    private static IAsyncObservable<decimal?> AverageNullableDecimalSelectorAsyncImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<decimal?>> selector) =>
        source.Average(selector);
}
