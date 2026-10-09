// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async;

/// <summary>AsyncRx.NET's object behind the <c>RefCount</c> tests' connectable double.</summary>
/// <remarks>
/// The behaviour <see cref="RefCountTests.SerialSingleNotificationConnectable{T}"/> documents,
/// over <see cref="SequentialSimpleAsyncSubject{T}"/>, recording into the shared state.
/// </remarks>
internal sealed class AsyncRxSerialSingleNotificationConnectable<T> : IConnectableAsyncObservable<T>
{
    private readonly RefCountTests.SerialSingleNotificationConnectable<T>.State _state;
    private readonly AsyncRxTarget _target;
    private readonly object _gate = new();
    private SequentialSimpleAsyncSubject<T> _sourceForNextConnect = new();
    private (RefCountTests.SerialSingleNotificationConnectable<T>.Connection Record, SequentialSimpleAsyncSubject<T> Source)? _active;

    public AsyncRxSerialSingleNotificationConnectable(RefCountTests.SerialSingleNotificationConnectable<T>.State state, AsyncRxTarget target)
    {
        _state = state;
        _target = target;
        state.DeliverToActive = notification =>
        {
            SequentialSimpleAsyncSubject<T> source;
            lock (_gate)
            {
                if (_active is not { Record.Disposed: false } active)
                {
                    throw new InvalidOperationException("No connection is currently active");
                }

                if (active.Record.ReplacedSource is not null)
                {
                    throw new InvalidOperationException("Active connection's source has been replaced and is no longer a subject, so it is not possible to deliver further notifications to current subscribers");
                }

                source = active.Source;
            }

            return Deliver(source, notification);
        };
    }

    private static ValueTask Deliver(SequentialSimpleAsyncSubject<T> source, Notification<T> notification) =>
        notification.Kind switch
        {
            NotificationKind.OnNext => source.OnNextAsync(notification.Value),
            NotificationKind.OnError => source.OnErrorAsync(notification.Exception!),
            _ => source.OnCompletedAsync(),
        };

    public async ValueTask<IAsyncDisposable> ConnectAsync()
    {
        SequentialSimpleAsyncSubject<T> source;
        Notification<T> notification;
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        lock (_gate)
        {
            source = _sourceForNextConnect;
            notification = _state.Next;
            _sourceForNextConnect = new SequentialSimpleAsyncSubject<T>();
            _active = (record, source);
            _state.Connections.Add(record);
        }

        await Deliver(source, notification);
        return new Connection(record);
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
    {
        IAsyncObservable<T> source;
        lock (_gate)
        {
            source = _active is { Record.Disposed: false } active
                ? active.Record.ReplacedSource is { } replaced ? _target.MaterializeForDouble(replaced) : active.Source
                : _sourceForNextConnect;
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
