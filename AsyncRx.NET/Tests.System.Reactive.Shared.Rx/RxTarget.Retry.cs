// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Retry<T>(RetrySeq<T> seq) =>
        _bridge.Run<Seq<T>>(RetryImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RetryCount<T>(RetryCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RetryCountImpl<T>, seq.Source, seq.RetryCount);

    private static IObservable<T> RetryImpl<T>(IObservable<T> source) => source.Retry();

    private static IObservable<T> RetryCountImpl<T>(
        IObservable<T> source,
        int retryCount) =>
        source.Retry(retryCount);
}
