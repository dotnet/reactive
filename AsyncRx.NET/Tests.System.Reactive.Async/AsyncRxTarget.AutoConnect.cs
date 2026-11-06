// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    // The asynchronous overload has no default for its handler, so a scenario without one has
    // only the synchronous call.
    Realized<Seq<T>> ISeqVisitor.AutoConnect<T>(AutoConnectSeq<T> seq) =>
        seq.OnConnect is null
            ? _bridge.Run<Seq<T>>(AutoConnectImpl<T>, seq.Source, seq.MinObservers, seq.OnConnect)
            : RunByForm<Seq<T>>(
                AutoConnectImpl<T>,
                AutoConnectAsyncImpl<T>,
                seq.Source,
                seq.MinObservers,
                seq.OnConnect);

    private static IAsyncObservable<T> AutoConnectImpl<T>(
        IConnectableAsyncObservable<T> source,
        int minObservers,
        Action<IAsyncDisposable>? onConnect) =>
        source.AutoConnect(minObservers, onConnect);

    private static IAsyncObservable<T> AutoConnectAsyncImpl<T>(
        IConnectableAsyncObservable<T> source,
        int minObservers,
        Func<IAsyncDisposable, ValueTask> onConnect) =>
        source.AutoConnect(minObservers, onConnect);
}
