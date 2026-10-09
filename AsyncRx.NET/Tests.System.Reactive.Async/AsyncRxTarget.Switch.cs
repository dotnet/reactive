// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Switch<T>(SwitchSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SwitchImpl<T>, seq.Sources);

    private static IAsyncObservable<T> SwitchImpl<T>(
        IAsyncObservable<IAsyncObservable<T>> sources) =>
        sources.Switch();
}
