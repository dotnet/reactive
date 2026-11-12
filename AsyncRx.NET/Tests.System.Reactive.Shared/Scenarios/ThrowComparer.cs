// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>ThrowComparer</c> from <c>SequenceEqualTest.cs</c>: every comparison throws.</summary>
/// <param name="ex">The exception every comparison throws.</param>
public sealed class ThrowComparer(Exception ex) : IEqualityComparer<int>
{
    /// <inheritdoc/>
    public bool Equals(int x, int y)
    {
        throw ex;
    }

    /// <inheritdoc/>
    public int GetHashCode(int obj)
    {
        throw new NotImplementedException();
    }
}
