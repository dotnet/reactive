// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Catch<T>(CatchSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.CatchParams<T>(CatchParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.CatchEnumerable<T>(CatchEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchEnumerableImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.CatchHandler<T, TException>(CatchHandlerSeq<T, TException> seq) =>
        _bridge.Run<Seq<T>>(CatchHandlerImpl<T, TException>, seq.Source, seq.Handler);

    private static IAsyncObservable<T> CatchImpl<T>(
        IAsyncObservable<T> first,
        IAsyncObservable<T> second) =>
        first.Catch(second);

    private static IAsyncObservable<T> CatchParamsImpl<T>(IAsyncObservable<T>[] sources) =>
        AsyncObservable.Catch(sources);

    private static IAsyncObservable<T> CatchEnumerableImpl<T>(
        IEnumerable<IAsyncObservable<T>> sources) =>
        sources.Catch();

    private static IAsyncObservable<T> CatchHandlerImpl<T, TException>(
        IAsyncObservable<T> source,
        Func<TException, IAsyncObservable<T>> handler)
        where TException : Exception =>
        source.Catch(handler);
}
