// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.AutoConnect<T>(AutoConnectSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AutoConnectImpl<T>, seq.Source, seq.MinObservers, seq.OnConnect);

    private static IAsyncObservable<T> AutoConnectImpl<T>(
        IConnectableAsyncObservable<T> source,
        int minObservers,
        Action<IAsyncDisposable>? onConnect) =>
        source.AutoConnect(minObservers, onConnect);
}
