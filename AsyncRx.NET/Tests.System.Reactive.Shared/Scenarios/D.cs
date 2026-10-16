// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>D</c>, derived from <see cref="B"/>.</summary>
/// <remarks>
/// Declared in Rx.NET's <c>CastTest.cs</c> for the <c>Cast</c> and <c>OfType</c> tests; a
/// top-level type here for the same reason.
/// </remarks>
internal class D : B
{
    public D(int id)
        : base(id)
    {
    }
}
