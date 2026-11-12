// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Rx.NET's object behind the <c>RefCount</c> tests' connect-ignoring double.</summary>
/// <remarks>
/// The behaviour <see cref="RefCountTests.SerialConnectableIgnoringConnect{T}"/> documents:
/// subscriptions go to the state's current source, and <c>Connect()</c> only logs.
/// </remarks>
internal sealed class RxSerialConnectableIgnoringConnect<T>(
    RefCountTests.SerialConnectableIgnoringConnect<T>.State state,
    RxTarget target) : IConnectableObservable<T>
{
    public IDisposable Connect()
    {
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        state.Connections.Add(record);
        return new Connection(record);
    }

    public IDisposable Subscribe(IObserver<T> observer) =>
        target.MaterializeForDouble(state.Source).Subscribe(observer);

    private sealed class Connection(RefCountTests.SerialSingleNotificationConnectable<T>.Connection record) : IDisposable
    {
        public void Dispose() => record.Disposed = true;
    }
}
