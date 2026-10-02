// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A disposable that records whether it has been disposed.</summary>
/// <remarks>Where the Rx.NET test has <c>BooleanDisposable</c>.</remarks>
public sealed class BooleanAsyncDisposable : IAsyncDisposable
{
    /// <summary>Whether <see cref="DisposeAsync"/> has been called.</summary>
    public bool IsDisposed { get; private set; }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        IsDisposed = true;
        return default;
    }
}
