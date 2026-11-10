// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Which of a target's two operator classes an argument check looks at.</summary>
/// <remarks>
/// A method name can mean different things on the two classes: Rx.NET's <c>Subscribe</c> is the
/// enumerable form on <c>Observable</c> and the handler-based family on
/// <c>ObservableExtensions</c>. A test that must tell them apart scopes its check; most do not.
/// </remarks>
public enum SurfaceClass
{
    /// <summary>Both classes.</summary>
    Any,

    /// <summary>The operator class: <c>Observable</c> or <c>AsyncObservable</c>.</summary>
    Operators,

    /// <summary>
    /// The extensions class: <c>ObservableExtensions</c> or <c>AsyncObservableExtensions</c>.
    /// </summary>
    Extensions,
}
