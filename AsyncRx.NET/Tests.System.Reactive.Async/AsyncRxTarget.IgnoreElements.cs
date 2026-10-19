// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.IgnoreElements<T>(IgnoreElementsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(IgnoreElementsImpl<T>, seq.Source);

    private static IAsyncObservable<T> IgnoreElementsImpl<T>(IAsyncObservable<T> source) =>
        source.IgnoreElements();
}
