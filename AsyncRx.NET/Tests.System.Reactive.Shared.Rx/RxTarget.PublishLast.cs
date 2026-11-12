// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.PublishLast<T>(PublishLastSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PublishLastImpl<T>, seq.Source);

    Realized<Seq<TResult>> ISeqVisitor.PublishLastSelector<T, TResult>(
        PublishLastSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(PublishLastSelectorImpl<T, TResult>, seq.Source, seq.Selector);

    private static IConnectableObservable<T> PublishLastImpl<T>(IObservable<T> source) =>
        source.PublishLast();

    private static IObservable<TResult> PublishLastSelectorImpl<T, TResult>(
        IObservable<T> source,
        Func<IObservable<T>, IObservable<TResult>> selector) =>
        source.PublishLast(selector);
}
