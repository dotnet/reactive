// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.PublishLast<T>(PublishLastSeq<T> seq) =>
        _bridge.Run<Seq<T>>(PublishLastImpl<T>, seq.Source);

    Realized<Seq<TResult>> ISeqVisitor.PublishLastSelector<T, TResult>(
        PublishLastSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(PublishLastSelectorImpl<T, TResult>, seq.Source, seq.Selector);

    private static IConnectableAsyncObservable<T> PublishLastImpl<T>(IAsyncObservable<T> source) =>
        source.PublishLast();

    private static IAsyncObservable<TResult> PublishLastSelectorImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector) =>
        source.PublishLast(selector);
}
