// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Catch<T>(CatchSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.CatchParams<T>(CatchParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.CatchEnumerable<T>(CatchEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CatchEnumerableImpl<T>, seq.Sources);

    Realized<Seq<T>> ISeqVisitor.CatchHandler<T, TException>(CatchHandlerSeq<T, TException> seq) =>
        _bridge.Run<Seq<T>>(CatchHandlerImpl<T, TException>, seq.Source, seq.Handler);

    private static IObservable<T> CatchImpl<T>(
        IObservable<T> first,
        IObservable<T> second) =>
        first.Catch(second);

    private static IObservable<T> CatchParamsImpl<T>(IObservable<T>[] sources) =>
        Observable.Catch(sources);

    private static IObservable<T> CatchEnumerableImpl<T>(
        IEnumerable<IObservable<T>> sources) =>
        sources.Catch();

    private static IObservable<T> CatchHandlerImpl<T, TException>(
        IObservable<T> source,
        Func<TException, IObservable<T>> handler)
        where TException : Exception =>
        source.Catch(handler);
}
