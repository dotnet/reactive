// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<Notification<T>>> ISeqVisitor.Materialize<T>(MaterializeSeq<T> seq) =>
        _bridge.Run<Seq<Notification<T>>>(MaterializeImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.Dematerialize<T>(DematerializeSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DematerializeImpl<T>, seq.Source);

    private static IObservable<Notification<T>> MaterializeImpl<T>(
        IObservable<T> source) =>
        source.Materialize();

    private static IObservable<T> DematerializeImpl<T>(
        IObservable<Notification<T>> source) =>
        source.Dematerialize();
}
