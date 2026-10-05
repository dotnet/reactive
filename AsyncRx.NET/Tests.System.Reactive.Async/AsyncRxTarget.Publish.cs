// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Publish<T>(PublishSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PublishImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.PublishInitial<T>(PublishInitialSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PublishInitialImpl<T>, seq.Source, seq.InitialValue);

    Realized<Seq<TResult>> ISeqVisitor.PublishSelector<T, TResult>(PublishSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(PublishSelectorImpl<T, TResult>, seq.Source, seq.Selector);

    Realized<Seq<TResult>> ISeqVisitor.PublishSelectorInitial<T, TResult>(
        PublishSelectorInitialSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            PublishSelectorInitialImpl<T, TResult>,
            seq.Source,
            seq.Selector,
            seq.InitialValue);

    private static IConnectableAsyncObservable<T> PublishImpl<T>(IAsyncObservable<T> source) => source.Publish();

    private static IConnectableAsyncObservable<T> PublishInitialImpl<T>(IAsyncObservable<T> source, T initialValue) =>
        source.Publish(initialValue);

    private static IAsyncObservable<TResult> PublishSelectorImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector) =>
        source.Publish(selector);

    private static IAsyncObservable<TResult> PublishSelectorInitialImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector,
        T initialValue) =>
        source.Publish(selector, initialValue);
}
