// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.TakeWhile<T>(TakeWhileSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeWhileImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.TakeWhileIndexed<T>(TakeWhileIndexedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeWhileIndexedImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T> TakeWhileImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.TakeWhile(predicate);

    private static IObservable<T> TakeWhileIndexedImpl<T>(
        IObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.TakeWhile(predicate);
}
