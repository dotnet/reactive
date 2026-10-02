// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Create<T>(CreateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateDisposable<T>(CreateDisposableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateDisposableImpl<T>, seq.Subscribe);

    // AsyncRx.NET has no Create over a callback that returns an action; Rx.NET defines that
    // form as the disposable form over Disposable.Create(action), with null meaning no action
    // (a plumbing decision, as for SelectMany(other)).
    private static IAsyncObservable<T> CreateImpl<T>(
        Func<IAsyncObserver<T>, ValueTask<Action?>> subscribe) =>
        AsyncObservable.Create<T>(async o =>
        {
            var action = await subscribe(o);
            return action is null
                ? AsyncDisposable.Nop
                : AsyncDisposable.Create(() =>
                {
                    action();
                    return default;
                });
        });

    // A null result is passed through, since coalescing it is the library's job.
    private static IAsyncObservable<T> CreateDisposableImpl<T>(
        Func<IAsyncObserver<T>, ValueTask<IAsyncDisposable?>> subscribe) =>
        AsyncObservable.Create<T>(subscribe!);
}
