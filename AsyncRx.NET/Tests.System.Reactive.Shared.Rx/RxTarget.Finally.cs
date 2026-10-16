// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Finally<T>(FinallySeq<T> seq) =>
        _bridge.Run<Seq<T>>(FinallyImpl<T>, seq.Source, seq.FinallyAction);

    private static IObservable<T> FinallyImpl<T>(
        IObservable<T> source,
        Action finallyAction) =>
        source.Finally(finallyAction);
}
