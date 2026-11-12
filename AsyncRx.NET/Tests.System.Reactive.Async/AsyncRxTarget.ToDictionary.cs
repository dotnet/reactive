// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<IDictionary<TKey, TSource>>> ISeqVisitor.ToDictionary<TSource, TKey>(
        ToDictionarySeq<TSource, TKey> seq) =>
        RunByForm<Seq<IDictionary<TKey, TSource>>>(
            ToDictionaryImpl<TSource, TKey>, ToDictionaryAsyncImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector);

    private static IAsyncObservable<IDictionary<TKey, TSource>> ToDictionaryImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector);

    private static IAsyncObservable<IDictionary<TKey, TSource>> ToDictionaryAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector);

    Realized<Seq<IDictionary<TKey, TSource>>> ISeqVisitor.ToDictionaryComparer<TSource, TKey>(
        ToDictionaryComparerSeq<TSource, TKey> seq) =>
        RunByForm<Seq<IDictionary<TKey, TSource>>>(
            ToDictionaryComparerImpl<TSource, TKey>, ToDictionaryComparerAsyncImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IAsyncObservable<IDictionary<TKey, TSource>> ToDictionaryComparerImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, comparer);

    private static IAsyncObservable<IDictionary<TKey, TSource>> ToDictionaryComparerAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, comparer);

    Realized<Seq<IDictionary<TKey, TElement>>> ISeqVisitor.ToDictionaryElement<TSource, TKey, TElement>(
        ToDictionaryElementSeq<TSource, TKey, TElement> seq) =>
        RunByForm<Seq<IDictionary<TKey, TElement>>>(
            ToDictionaryElementImpl<TSource, TKey, TElement>,
            ToDictionaryElementAsyncImpl<TSource, TKey, TElement>, seq.Source, seq.KeySelector,
            seq.ElementSelector);

    private static IAsyncObservable<IDictionary<TKey, TElement>> ToDictionaryElementImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector);

    private static IAsyncObservable<IDictionary<TKey, TElement>> ToDictionaryElementAsyncImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector);

    Realized<Seq<IDictionary<TKey, TElement>>> ISeqVisitor.ToDictionaryElementComparer<TSource, TKey, TElement>(
        ToDictionaryElementComparerSeq<TSource, TKey, TElement> seq) =>
        RunByForm<Seq<IDictionary<TKey, TElement>>>(
            ToDictionaryElementComparerImpl<TSource, TKey, TElement>,
            ToDictionaryElementComparerAsyncImpl<TSource, TKey, TElement>, seq.Source,
            seq.KeySelector, seq.ElementSelector, seq.Comparer);

    private static IAsyncObservable<IDictionary<TKey, TElement>> ToDictionaryElementComparerImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector, comparer);

    private static IAsyncObservable<IDictionary<TKey, TElement>> ToDictionaryElementComparerAsyncImpl<TSource, TKey, TElement>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        Func<TSource, ValueTask<TElement>> elementSelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector, comparer);
}
