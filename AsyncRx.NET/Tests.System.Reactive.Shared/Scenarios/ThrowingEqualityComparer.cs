// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's <c>ThrowingComparer</c> from <c>SequenceEqualTest.cs</c>: throws when its first
/// operand is a given value, and compares by equality otherwise.
/// </summary>
/// <param name="throwOn">The first operand that throws.</param>
/// <param name="ex">The exception thrown for it.</param>
/// <remarks>
/// Named <c>ThrowingEqualityComparer</c> here because the suite already holds the
/// <see cref="ThrowingComparer{T}"/> of the <c>Max</c> and <c>Min</c> scenarios, an
/// <see cref="IComparer{T}"/> that always throws, under Rx.NET's name for that one.
/// </remarks>
public sealed class ThrowingEqualityComparer(int throwOn, Exception ex) : IEqualityComparer<int>
{
    /// <inheritdoc/>
    public bool Equals(int x, int y)
    {
        if (x == throwOn)
        {
            throw ex;
        }

        return x == y;
    }

    /// <inheritdoc/>
    public int GetHashCode(int obj)
    {
        return obj.GetHashCode();
    }
}
