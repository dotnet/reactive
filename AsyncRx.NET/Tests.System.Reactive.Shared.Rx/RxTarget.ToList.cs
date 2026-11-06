// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<IList<T>>> ISeqVisitor.ToList<T>(ToListSeq<T> seq) =>
        _bridge.Run<Seq<IList<T>>>(ToListImpl<T>, seq.Source);

    private static IObservable<IList<T>> ToListImpl<T>(IObservable<T> source) => source.ToList();
}
