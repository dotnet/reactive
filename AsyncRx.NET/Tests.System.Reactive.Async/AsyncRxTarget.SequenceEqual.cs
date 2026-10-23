// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<bool>> ISeqVisitor.SequenceEqual<T>(SequenceEqualSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(SequenceEqualImpl<T>, seq.First, seq.Second);

    Realized<Seq<bool>> ISeqVisitor.SequenceEqualComparer<T>(SequenceEqualComparerSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(SequenceEqualComparerImpl<T>, seq.First, seq.Second, seq.Comparer);

    Realized<Seq<bool>> ISeqVisitor.SequenceEqualEnumerable<T>(SequenceEqualEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(SequenceEqualEnumerableImpl<T>, seq.First, seq.Second);

    Realized<Seq<bool>> ISeqVisitor.SequenceEqualEnumerableComparer<T>(
        SequenceEqualEnumerableComparerSeq<T> seq) =>
        _bridge.Run<Seq<bool>>(
            SequenceEqualEnumerableComparerImpl<T>, seq.First, seq.Second, seq.Comparer);

    private static IAsyncObservable<bool> SequenceEqualImpl<T>(IAsyncObservable<T> first, IAsyncObservable<T> second) =>
        first.SequenceEqual(second);

    private static IAsyncObservable<bool> SequenceEqualComparerImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second,
        IEqualityComparer<T> comparer) =>
        first.SequenceEqual(second, comparer);

    private static IAsyncObservable<bool> SequenceEqualEnumerableImpl<T>(
        IAsyncObservable<T> first,
        IEnumerable<T> second) =>
        first.SequenceEqual(second);

    private static IAsyncObservable<bool> SequenceEqualEnumerableComparerImpl<T>(
        IAsyncObservable<T> first,
        IEnumerable<T> second,
        IEqualityComparer<T> comparer) =>
        first.SequenceEqual(second, comparer);
}
