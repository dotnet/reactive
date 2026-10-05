// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
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

    private static IConnectableObservable<T> MulticastImpl<T>(
        IObservable<T> source,
        ISubject<T> subject) =>
        source.Multicast(subject);

    private static IObservable<TResult> MulticastSelectorImpl<T, TResult>(
        IObservable<T> source,
        Func<ISubject<T>> subjectSelector,
        Func<IObservable<T>, IObservable<TResult>> selector) =>
        source.Multicast(subjectSelector, selector);
}
