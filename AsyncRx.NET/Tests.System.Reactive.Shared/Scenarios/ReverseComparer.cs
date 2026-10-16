// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>A comparer that reverses another's order.</summary>
/// <typeparam name="T">The type of the objects to compare.</typeparam>
/// <param name="comparer">The comparer whose order to reverse.</param>
/// <remarks>
/// Rx.NET's <c>ReverseComparer</c>, shared by its <c>Min</c> and <c>Max</c> tests, so a
/// top-level type here.
/// </remarks>
public sealed class ReverseComparer<T>(IComparer<T> comparer) : IComparer<T>
{
    /// <inheritdoc/>
    public int Compare(T? x, T? y) => -comparer.Compare(x, y);
}
