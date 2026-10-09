// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Multicast<T>(MulticastSeq<T> seq) =>
        _bridge.Run<Seq<T>>(MulticastImpl<T>, seq.Source, seq.Subject);

    Realized<Seq<TResult>> ISeqVisitor.MulticastSelector<T, TResult>(
        MulticastSelectorSeq<T, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            MulticastSelectorImpl<T, TResult>,
            seq.Source,
            seq.SubjectSelector,
            seq.Selector);

    private static IConnectableAsyncObservable<T> MulticastImpl<T>(
        IAsyncObservable<T> source,
        IAsyncSubject<T> subject) =>
        source.Multicast(subject);

    private static IAsyncObservable<TResult> MulticastSelectorImpl<T, TResult>(
        IAsyncObservable<T> source,
        Func<IAsyncSubject<T>> subjectSelector,
        Func<IAsyncObservable<T>, IAsyncObservable<TResult>> selector) =>
        source.Multicast(subjectSelector, selector);
}
