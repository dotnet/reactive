// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Retry<T>(RetrySeq<T> seq) =>
        _bridge.Run<Seq<T>>(RetryImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RetryCount<T>(RetryCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RetryCountImpl<T>, seq.Source, seq.RetryCount);

    private static IAsyncObservable<T> RetryImpl<T>(IAsyncObservable<T> source) => source.Retry();

    private static IAsyncObservable<T> RetryCountImpl<T>(
        IAsyncObservable<T> source,
        int retryCount) =>
        source.Retry(retryCount);
}
