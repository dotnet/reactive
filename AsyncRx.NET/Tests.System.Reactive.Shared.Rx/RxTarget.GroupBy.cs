// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupBy<T, TKey, TElement>(
        GroupBySeq<T, TKey, TElement> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(
            GroupByImpl<T, TKey, TElement>,
            seq.Source,
            seq.KeySelector,
            seq.ElementSelector,
            seq.Comparer);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByImpl<T, TKey, TElement>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        IEqualityComparer<TKey> comparer) =>
        source.GroupBy(keySelector, elementSelector, comparer);
}
