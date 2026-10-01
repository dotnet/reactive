// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

/// <summary>AsyncRx.NET's operator surface, for the argument checks.</summary>
public sealed class AsyncRxApiSurface : IApiSurface
{
    public static AsyncRxApiSurface Instance { get; } = new();

    private AsyncRxApiSurface()
    {
    }

    /// <inheritdoc/>
    public Type Operators => typeof(AsyncObservable);

    /// <inheritdoc/>
    public Type ObservableDefinition => typeof(IAsyncObservable<>);

    /// <inheritdoc/>
    public string SubscribeMethodName => nameof(IAsyncObservable<int>.SubscribeAsync);
}
