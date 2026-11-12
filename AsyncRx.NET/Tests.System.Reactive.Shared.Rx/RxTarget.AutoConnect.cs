// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.AutoConnect<T>(AutoConnectSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AutoConnectImpl<T>, seq.Source, seq.MinObservers, seq.OnConnect);

    // The scenario's callback takes the kit's asynchronous disposable; Rx.NET hands an
    // IDisposable, wrapped here as the target wraps every disposable it returns.
    [AdaptsLibraryCall]
    private static IObservable<T> AutoConnectImpl<T>(
        IConnectableObservable<T> source,
        int minObservers,
        Action<IAsyncDisposable>? onConnect) =>
        source.AutoConnect(
            minObservers,
            onConnect is null ? null : d => onConnect(RxDisposable.For(d)));
}
