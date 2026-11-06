// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<IDictionary<TKey, TSource>>> ISeqVisitor.ToDictionary<TSource, TKey>(
        ToDictionarySeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<IDictionary<TKey, TSource>>>(
            ToDictionaryImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector);

    private static IObservable<IDictionary<TKey, TSource>> ToDictionaryImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector);

    Realized<Seq<IDictionary<TKey, TSource>>> ISeqVisitor.ToDictionaryComparer<TSource, TKey>(
        ToDictionaryComparerSeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<IDictionary<TKey, TSource>>>(
            ToDictionaryComparerImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IObservable<IDictionary<TKey, TSource>> ToDictionaryComparerImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, comparer);

    Realized<Seq<IDictionary<TKey, TElement>>> ISeqVisitor.ToDictionaryElement<TSource, TKey, TElement>(
        ToDictionaryElementSeq<TSource, TKey, TElement> seq) =>
        _bridge.Run<Seq<IDictionary<TKey, TElement>>>(
            ToDictionaryElementImpl<TSource, TKey, TElement>,
            seq.Source,
            seq.KeySelector,
            seq.ElementSelector);

    private static IObservable<IDictionary<TKey, TElement>> ToDictionaryElementImpl<TSource, TKey, TElement>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector);

    Realized<Seq<IDictionary<TKey, TElement>>> ISeqVisitor.ToDictionaryElementComparer<TSource, TKey, TElement>(
        ToDictionaryElementComparerSeq<TSource, TKey, TElement> seq) =>
        _bridge.Run<Seq<IDictionary<TKey, TElement>>>(
            ToDictionaryElementComparerImpl<TSource, TKey, TElement>,
            seq.Source,
            seq.KeySelector,
            seq.ElementSelector,
            seq.Comparer);

    private static IObservable<IDictionary<TKey, TElement>> ToDictionaryElementComparerImpl<TSource, TKey, TElement>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer)
        where TKey : notnull =>
        source.ToDictionary(keySelector, elementSelector, comparer);
}
