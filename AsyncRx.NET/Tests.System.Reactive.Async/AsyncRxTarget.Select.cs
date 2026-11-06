// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TOut>> ISeqVisitor.Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) =>
        RunByForm<Seq<TOut>>(
            SelectImpl<TIn, TOut>, SelectAsyncImpl<TIn, TOut>, seq.Source, seq.Selector);

    Realized<Seq<TOut>> ISeqVisitor.SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) =>
        RunByForm<Seq<TOut>>(
            SelectIndexedImpl<TIn, TOut>, SelectIndexedAsyncImpl<TIn, TOut>, seq.Source,
            seq.Selector);

    private static IAsyncObservable<TOut> SelectImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, TOut> selector) =>
        source.Select(selector);

    private static IAsyncObservable<TOut> SelectAsyncImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, ValueTask<TOut>> selector) =>
        source.Select(selector);

    private static IAsyncObservable<TOut> SelectIndexedImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, TOut> selector) =>
        source.Select(selector);

    private static IAsyncObservable<TOut> SelectIndexedAsyncImpl<TIn, TOut>(
        IAsyncObservable<TIn> source,
        Func<TIn, int, ValueTask<TOut>> selector) =>
        source.Select(selector);
}
