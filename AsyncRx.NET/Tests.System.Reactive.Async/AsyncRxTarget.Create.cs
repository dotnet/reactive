// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Create<T>(CreateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateDisposable<T>(CreateDisposableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateDisposableImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateWithToken<T>(CreateWithTokenSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateWithTokenImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateDisposableWithToken<T>(
        CreateDisposableWithTokenSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateDisposableWithTokenImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateAutoCompleting<T>(CreateAutoCompletingSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateAutoCompletingImpl<T>, seq.Subscribe);

    Realized<Seq<T>> ISeqVisitor.CreateAutoCompletingWithToken<T>(
        CreateAutoCompletingWithTokenSeq<T> seq) =>
        _bridge.Run<Seq<T>>(CreateAutoCompletingWithTokenImpl<T>, seq.Subscribe);

    // AsyncRx.NET's Create overloads all take the token; the forms without one ignore it. The
    // action-returning forms hand the action over as the asynchronous unsubscription callback
    // AsyncRx.NET's Create takes in its place.

    [AdaptsLibraryCall]
    private static IAsyncObservable<T> CreateImpl<T>(
        Func<IAsyncObserver<T>, ValueTask<Action?>> subscribe) =>
        AsyncObservable.Create<T>(async (o, _) => AsDispose(await subscribe(o))!);

    [AdaptsLibraryCall]
    private static IAsyncObservable<T> CreateDisposableImpl<T>(
        Func<IAsyncObserver<T>, ValueTask<IAsyncDisposable?>> subscribe) =>
        AsyncObservable.Create<T>(async (o, _) => (await subscribe(o))!);

    [AdaptsLibraryCall]
    private static IAsyncObservable<T> CreateWithTokenImpl<T>(
        Func<IAsyncObserver<T>, CancellationToken, ValueTask<Action?>> subscribe) =>
        AsyncObservable.Create<T>(async (o, token) => AsDispose(await subscribe(o, token))!);

    [AdaptsLibraryCall]
    private static IAsyncObservable<T> CreateDisposableWithTokenImpl<T>(
        Func<IAsyncObserver<T>, CancellationToken, ValueTask<IAsyncDisposable?>> subscribe) =>
        AsyncObservable.Create<T>(subscribe!);

    [AdaptsLibraryCall]
    private static IAsyncObservable<T> CreateAutoCompletingImpl<T>(
        Func<IAsyncObserver<T>, ValueTask> subscribe) =>
        AsyncObservable.Create<T>((o, _) => subscribe(o));

    private static IAsyncObservable<T> CreateAutoCompletingWithTokenImpl<T>(
        Func<IAsyncObserver<T>, CancellationToken, ValueTask> subscribe) =>
        AsyncObservable.Create<T>(subscribe);

    // The continuations here are awaits, which run inline on the thread that completes the
    // callback's task; a current task scheduler other than the default would send them to the
    // thread pool instead, which is what the original's wrapper would do here.
    public void RunSynchronously(Action body)
    {
        ArgumentNullException.ThrowIfNull(body);

        body();
    }

    private static Func<ValueTask>? AsDispose(Action? action) =>
        action is null
            ? null
            : () =>
            {
                action();
                return default;
            };
}
