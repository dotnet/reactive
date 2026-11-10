// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Rx.NET's disposable that records the virtual time of its creation and disposal.
/// </summary>
/// <remarks>
/// From Rx.NET's <c>MockDisposable.cs</c>, shared by its test files; a top-level type here for
/// the same reason. It is the list of those times, the creation's first, over the shared test
/// scheduler's clock, and <see cref="AssertEqual"/> stands in for the <c>ReactiveAssert</c>
/// extension the originals compare it with.
/// </remarks>
public sealed class MockDisposable : List<long>, IDisposable
{
    private readonly TestSchedulerRef _scheduler;

    /// <summary>Creates the disposable and records the time.</summary>
    /// <param name="scheduler">The scheduler whose clock the times are read from.</param>
    public MockDisposable(TestSchedulerRef scheduler)
    {
        ArgumentNullException.ThrowIfNull(scheduler);
        _scheduler = scheduler;
        Add(scheduler.Clock);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Add(_scheduler.Clock);
    }

    /// <summary>Asserts the recorded times are exactly <paramref name="expected"/>.</summary>
    /// <param name="expected">The expected times, the creation's first.</param>
    public void AssertEqual(params long[] expected)
    {
        CollectionAssert.AreEqual(expected, this);
    }
}
