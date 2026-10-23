// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Rx.NET's object behind the <see cref="MySubject"/> double.</summary>
/// <remarks>
/// The behaviour that type documents, recording into and reading from the shared state. A
/// disposable registered through <c>DisposeOn</c> is one this target handed out, so disposing
/// it completes synchronously, as everything on this target does.
/// </remarks>
internal sealed class RxMySubject(MySubject.State state) : ISubject<int>
{
    private readonly List<IObserver<int>> _observers = [];

    public void OnNext(int value)
    {
        foreach (var observer in _observers.ToArray())
        {
            observer.OnNext(value);
        }

        if (state.DisposeOn.TryGetValue(value, out var disconnect))
        {
            RxTarget.Complete(disconnect.DisposeAsync());
        }
    }

    public void OnError(Exception error)
    {
        foreach (var observer in _observers.ToArray())
        {
            observer.OnError(error);
        }
    }

    public void OnCompleted()
    {
        foreach (var observer in _observers.ToArray())
        {
            observer.OnCompleted();
        }
    }

    public IDisposable Subscribe(IObserver<int> observer)
    {
        state.SubscribeCount++;
        _observers.Add(observer);
        return Disposable.Create(() =>
        {
            _observers.Remove(observer);
            state.Disposed = true;
        });
    }
}
