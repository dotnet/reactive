// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.AsObservable<T>(AsObservableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AsObservableImpl<T>, seq.Source);

    private static IObservable<T> AsObservableImpl<T>(IObservable<T> source) =>
        source.AsObservable();
}
