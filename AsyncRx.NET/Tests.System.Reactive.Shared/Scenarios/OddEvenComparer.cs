// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>OddEvenComparer</c> from <c>SequenceEqualTest.cs</c>: equal when of the same parity.</summary>
public sealed class OddEvenComparer : IEqualityComparer<int>
{
    /// <inheritdoc/>
    public bool Equals(int x, int y)
    {
        return x % 2 == y % 2;
    }

    /// <inheritdoc/>
    public int GetHashCode(int obj)
    {
        return (obj % 2).GetHashCode();
    }
}
