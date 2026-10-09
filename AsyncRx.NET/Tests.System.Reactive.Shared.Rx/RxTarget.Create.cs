// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Create<T>(CreateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateDisposable<T>(CreateDisposableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateDisposableImpl<T>, seq.Subscribe);

    // The shared callback is async-shaped; on this target it completes synchronously, so the
    // real Create receives a synchronous subscribe function. A null result is passed through,
    // since coalescing it is the library's job.
    private static IObservable<T> CreateImpl<T>(Func<IObserver<T>, ValueTask<Action?>> subscribe) =>
        Observable.Create<T>(o => Complete(subscribe(o))!);

    private static IObservable<T> CreateDisposableImpl<T>(
        Func<IObserver<T>, ValueTask<IAsyncDisposable?>> subscribe) =>
        Observable.Create<T>(o => Complete(subscribe(o)) is { } d
            ? Disposable.Create(() => Complete(d.DisposeAsync()))
            : null!);
}
