// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IObservable<bool> AnyImpl<T>(IObservable<T> source) => source.Any();

    private static IObservable<bool> AnyPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.Any(predicate);

    private static IObservable<bool> AllImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.All(predicate);

    private static IObservable<bool> ContainsImpl<T>(IObservable<T> source, T value) =>
        source.Contains(value);

    private static IObservable<bool> ContainsComparerImpl<T>(
        IObservable<T> source,
        T value,
        IEqualityComparer<T> comparer) =>
        source.Contains(value, comparer);

    private static IObservable<bool> IsEmptyImpl<T>(IObservable<T> source) =>
        source.IsEmpty();
}
