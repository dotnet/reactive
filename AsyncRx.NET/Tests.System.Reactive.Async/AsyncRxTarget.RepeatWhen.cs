// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.RepeatWhen<T, TSignal>(RepeatWhenSeq<T, TSignal> seq) =>
        _bridge.Run<Seq<T>>(RepeatWhenImpl<T, TSignal>, seq.Source, seq.Handler);

    private static IAsyncObservable<T> RepeatWhenImpl<T, TSignal>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<object>, IAsyncObservable<TSignal>> handler) =>
        source.RepeatWhen(handler);
}
