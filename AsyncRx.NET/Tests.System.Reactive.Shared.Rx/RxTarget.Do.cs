// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Do<T>(DoSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DoImpl<T>, seq.Source, seq.OnNext);

    private static IObservable<T> DoImpl<T>(
        IObservable<T> source,
        Action<T> onNext) =>
        source.Do(onNext);
}
