// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Do<T>(DoSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoImpl<T>, seq.Source, seq.OnNext);

    Realized<Seq<T>> ISeqVisitor.DoNextCompleted<T>(DoNextCompletedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoNextCompletedImpl<T>, seq.Source, seq.OnNext, seq.OnCompleted);

    Realized<Seq<T>> ISeqVisitor.DoNextError<T>(DoNextErrorSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoNextErrorImpl<T>, seq.Source, seq.OnNext, seq.OnError);

    Realized<Seq<T>> ISeqVisitor.DoNextErrorCompleted<T>(DoNextErrorCompletedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(
            DoNextErrorCompletedImpl<T>, seq.Source, seq.OnNext, seq.OnError, seq.OnCompleted);

    Realized<Seq<T>> ISeqVisitor.DoObserver<T>(DoObserverSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoObserverImpl<T>, seq.Source, seq.Observer);

    private static IAsyncObservable<T> DoImpl<T>(
        IAsyncObservable<T> source,
        Action<T> onNext) =>
        source.Do(onNext);

    private static IAsyncObservable<T> DoNextCompletedImpl<T>(
        IAsyncObservable<T> source,
        Action<T> onNext,
        Action onCompleted) =>
        source.Do(onNext, onCompleted);

    private static IAsyncObservable<T> DoNextErrorImpl<T>(
        IAsyncObservable<T> source,
        Action<T> onNext,
        Action<Exception> onError) =>
        source.Do(onNext, onError);

    private static IAsyncObservable<T> DoNextErrorCompletedImpl<T>(
        IAsyncObservable<T> source,
        Action<T> onNext,
        Action<Exception> onError,
        Action onCompleted) =>
        source.Do(onNext, onError, onCompleted);

    private static IAsyncObservable<T> DoObserverImpl<T>(
        IAsyncObservable<T> source,
        IAsyncObserver<T> observer) =>
        source.Do(observer);
}
