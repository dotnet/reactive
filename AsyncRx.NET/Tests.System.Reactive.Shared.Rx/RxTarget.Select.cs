// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<TOut>> ISeqVisitor.Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) =>
        _bridge.Run<Seq<TOut>>(SelectIndexedImpl<TIn, TOut>, seq.Source, seq.Selector);

    private static IObservable<TOut> SelectImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, TOut> selector) =>
        source.Select(selector);

    private static IObservable<TOut> SelectIndexedImpl<TIn, TOut>(
        IObservable<TIn> source,
        Func<TIn, int, TOut> selector) =>
        source.Select(selector);
}
