// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>Rx.NET's operator surface, for the argument checks.</summary>
public sealed class RxApiSurface : IApiSurface
{
    public static RxApiSurface Instance { get; } = new();

    private RxApiSurface()
    {
    }

    /// <inheritdoc/>
    public Type Operators => typeof(Observable);

    /// <inheritdoc/>
    public Type Extensions => typeof(ObservableExtensions);

    /// <inheritdoc/>
    public Type ObservableDefinition => typeof(IObservable<>);

    /// <inheritdoc/>
    public string SubscribeMethodName => nameof(IObservable<int>.Subscribe);
}
