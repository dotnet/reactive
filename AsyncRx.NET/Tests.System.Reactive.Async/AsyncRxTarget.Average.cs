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
        _bridge.Run<Seq<double>>(
            AverageInt32SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int> selector) =>
        source.Average(selector);

    Realized<Seq<double>> ISeqVisitor.AverageInt64(AverageInt64Seq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Average());

    Realized<Seq<double>> ISeqVisitor.AverageInt64Selector<TSource>(
        AverageInt64SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double>>(
            AverageInt64SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long> selector) =>
        source.Average(selector);

    Realized<Seq<float>> ISeqVisitor.AverageSingle(AverageSingleSeq seq) =>
        Realized.Of<Seq<float>>(Materialize(seq.Source).Average());

    Realized<Seq<float>> ISeqVisitor.AverageSingleSelector<TSource>(
        AverageSingleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<float>>(
            AverageSingleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float> AverageSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float> selector) =>
        source.Average(selector);

    Realized<Seq<double>> ISeqVisitor.AverageDouble(AverageDoubleSeq seq) =>
        Realized.Of<Seq<double>>(Materialize(seq.Source).Average());

    Realized<Seq<double>> ISeqVisitor.AverageDoubleSelector<TSource>(
        AverageDoubleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double>>(
            AverageDoubleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double> AverageDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double> selector) =>
        source.Average(selector);

    Realized<Seq<decimal>> ISeqVisitor.AverageDecimal(AverageDecimalSeq seq) =>
        Realized.Of<Seq<decimal>>(Materialize(seq.Source).Average());

    Realized<Seq<decimal>> ISeqVisitor.AverageDecimalSelector<TSource>(
        AverageDecimalSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<decimal>>(
            AverageDecimalSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal> AverageDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt32(AverageNullableInt32Seq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt32Selector<TSource>(
        AverageNullableInt32SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double?>>(
            AverageNullableInt32SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> AverageNullableInt32SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, int?> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt64(AverageNullableInt64Seq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableInt64Selector<TSource>(
        AverageNullableInt64SelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double?>>(
            AverageNullableInt64SelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> AverageNullableInt64SelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, long?> selector) =>
        source.Average(selector);

    Realized<Seq<float?>> ISeqVisitor.AverageNullableSingle(AverageNullableSingleSeq seq) =>
        Realized.Of<Seq<float?>>(Materialize(seq.Source).Average());

    Realized<Seq<float?>> ISeqVisitor.AverageNullableSingleSelector<TSource>(
        AverageNullableSingleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<float?>>(
            AverageNullableSingleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<float?> AverageNullableSingleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, float?> selector) =>
        source.Average(selector);

    Realized<Seq<double?>> ISeqVisitor.AverageNullableDouble(AverageNullableDoubleSeq seq) =>
        Realized.Of<Seq<double?>>(Materialize(seq.Source).Average());

    Realized<Seq<double?>> ISeqVisitor.AverageNullableDoubleSelector<TSource>(
        AverageNullableDoubleSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<double?>>(
            AverageNullableDoubleSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<double?> AverageNullableDoubleSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, double?> selector) =>
        source.Average(selector);

    Realized<Seq<decimal?>> ISeqVisitor.AverageNullableDecimal(AverageNullableDecimalSeq seq) =>
        Realized.Of<Seq<decimal?>>(Materialize(seq.Source).Average());

    Realized<Seq<decimal?>> ISeqVisitor.AverageNullableDecimalSelector<TSource>(
        AverageNullableDecimalSelectorSeq<TSource> seq) =>
        _bridge.Run<Seq<decimal?>>(
            AverageNullableDecimalSelectorImpl<TSource>,
            seq.Source,
            seq.Selector);

    private static IAsyncObservable<decimal?> AverageNullableDecimalSelectorImpl<TSource>(
        IAsyncObservable<TSource> source,
        Func<TSource, decimal?> selector) =>
        source.Average(selector);
}
