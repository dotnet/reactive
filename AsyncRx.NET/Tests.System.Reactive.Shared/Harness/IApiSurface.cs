// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>What the argument checks need to know about a target's operator surface.</summary>
/// <remarks>
/// The checks walk the public static methods of <see cref="Operators"/> by reflection; the other
/// two members tell them what an observable is on this target and how to subscribe to one. See
/// <see cref="ArgumentChecks"/>.
/// </remarks>
public interface IApiSurface
{
    /// <summary>The static class the operators are declared on.</summary>
    /// <remarks><c>Observable</c> on Rx.NET, <c>AsyncObservable</c> on AsyncRx.NET.</remarks>
    Type Operators { get; }

    /// <summary>The target's observable interface, as an open generic definition.</summary>
    Type ObservableDefinition { get; }

    /// <summary>The name of the observable interface's subscribe method.</summary>
    /// <remarks><c>Subscribe</c> on Rx.NET, <c>SubscribeAsync</c> on AsyncRx.NET.</remarks>
    string SubscribeMethodName { get; }
}
