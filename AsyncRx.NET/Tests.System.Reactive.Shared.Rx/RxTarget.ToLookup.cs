// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<ILookup<TKey, TSource>>> ISeqVisitor.ToLookup<TSource, TKey>(
        ToLookupSeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<ILookup<TKey, TSource>>>(
            ToLookupImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector);

    private static IObservable<ILookup<TKey, TSource>> ToLookupImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector) =>
        source.ToLookup(keySelector);

    Realized<Seq<ILookup<TKey, TSource>>> ISeqVisitor.ToLookupComparer<TSource, TKey>(
        ToLookupComparerSeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<ILookup<TKey, TSource>>>(
            ToLookupComparerImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IObservable<ILookup<TKey, TSource>> ToLookupComparerImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, comparer);

    Realized<Seq<ILookup<TKey, TElement>>> ISeqVisitor.ToLookupElement<TSource, TKey, TElement>(
        ToLookupElementSeq<TSource, TKey, TElement> seq) =>
        _bridge.Run<Seq<ILookup<TKey, TElement>>>(
            ToLookupElementImpl<TSource, TKey, TElement>,
            seq.Source,
            seq.KeySelector,
            seq.ElementSelector);

    private static IObservable<ILookup<TKey, TElement>> ToLookupElementImpl<TSource, TKey, TElement>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector) =>
        source.ToLookup(keySelector, elementSelector);

    Realized<Seq<ILookup<TKey, TElement>>> ISeqVisitor.ToLookupElementComparer<TSource, TKey, TElement>(
        ToLookupElementComparerSeq<TSource, TKey, TElement> seq) =>
        _bridge.Run<Seq<ILookup<TKey, TElement>>>(
            ToLookupElementComparerImpl<TSource, TKey, TElement>,
            seq.Source,
            seq.KeySelector,
            seq.ElementSelector,
            seq.Comparer);

    private static IObservable<ILookup<TKey, TElement>> ToLookupElementComparerImpl<TSource, TKey, TElement>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> elementSelector,
        IEqualityComparer<TKey> comparer) =>
        source.ToLookup(keySelector, elementSelector, comparer);
}
