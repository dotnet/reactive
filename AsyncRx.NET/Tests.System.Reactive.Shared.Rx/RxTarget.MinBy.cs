// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<IList<TSource>>> ISeqVisitor.MinBy<TSource, TKey>(MinBySeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<IList<TSource>>>(MinByImpl<TSource, TKey>, seq.Source, seq.KeySelector);

    Realized<Seq<IList<TSource>>> ISeqVisitor.MinByComparer<TSource, TKey>(
        MinByComparerSeq<TSource, TKey> seq) =>
        _bridge.Run<Seq<IList<TSource>>>(
            MinByComparerImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IObservable<IList<TSource>> MinByImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector) =>
        source.MinBy(keySelector);

    private static IObservable<IList<TSource>> MinByComparerImpl<TSource, TKey>(
        IObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey> comparer) =>
        source.MinBy(keySelector, comparer);
}
