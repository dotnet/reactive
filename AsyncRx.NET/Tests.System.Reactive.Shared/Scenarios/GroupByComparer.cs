// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// A case-insensitive string comparer that can be told to start throwing at a tick.
/// </summary>
/// <param name="scheduler">The test's scheduler, whose clock decides when to throw.</param>
/// <param name="equalsThrowsAfter">The tick after which <see cref="Equals"/> throws.</param>
/// <param name="getHashCodeThrowsAfter">
/// The tick after which <see cref="GetHashCode"/> throws.
/// </param>
/// <remarks>
/// Rx.NET's <c>GroupByComparer</c> from <c>GroupByTest.cs</c>, over the shared scheduler handle.
/// The <c>GroupBy</c> scenarios use it to drive errors out of the comparer at a chosen time.
/// </remarks>
internal sealed class GroupByComparer(
    TestSchedulerRef scheduler,
    ushort equalsThrowsAfter,
    ushort getHashCodeThrowsAfter) : IEqualityComparer<string>
{
    /// <summary>The exception <see cref="GetHashCode"/> throws once its tick has passed.</summary>
    public Exception HashCodeException { get; } = new();

    /// <summary>The exception <see cref="Equals"/> throws once its tick has passed.</summary>
    public Exception EqualsException { get; } = new();

    /// <summary>A comparer that never throws.</summary>
    /// <param name="scheduler">The test's scheduler.</param>
    public GroupByComparer(TestSchedulerRef scheduler)
        : this(scheduler, ushort.MaxValue, ushort.MaxValue)
    {
    }

    /// <inheritdoc/>
    public bool Equals(string? x, string? y)
    {
        if (scheduler.Clock > equalsThrowsAfter)
        {
            throw EqualsException;
        }

        return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc/>
    public int GetHashCode(string obj)
    {
        if (scheduler.Clock > getHashCodeThrowsAfter)
        {
            throw HashCodeException;
        }

        return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
    }
}
