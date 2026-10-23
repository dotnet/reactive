// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Amb<T>(AmbSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.AmbParams<T>(AmbParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.AmbEnumerable<T>(AmbEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbEnumerableImpl<T>, seq.Sources);

    private static IAsyncObservable<T> AmbImpl<T>(IAsyncObservable<T> first, IAsyncObservable<T> second) =>
        first.Amb(second);

    private static IAsyncObservable<T> AmbParamsImpl<T>(IAsyncObservable<T>[] sources) =>
        AsyncObservable.Amb(sources);

    private static IAsyncObservable<T> AmbEnumerableImpl<T>(IEnumerable<IAsyncObservable<T>> sources) =>
        sources.Amb();
}
