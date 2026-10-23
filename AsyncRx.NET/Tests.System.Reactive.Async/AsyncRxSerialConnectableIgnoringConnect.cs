// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async;

/// <summary>
/// AsyncRx.NET's object behind the <c>RefCount</c> tests' connect-ignoring double.
/// </summary>
/// <remarks>
/// The behaviour <see cref="RefCountTests.SerialConnectableIgnoringConnect{T}"/> documents:
/// subscriptions go to the state's current source, and <c>ConnectAsync()</c> only logs.
/// </remarks>
internal sealed class AsyncRxSerialConnectableIgnoringConnect<T>(
    RefCountTests.SerialConnectableIgnoringConnect<T>.State state,
    AsyncRxTarget target) : IConnectableAsyncObservable<T>
{
    public ValueTask<IAsyncDisposable> ConnectAsync()
    {
        var record = new RefCountTests.SerialSingleNotificationConnectable<T>.Connection();
        state.Connections.Add(record);
        return new(new Connection(record));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer) =>
        target.MaterializeForDouble(state.Source).SubscribeAsync(observer);

    private sealed class Connection(RefCountTests.SerialSingleNotificationConnectable<T>.Connection record) : IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            record.Disposed = true;
            return default;
        }
    }
}
