// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IConnectableObservable<T> PublishImpl<T>(IObservable<T> source) => source.Publish();

    private static IConnectableObservable<T> PublishInitialImpl<T>(IObservable<T> source, T initialValue) =>
        source.Publish(initialValue);

    private static IObservable<TResult> PublishSelectorImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector) =>
        source.Publish(selector);

    private static IObservable<TResult> PublishSelectorInitialImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector,
        T initialValue) =>
        source.Publish(selector, initialValue);
}
