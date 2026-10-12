// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Repeat<T>(RepeatSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RepeatCount<T>(RepeatCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RepeatCountImpl<T>, seq.Source, seq.RepeatCount);

    private static IObservable<T> RepeatImpl<T>(IObservable<T> source) => source.Repeat();

    private static IObservable<T> RepeatCountImpl<T>(IObservable<T> source, int repeatCount) =>
        source.Repeat(repeatCount);
}
