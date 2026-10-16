// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<bool>> ISeqVisitor.Any<T>(AnySeq<T> seq) =>
        _bridge.Run<Seq<bool>>(AnyImpl<T>, seq.Source);

    Realized<Seq<bool>> ISeqVisitor.AnyPredicate<T>(AnyPredicateSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(AnyPredicateImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<bool>> ISeqVisitor.All<T>(AllSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(AllImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<bool>> ISeqVisitor.Contains<T>(ContainsSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(ContainsImpl<T>, seq.Source, seq.Value);

    Realized<Seq<bool>> ISeqVisitor.ContainsComparer<T>(ContainsComparerSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(ContainsComparerImpl<T>, seq.Source, seq.Value, seq.Comparer);

    Realized<Seq<bool>> ISeqVisitor.IsEmpty<T>(IsEmptySeq<T> seq) =>
        _bridge.Run<Seq<bool>>(IsEmptyImpl<T>, seq.Source);

    private static IAsyncObservable<bool> AnyImpl<T>(IAsyncObservable<T> source) => source.Any();

    private static IAsyncObservable<bool> AnyPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.Any(predicate);

    private static IAsyncObservable<bool> AllImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.All(predicate);

    private static IAsyncObservable<bool> ContainsImpl<T>(IAsyncObservable<T> source, T value) =>
        source.Contains(value);

    private static IAsyncObservable<bool> ContainsComparerImpl<T>(
        IAsyncObservable<T> source,
        T value,
        IEqualityComparer<T> comparer) =>
        source.Contains(value, comparer);

    private static IAsyncObservable<bool> IsEmptyImpl<T>(IAsyncObservable<T> source) =>
        source.IsEmpty();
}
