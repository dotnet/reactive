// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Where<T>(WhereSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhereImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.WhereIndexed<T>(WhereIndexedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(WhereIndexedImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T> WhereImpl<T>(IObservable<T> source, Func<T, bool> predicate) =>
        source.Where(predicate);

    private static IObservable<T> WhereIndexedImpl<T>(
        IObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.Where(predicate);
}
