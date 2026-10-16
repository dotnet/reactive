// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Rx.NET's object behind the <c>RefCount</c> tests' connectable double.</summary>
/// <remarks>
/// The behaviour <see cref="RefCountTests.SerialSingleNotificationConnectable{T}"/> documents,
/// over <see cref="Subject{T}"/>, recording into the shared state.
/// </remarks>
internal sealed class RxSerialSingleNotificationConnectable<T> : IConnectableObservable<T>
{
    private readonly RefCountTests.SerialSingleNotificationConnectable<T>.State _state;
    private readonly RxTarget _target;
    private readonly object _gate = new();
    private Subject<T> _sourceForNextConnect = new();
    private (RefCountTests.SerialSingleNotificationConnectable<T>.Connection Record, Subject<T> Source)? _active;

    public RxSerialSingleNotificationConnectable(RefCountTests.SerialSingleNotificationConnectable<T>.State state, RxTarget target)
    {
        _state = state;
        _target = target;
        state.DeliverToActive = notification =>
        {
            Subject<T> source;
            lock (_gate)
            {
                if (_active is not { Record.Disposed: false } active)
                {
                    throw new InvalidOperationException("No connection is currently active");
                }

                if (active.Record.ReplacedSource is not null)
                {
                    throw new InvalidOperationException("Active connection's source has been replaced and is no longer a Subject<T>, so it is not possible to deliver further notifications to current subscribers");
                }

                source = active.Source;
            }

            notification.Accept(source);
            return default;
        };
    }

    public IDisposable Connect()
    {
        Subject<T> source;
        Notification<T> notification;
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        lock (_gate)
        {
            source = _sourceForNextConnect;
            notification = _state.Next;
            _sourceForNextConnect = new Subject<T>();
            _active = (record, source);
            _state.Connections.Add(record);
        }

        notification.Accept(source);
        return new Connection(record);
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        IObservable<T> source;
        lock (_gate)
        {
            source = _active is { Record.Disposed: false } active
                ? active.Record.ReplacedSource is { } replaced ? _target.MaterializeForDouble(replaced) : active.Source
                : _sourceForNextConnect;
        }

        return source.Subscribe(observer);
    }

    private sealed class Connection(RefCountTests.SerialSingleNotificationConnectable<T>.Connection record) : IDisposable
    {
        public void Dispose() => record.Disposed = true;
    }
}
