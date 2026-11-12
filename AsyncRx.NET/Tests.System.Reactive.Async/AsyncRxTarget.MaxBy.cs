// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<IList<TSource>>> ISeqVisitor.MaxBy<TSource, TKey>(MaxBySeq<TSource, TKey> seq) =>
        RunByForm<Seq<IList<TSource>>>(
            MaxByImpl<TSource, TKey>, MaxByAsyncImpl<TSource, TKey>, seq.Source, seq.KeySelector);

    Realized<Seq<IList<TSource>>> ISeqVisitor.MaxByComparer<TSource, TKey>(
        MaxByComparerSeq<TSource, TKey> seq) =>
        RunByForm<Seq<IList<TSource>>>(
            MaxByComparerImpl<TSource, TKey>, MaxByComparerAsyncImpl<TSource, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IAsyncObservable<IList<TSource>> MaxByImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector) =>
        source.MaxBy(keySelector);

    private static IAsyncObservable<IList<TSource>> MaxByAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector) =>
        source.MaxBy(keySelector);

    private static IAsyncObservable<IList<TSource>> MaxByComparerImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, TKey> keySelector,
        IComparer<TKey> comparer) =>
        source.MaxBy(keySelector, comparer);

    private static IAsyncObservable<IList<TSource>> MaxByComparerAsyncImpl<TSource, TKey>(
        IAsyncObservable<TSource> source,
        Func<TSource, ValueTask<TKey>> keySelector,
        IComparer<TKey> comparer) =>
        source.MaxBy(keySelector, comparer);
}
