// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>E</c>, an element type whose equality counts the exact type.</summary>
/// <remarks>
/// Declared in Rx.NET's <c>CastTest.cs</c> for the <c>Cast</c> and <c>OfType</c> tests, which
/// subscribe sources of objects and expect elements by type as well as by id; a top-level type
/// here for the same reason. It stands apart from the <c>A</c> hierarchy.
/// </remarks>
internal class E : IEquatable<E>
{
    private readonly int _id;

    public E(int id)
    {
        _id = id;
    }

    public bool Equals(E? other)
    {
        if (other == null)
        {
            return false;
        }

        return _id == other._id && GetType().Equals(other.GetType());
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as E);
    }

    public override int GetHashCode()
    {
        return _id;
    }
}
