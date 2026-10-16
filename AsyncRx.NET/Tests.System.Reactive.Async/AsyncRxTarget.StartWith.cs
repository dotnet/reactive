// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.StartWith<T>(StartWithSeq<T> seq) =>
        _bridge.Run<Seq<T>>(StartWithImpl<T>, seq.Source, seq.Values);

    private static IAsyncObservable<T> StartWithImpl<T>(
        IAsyncObservable<T> source,
        T[] values) =>
        source.StartWith(values);
}
