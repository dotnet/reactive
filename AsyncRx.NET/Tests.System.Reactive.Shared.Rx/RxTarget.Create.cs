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

    // Every form realizes through Rx.NET's task-based Create overloads, the callback's
    // ValueTask handed over as a Task. A callback that completed synchronously runs exactly as
    // Rx.NET's synchronous Create would run it, and one that did not runs as Rx.NET's own
    // asynchronous callbacks do: Subscribe has returned, the resource is attached when the task
    // produces it, and a fault reaches OnError.

    [AdaptsLibraryCall]
    private static IObservable<T> CreateImpl<T>(Func<IObserver<T>, ValueTask<Action?>> subscribe) =>
        Observable.Create<T>(o => subscribe(o).AsTask()!);

    [AdaptsLibraryCall]
    private static IObservable<T> CreateDisposableImpl<T>(
        Func<IObserver<T>, ValueTask<IAsyncDisposable?>> subscribe) =>
        Observable.Create<T>(o => ToDisposableAsync(subscribe(o)));

    [AdaptsLibraryCall]
    private static IObservable<T> CreateWithTokenImpl<T>(
        Func<IObserver<T>, CancellationToken, ValueTask<Action?>> subscribe) =>
        Observable.Create<T>((o, token) => subscribe(o, token).AsTask()!);

    [AdaptsLibraryCall]
    private static IObservable<T> CreateDisposableWithTokenImpl<T>(
        Func<IObserver<T>, CancellationToken, ValueTask<IAsyncDisposable?>> subscribe) =>
        Observable.Create<T>((o, token) => ToDisposableAsync(subscribe(o, token)));

    [AdaptsLibraryCall]
    private static IObservable<T> CreateAutoCompletingImpl<T>(
        Func<IObserver<T>, ValueTask> subscribe) =>
        Observable.Create<T>(o => subscribe(o).AsTask());

    [AdaptsLibraryCall]
    private static IObservable<T> CreateAutoCompletingWithTokenImpl<T>(
        Func<IObserver<T>, CancellationToken, ValueTask> subscribe) =>
        Observable.Create<T>((o, token) => subscribe(o, token).AsTask());

    public void RunSynchronously(Action body)
    {
        ArgumentNullException.ThrowIfNull(body);

        var t = new Task(body);
        t.RunSynchronously(new SynchronousScheduler());
        t.Wait();
    }

    // The callback's disposable as the IDisposable Rx.NET's Create takes; its DisposeAsync
    // completes synchronously on this target, as Complete checks.
    private static async Task<IDisposable> ToDisposableAsync(
        ValueTask<IAsyncDisposable?> resource) =>
        await resource is { } disposable
            ? Disposable.Create(() => Complete(disposable.DisposeAsync()))
            : null!;
}
