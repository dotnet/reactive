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
internal sealed class RxSerialSingleNotificationConnectable<T>(RefCountTests.SerialSingleNotificationConnectable<T>.State state)
    : IConnectableObservable<T>
{
    private readonly object _gate = new();
    private Subject<T> _sourceForNextConnect = new();
    private (RefCountTests.SerialSingleNotificationConnectable<T>.Connection Record, Subject<T> Source)? _active;

    public IDisposable Connect()
    {
        Subject<T> source;
        Notification<T> notification;
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        lock (_gate)
        {
            source = _sourceForNextConnect;
            notification = state.Next;
            _sourceForNextConnect = new Subject<T>();
            _active = (record, source);
            state.Connections.Add(record);
        }

        notification.Accept(source);
        return new Connection(record);
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        Subject<T> source;
        lock (_gate)
        {
            source = _active is { Record.Disposed: false } active ? active.Source : _sourceForNextConnect;
        }

        return source.Subscribe(observer);
    }

    private sealed class Connection(RefCountTests.SerialSingleNotificationConnectable<T>.Connection record) : IDisposable
    {
        public void Dispose() => record.Disposed = true;
    }
}
