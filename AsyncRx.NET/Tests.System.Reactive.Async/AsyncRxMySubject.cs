// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async;

/// <summary>AsyncRx.NET's object behind the <see cref="MySubject"/> double.</summary>
/// <remarks>
/// The behaviour that type documents, recording into and reading from the shared state.
/// </remarks>
internal sealed class AsyncRxMySubject(MySubject.State state) : IAsyncSubject<int>
{
    private readonly List<IAsyncObserver<int>> _observers = [];

    public async ValueTask OnNextAsync(int value)
    {
        foreach (var observer in _observers.ToArray())
        {
            await observer.OnNextAsync(value);
        }

        if (state.DisposeOn.TryGetValue(value, out var disconnect))
        {
            await disconnect.DisposeAsync();
        }
    }

    public async ValueTask OnErrorAsync(Exception error)
    {
        foreach (var observer in _observers.ToArray())
        {
            await observer.OnErrorAsync(error);
        }
    }

    public async ValueTask OnCompletedAsync()
    {
        foreach (var observer in _observers.ToArray())
        {
            await observer.OnCompletedAsync();
        }
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<int> observer)
    {
        state.SubscribeCount++;
        _observers.Add(observer);
        return new(AsyncDisposable.Create(() =>
        {
            _observers.Remove(observer);
            state.Disposed = true;
            return default;
        }));
    }
}
