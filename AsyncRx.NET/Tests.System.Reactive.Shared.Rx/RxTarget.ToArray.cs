// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T[]>> ISeqVisitor.ToArray<T>(ToArraySeq<T> seq) =>
        _bridge.Run<Seq<T[]>>(ToArrayImpl<T>, seq.Source);

    private static IObservable<T[]> ToArrayImpl<T>(IObservable<T> source) => source.ToArray();
}
