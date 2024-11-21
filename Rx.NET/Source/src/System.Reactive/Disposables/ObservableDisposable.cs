// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Disposables;
using System;
using System.Reactive.Subjects;
using System.Threading;

/// <summary>
/// Represents a disposable resource that can be observed
/// </summary>
public sealed class ObservableDisposable : ICancelable, IObservable<Unit>
{
    private Subject<Unit> _disposedNotification = new();
    private bool _disposed;

    /// <inheritdoc/>
    public bool IsDisposed => Volatile.Read(ref _disposed);

    /// <inheritdoc/>
    public IDisposable Subscribe(IObserver<Unit> observer)
        => _disposedNotification.Subscribe(observer);

    /// <inheritdoc/>
    public void Dispose()
    {
        if (Volatile.Read(ref _disposed) == false)
        {
            _disposedNotification.OnNext(Unit.Default);
            _disposedNotification.OnCompleted();
            _disposedNotification.Dispose();
            Volatile.Write(ref _disposed, true);
            _disposedNotification = null!;
        }
        GC.SuppressFinalize(this);
    }
}
