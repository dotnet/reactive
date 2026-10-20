// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.RetryWhen<T, TSignal>(RetryWhenSeq<T, TSignal> seq) =>
        _bridge.Run<Seq<T>>(RetryWhenImpl<T, TSignal>, seq.Source, seq.Handler);

    private static IAsyncObservable<T> RetryWhenImpl<T, TSignal>(
        IAsyncObservable<T> source,
        Func<IAsyncObservable<Exception>, IAsyncObservable<TSignal>> handler) =>
        source.RetryWhen(handler);
}
