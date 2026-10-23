// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<TResult>> ISeqVisitor.WithLatestFrom<TFirst, TSecond, TResult>(
        WithLatestFromSeq<TFirst, TSecond, TResult> seq) =>
        _bridge.Run<Seq<TResult>>(
            WithLatestFromImpl<TFirst, TSecond, TResult>,
            seq.First,
            seq.Second,
            seq.ResultSelector);

    private static IAsyncObservable<TResult> WithLatestFromImpl<TFirst, TSecond, TResult>(
        IAsyncObservable<TFirst> first,
        IAsyncObservable<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector) =>
        first.WithLatestFrom(second, resultSelector);
}
