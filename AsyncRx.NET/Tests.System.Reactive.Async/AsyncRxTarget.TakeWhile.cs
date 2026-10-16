// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.TakeWhile<T>(TakeWhileSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeWhileImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.TakeWhileIndexed<T>(TakeWhileIndexedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(TakeWhileIndexedImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> TakeWhileImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.TakeWhile(predicate);

    private static IAsyncObservable<T> TakeWhileIndexedImpl<T>(
        IAsyncObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.TakeWhile(predicate);
}
