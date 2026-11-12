// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SkipWhile<T>(SkipWhileSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipWhileImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.SkipWhileIndexed<T>(SkipWhileIndexedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipWhileIndexedImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T> SkipWhileImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.SkipWhile(predicate);

    private static IObservable<T> SkipWhileIndexedImpl<T>(
        IObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.SkipWhile(predicate);
}
