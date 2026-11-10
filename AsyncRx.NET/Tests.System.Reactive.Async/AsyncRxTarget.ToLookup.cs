// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<ILookup<TKey, TSource>>> ISeqVisitor.ToLookup<TSource, TKey>(
        ToLookupSeq<TSource, TKey> seq) =>
        RunByForm<Seq<ILookup<TKey, TSource>>>(
            ToLookupImpl<TSource, TKey>, ToLookupAsyncImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector);

    private static IAsyncObservable<ILookup<TKey, TSource>> ToLookupImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector) =>
        source.ToLookup(keySelector);

    private static IAsyncObservable<ILookup<TKey, TSource>> ToLookupAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector) =>
        source.ToLookup(keySelector);

    Realized<Seq<ILookup<TKey, TSource>>> ISeqVisitor.ToLookupComparer<TSource, TKey>(
        ToLookupComparerSeq<TSource, TKey> seq) =>
        RunByForm<Seq<ILookup<TKey, TSource>>>(
            ToLookupComparerImpl<TSource, TKey>, ToLookupComparerAsyncImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IAsyncObservable<ILookup<TKey, TSource>> ToLookupComparerImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, comparer);

    private static IAsyncObservable<ILookup<TKey, TSource>> ToLookupComparerAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, comparer);

    Realized<Seq<ILookup<TKey, TElement>>> ISeqVisitor.ToLookupElement<TSource, TKey, TElement>(
        ToLookupElementSeq<TSource, TKey, TElement> seq) =>
        RunByForm<Seq<ILookup<TKey, TElement>>>(
            ToLookupElementImpl<TSource, TKey, TElement>,
            ToLookupElementAsyncImpl<TSource, TKey, TElement>, seq.Source, seq.KeySelector,
            seq.ElementSelector);

    private static IAsyncObservable<ILookup<TKey, TElement>> ToLookupElementImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector) =>
        source.ToLookup(keySelector, elementSelector);

    private static IAsyncObservable<ILookup<TKey, TElement>> ToLookupElementAsyncImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector) =>
        source.ToLookup(keySelector, elementSelector);

    Realized<Seq<ILookup<TKey, TElement>>> ISeqVisitor.ToLookupElementComparer<TSource, TKey, TElement>(
        ToLookupElementComparerSeq<TSource, TKey, TElement> seq) =>
        RunByForm<Seq<ILookup<TKey, TElement>>>(
            ToLookupElementComparerImpl<TSource, TKey, TElement>,
            ToLookupElementComparerAsyncImpl<TSource, TKey, TElement>, seq.Source, seq.KeySelector,
            seq.ElementSelector, seq.Comparer);

    private static IAsyncObservable<ILookup<TKey, TElement>> ToLookupElementComparerImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, elementSelector, comparer);

    private static IAsyncObservable<ILookup<TKey, TElement>> ToLookupElementComparerAsyncImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, elementSelector, comparer);
}
