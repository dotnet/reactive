// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A disposable that does nothing, for a scenario to return from a callback.</summary>
/// <remarks>Where the Rx.NET test has <c>Disposable.Empty</c>.</remarks>
public sealed class EmptyAsyncDisposable : IAsyncDisposable
{
    /// <summary>The one instance.</summary>
    public static EmptyAsyncDisposable Instance { get; } = new();

    private EmptyAsyncDisposable()
    {
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync() => default;
}
