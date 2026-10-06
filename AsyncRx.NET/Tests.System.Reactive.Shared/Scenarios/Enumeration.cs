// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// One enumeration of a <see cref="MockEnumerable{T}"/>: when it began and ended.
/// </summary>
/// <param name="Start">The tick at which the enumerator was obtained.</param>
/// <param name="End">
/// The tick at which the enumerator was disposed, or <see cref="Infinite"/> while it is still
/// open.
/// </param>
/// <remarks>
/// The enumerable counterpart of a recorded subscription, with the same two ticks, so a scenario
/// asserts on how an operator enumerated what a selector returned.
/// </remarks>
public readonly record struct Enumeration(long Start, long End)
{
    /// <summary>The <see cref="End"/> of an enumeration that has not been disposed.</summary>
    public const long Infinite = long.MaxValue;

    /// <summary>An enumeration that began at <paramref name="start"/> and is still open.</summary>
    /// <param name="start">The tick at which the enumerator was obtained.</param>
    public Enumeration(long start)
        : this(start, Infinite)
    {
    }

    /// <inheritdoc/>
    public override string ToString() =>
        End == Infinite ? $"({Start}, Infinite)" : $"({Start}, {End})";
}
