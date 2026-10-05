// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async;

/// <summary>AsyncRx.NET's object behind the <c>RefCount</c> tests' connectable double.</summary>
/// <remarks>
/// The behaviour <see cref="RefCountTests.SerialSingleNotificationConnectable{T}"/> documents,
/// over <see cref="SequentialSimpleAsyncSubject{T}"/>, recording into the shared state.
/// </remarks>
internal sealed class AsyncRxSerialSingleNotificationConnectable<T>(RefCountTests.SerialSingleNotificationConnectable<T>.State state)
    : IConnectableAsyncObservable<T>
{
    private readonly object _gate = new();
    private SequentialSimpleAsyncSubject<T> _sourceForNextConnect = new();
    private (RefCountTests.SerialSingleNotificationConnectable<T>.Connection Record, SequentialSimpleAsyncSubject<T> Source)? _active;

    public async ValueTask<IAsyncDisposable> ConnectAsync()
    {
        SequentialSimpleAsyncSubject<T> source;
        Notification<T> notification;
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        lock (_gate)
        {
            source = _sourceForNextConnect;
            notification = state.Next;
            _sourceForNextConnect = new SequentialSimpleAsyncSubject<T>();
            _active = (record, source);
            state.Connections.Add(record);
        }

        await (notification.Kind switch
        {
            NotificationKind.OnNext => source.OnNextAsync(notification.Value),
            NotificationKind.OnError => source.OnErrorAsync(notification.Exception!),
            _ => source.OnCompletedAsync(),
        });

        return new Connection(record);
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
    {
        SequentialSimpleAsyncSubject<T> source;
        lock (_gate)
        {
            source = _active is { Record.Disposed: false } active ? active.Source : _sourceForNextConnect;
        }

        return source.SubscribeAsync(observer);
    }

    private sealed class Connection(RefCountTests.SerialSingleNotificationConnectable<T>.Connection record) : IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            record.Disposed = true;
            return default;
        }
    }
}
