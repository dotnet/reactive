// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Skip<T>(SkipSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SkipImpl<T>, seq.Source, seq.Count);

    private static IObservable<T> SkipImpl<T>(IObservable<T> source, int count) =>
        source.Skip(count);
}
